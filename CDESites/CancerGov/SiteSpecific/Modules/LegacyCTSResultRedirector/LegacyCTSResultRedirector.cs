using System;
using System.Collections.Generic;
using System.Configuration;
using System.Text.RegularExpressions;
using System.Web;
using Common.Logging;

using NCI.Web.CDE.Modules;
using CancerGov.ClinicalTrials.Basic.v2;

namespace CancerGov.HttpModules
{
    /// <summary>
    /// This module is to map old clinical trial URLs that were pre-Devon Rex launch
    /// to post devon rex URLs.  The ProtocolSearchIDs have to be remapped for these
    /// searches.
    /// 
    /// The URLs below should be put in configuration and maybe that will be a CTS Phase-1
    /// tasks.
    /// </summary>
    public class LegacyCTSResultRedirector : IHttpModule
    {
        #region IHttpModule Members

        static ILog log = LogManager.GetLogger(typeof(LegacyCTSResultRedirector));

        /// <summary>
        /// Gets the Snippet Controls Config.
        /// </summary>
        protected BasicCTSPageInfo Config { get; private set; }

        // Member variables 
        const String TYPE_PARAM = "t";
        const String PSID_PARAM = "protocolsearchid";

        public void Dispose()
        {
            return;
        }

        public void Init(HttpApplication context)
        {
            context.BeginRequest += new EventHandler(OnBeginRequest);
            string configPath = ConfigurationManager.AppSettings["CTSConfigFilePath"];
            Config = ModuleObjectFactory<BasicCTSPageInfo>.GetObjectFromFile(configPath);
        }

        #endregion

        [Obsolete("Deprecated function for legacy search functionality; needs to be removed eventually.", false)]
        void OnBeginRequest(object sender, EventArgs e)
        {
            /**
             * TODO: handle the following path 90 days after 2017-09-15
             * /about-cancer/treatment/clinical-trials/search/printresults
             */
            // First we need to load the URL map.
            HttpContext context = ((HttpApplication)sender).Context;

            // Get absolute path of the request URL as pretty URL
            string url = context.Server.UrlDecode(context.Request.Url.AbsolutePath);
            string parms = context.Server.UrlDecode(context.Request.Url.Query);
            string reqType = context.Request.HttpMethod.ToLower();

            // Get various page URLs from configuration 
            string ctsAdvSearchPage = Config.AdvSearchPagePrettyUrl;
            string ctsResultsPage = Config.ResultsPagePrettyUrl;
            string ctsDetailsPage = Config.DetailedViewPagePrettyUrl;
            string ctsRedirPage = Config.RedirectPagePrettyUrl;

            //If this is the homepage, then exit.
            if (url == "/")
                return;

            //Chop off any trailing slashes
            if (url.EndsWith("/"))
                url = url.Substring(0, url.Length - 1);

            // Redirect pre-NVCG search URLs to advanced search
            if (
                url.Equals("/clinicaltrials/search/results", StringComparison.OrdinalIgnoreCase) ||
                url.Equals("/clinicaltrials/search/printresults", StringComparison.OrdinalIgnoreCase)
                )
            {
                DoPermanentRedirect(context.Response, ctsAdvSearchPage);
            }

            // Redirect old Advanced Search Results to search options content page
            if (url.Equals("/about-cancer/treatment/clinical-trials/search/results", StringComparison.OrdinalIgnoreCase))
            {
                DoPermanentRedirect(context.Response, ctsRedirPage);
            }

            // Redirect old Advanced Search form page:
            // 1. If protocolsearchid query is present, redirect to search options content page
            // 2. Otherwise, if a POST request is made, redirect to current Advanced Search form
            if (url.Equals(ctsAdvSearchPage, StringComparison.OrdinalIgnoreCase))
            {
                if (parms.ToLower().IndexOf(PSID_PARAM) > -1)
                {
                    DoPermanentRedirect(context.Response, ctsRedirPage);
                }
                // E.g. if a user is on a cached version of the legacy Advanced Search page and then does a postBack call by selecting a 
                // Type/Condition, redirect to the current Advanced Search page
                else if (reqType == "post")
                {
                    DoPermanentRedirect(context.Response, ctsAdvSearchPage);
                }
            }

            // Clean up cancer type parameter ('t') from legacy Basic CTS on Results and View Details pages
            if (url.Equals(ctsResultsPage, StringComparison.OrdinalIgnoreCase) || url.Equals(ctsDetailsPage, StringComparison.OrdinalIgnoreCase))
            {
                // Check if parameter exists
                if (!string.IsNullOrWhiteSpace(context.Request.QueryString[TYPE_PARAM]))
                {
                    DoLegacyBasicRedirect(context, url, parms);
                }
            }

        }

