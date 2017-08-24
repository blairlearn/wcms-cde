using System;
using System.Configuration;
using System.Web;
using CancerGov.ClinicalTrials.Basic.v2;
using NCI.Web.CDE.Modules;

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
        /// <summary>
        /// 
        /// </summary>
        #region IHttpModule Members

        /// <summary>
        /// Gets the Snippet Controls Config.
        /// </summary>
        protected BasicCTSPageInfo Config { get; private set; }

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

            //First we need to load the URL map.
            HttpContext context = ((HttpApplication)sender).Context;

            // Get absolute path of the request URL as pretty URL
            String url = context.Server.UrlDecode(context.Request.Url.AbsolutePath);
            String parms = context.Server.UrlDecode(context.Request.Url.Query);
            String reqType = context.Request.HttpMethod.ToLower();

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
                if (parms.ToLower().IndexOf("protocolsearchid") > -1)
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

            // Clean up cancer type parameter ('t') from legacy Basic CTS
            if (url.Equals(ctsResultsPage, StringComparison.OrdinalIgnoreCase) || url.Equals(ctsDetailsPage, StringComparison.OrdinalIgnoreCase))
            {
                // Check if parameter exists
                if (!string.IsNullOrWhiteSpace(context.Request.QueryString["t"]))
                {
                    var values = HttpUtility.ParseQueryString(parms);

                    // Now check that the parameter has a value
                    if(!string.IsNullOrWhiteSpace(values.Get("t")))
                    {
                        string oldBasicTypeValue = (values.Get("t"));
                        //values.Set("test", "true");
                        //DoPermanentRedirect(context.Response, url + "?" + values);
                    }
                }
            }

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
