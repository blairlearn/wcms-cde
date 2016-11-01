using System;
using System.Configuration;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using Common.Logging;

using CancerGov.ClinicalTrialsAPI;
using CancerGov.ClinicalTrials.Basic.v2.Configuration;

using NCI.Web.CDE;

namespace CancerGov.ClinicalTrials.Basic.v2
{
    // NCTIDRedirect module 
    public class NCTIDRedirectModule : IHttpModule
    {
        static ILog log = LogManager.GetLogger(typeof(NCTIDRedirectModule));

        protected string _APIURL = "";

        /// <summary>
        /// Gets the URL for the ClinicalTrials API from BasicClinicalTrialSearchAPISection:GetAPIUrl()
        /// TODO: clean up this property
        /// </summary>
        protected string APIURL
        {
            get
            {
                if (String.IsNullOrWhiteSpace(_APIURL))
                {
                    this._APIURL = BasicClinicalTrialSearchAPISection.GetAPIUrl();
                }

                return this._APIURL;
            }
        }

        private string SearchResultsPrettyUrl
        {
            get
            {
                if (!String.IsNullOrEmpty(ConfigurationManager.AppSettings["ClinicalTrialsViewPage"]))
                {
                    return ConfigurationManager.AppSettings["ClinicalTrialsViewPage"];
                }
                return "/about-cancer/treatment/clinical-trials/search/v";
            }
        }

        /// <summary>
        /// You will need to configure this module in the web.config file of your
        /// web and register it with IIS before being able to use it. For more information
        /// see the following link: http://go.microsoft.com/?linkid=8101007
        /// </summary>
        #region IHttpModule Members

        public void Dispose()
        {
        }

        public void Init(HttpApplication context)
        {
            context.BeginRequest += new EventHandler(OnBeginRequest);
        }

        void OnBeginRequest(object sender, EventArgs e)
        {
            HttpContext context = ((HttpApplication)sender).Context;

            // Get absolute path of the request URL as pretty URL
            String url = context.Server.UrlDecode(context.Request.Url.AbsolutePath);

            if (url.IndexOf(".css") != -1 || url.IndexOf(".gif") != -1 || url.IndexOf(".jpg") != -1 || url.IndexOf(".js") != -1 || url.IndexOf(".axd") != -1)
                return;

            // Check if the PageAssemblyInstruction is not null. If not null then it was already processed as 
            // pretty url.
            if (PageAssemblyContext.Current.PageAssemblyInstruction != null)
                return;

            try
            {
                // The URL should match this pattern: '<hostname>/clinicaltrials/<NCTID>. If it does, proceed with retrieving the ID
                // We're only concerned about the NCT ID at this point - not NCI, CDR, or any other trial IDs
                if (context.Request.Url.Segments.Count() >= 3 &&
                    context.Request.Url.Segments[1].ToLower() == "clinicaltrials/")
                {
                    string id = string.Empty;
                    // The third segment will be the old id that we can use to look up in the database
                    id = context.Request.Url.Segments[2];

                    if (!string.IsNullOrEmpty(id))
                    {
                        /**
                         * TODO: 
                         * Refactor & specialize error handling
                         * Add "toUpper()" logic on internal redirect
                         * 
                         */
                        id = id.Replace("/", "");
                        string cleanId = id.Trim();

                        // If API has trial ID, go to page on www.cancer.gov
                        if (!string.IsNullOrEmpty(cleanId) && IsValidTrial(cleanId, APIURL))
                        {
                            string ctViewUrl = string.Format(SearchResultsPrettyUrl + "?id={0}", cleanId);
                            context.Response.Redirect(ctViewUrl, true);
                        }
                        // If it's a valid NCT ID, redirect to clinicaltrials.gov
                        // CTGov URL format is "https://clinicaltrials.gov/show/<NCT_ID>"
                        else if (IsNctID(cleanId))
                        {
                            log.DebugFormat("NCT ID {0} not found in API.", cleanId);
                            String nlmUrl = String.Format("https://clinicaltrials.gov/show/{0}", cleanId);
                            log.DebugFormat("Redirecting to {0}", nlmUrl);
                            context.Response.Redirect(nlmUrl, true);
                        }
                        // If it's not a valid NCT ID, don't do anything. This will treat the result as a page not found
                        else
                        {
                            log.DebugFormat("NCT ID {0} not found in API and is not formatted correctly for clinicaltrials.cancer.gov", cleanId);

                        }
                    }
                    else
                        log.Debug("ID is null or empty");
                }
            }
            // Response.Redirect() throws a ThreadAbortException.  This is normal behavior.
            // Hide the "normal error" by swallowing the exception.
            catch (System.Threading.ThreadAbortException)
            { }
            catch (Exception ex)
            {
                log.Error("OnBeginRequest", ex);
            }
        }


        #endregion

        /// <summary>
        /// Determines whether the value contained by a string is an NCT ID.
        /// </summary>
        /// <param name="IDString"></param>
        /// <returns></returns>
        private bool IsNctID(string IDString)
        {
            // Per http://www.nlm.nih.gov/bsd/policy/clin_trials.html, 
            // "The format for the ClinicalTrials.gov registry number is "NCT" followed by an 8-digit number, e.g.: NCT00000419."

            bool isAMatch = false;

            if (!string.IsNullOrEmpty(IDString))
            {

                isAMatch = Regex.IsMatch(IDString.Trim(), "^NCT[0-9]{1,8}$", RegexOptions.IgnoreCase);
            }

            return isAMatch;
        }

        private bool IsValidTrial(string TrialId, string host)
        {
            // Is it a valid NCTID
            // If it is valid, go to web service and see if trial exists
            // Only concerned about NCTID at the moment
            // Find link that is not to an NCTID
            try
            {
                ClinicalTrialsAPIClient client = new ClinicalTrialsAPIClient(host);
                ClinicalTrial trial = client.Get(TrialId);
                if (trial != null)
                {
                    return true;
                }
            }
            catch (Exception ex)
            {
                return false;
            }
            return false;
        }



    }
}