        /// <summary>
        /// Checks for the old "CXXX|cancer_type_name" pattern in the Cancer Type query parameter, cleans up the parameter, 
        /// and redirects to a URL with the current query parameter value. 
        /// </summary>
        /// <param name="context"></param>
        /// <param name="url"></param>
        /// <param name="parms"></param>
        private void DoLegacyBasicRedirect(HttpContext context, string url, string parms)
        {
            var values = HttpUtility.ParseQueryString(parms);
            string typeValue = values.Get(TYPE_PARAM);

            // Check that the parameter has values. If so, run it through the cleanup logic.
            try
            {
                // Get an array of "type" parameter values.
                string[] typeValArray = SplitArrayOnPipes(typeValue);

                // If the array length == 1, it's just a C-code, which means it's fine.
                // If the array length > 2, it means that the array will be multiple C-codes
                // separated by multiple pipes; this is new functionality and is also fine. 
                // But...
                if (typeValArray.Length == 2)
                {
                    // If the right side of the array matches the regex, strip it out.
                    // Then, replace any commas with a pipe and do the redirect. 
                    if (IsLegacyValue(typeValArray[1]))
                    {
                        typeValue = typeValArray[0].Replace(",", "|");
                        values.Set(TYPE_PARAM, typeValue);
                        DoPermanentRedirect(context.Response, url + "?" + values);
                    }
                }
            }
            catch(NullReferenceException ex)
            {
                string errorMessage = "LegacyCTSResultRedirector.cs:DoLegacyBasicRedirect() error setting legacy redirect for \"" + TYPE_PARAM + "\" query parameter";
                log.Error(errorMessage, ex);
            }
        }

        /// <summary>
        /// Returns an array of values from a pipe-delimited string.
        /// </summary>
        /// <param name="val"></param>
        /// <returns></returns>
        public String[] SplitArrayOnPipes(string val)
        {
            List<string> vals = new List<string>();
            string[] valArray = vals.ToArray();
            char[] delimiterChars = { '|' };

            // Check that the parameter has a value. 
            if(!string.IsNullOrWhiteSpace(val))
            {
                // Create an array of pipe-separated values.
                valArray = val.Split(delimiterChars);
            }
            return valArray;
        }

        /// <summary>
        /// Checks for a non-C-Code pattern in a given string.
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public bool IsLegacyValue(string str)
        {
            // Regex pattern: any string that contains an underscore or a letter other than "c".
            string pattern = @"[ab-zAB-Z_]$";
            Regex rgx = new Regex(pattern);
            return rgx.IsMatch(str);
        }

        /// <summary>
        /// Clears the Response text, issues an HTTP redirect using status 301, and ends
        /// the current request.
        /// </summary>
        /// <param name="Response">The current response object.</param>
        /// <param name="url">The redirection's target URL.</param>
        /// <remarks>Response.Redirect() issues its redirect with a 301 (temporarily moved) status code.
        /// We want these redirects to be permanent so search engines will link to the new
        /// location. Unfortunately, HttpResponse.RedirectPermanent() isn't implemented until
        /// at version 4.0 of the .NET Framework.</remarks>
        /// <exception cref="ThreadAbortException">Called when the redirect takes place and the current
        /// request is ended.</exception>
        private void DoPermanentRedirect(HttpResponse Response, String url)
        {
            Response.Clear();
            Response.Status = "301 Moved Permanently";
            Response.AddHeader("Location", url);
            Response.End();
        }
    }
}
