using System;
using System.Configuration;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using Common.Logging;
using NCI.Web.CDE;

namespace CancerGov.ClinicalTrials.Basic.v2
{
    // NCTIDRedirect module 
    public class NCTIDRedirectModule : IHttpModule
    {
        static ILog log = LogManager.GetLogger(typeof(NCTIDRedirectModule));
        protected BasicCTSManager _basicCTSManager = null;

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
                // Access the database to get protocolid only if the request url has '/clinicaltrials/oldId'
                // In effect this request should have only three or four parts '/', 'clinicaltrials/' & 'oldid'. Examples of 
                // oldid are NCT00942578, NCI-W82-0538/patient, MDA-DT-7635A/healthprofessaional, E-4Z93, NCI-93-C-021O, UTHSC-9235011015, etc
                if (context.Request.Url.Segments.Count() >= 3 &&
                    context.Request.Url.Segments[1].ToLower() == "clinicaltrials/")
                {
                    string oldId = string.Empty;

                    // The third segment will be the old id that we can use to look up in the database
                    oldId = context.Request.Url.Segments[2];

                    if (!string.IsNullOrEmpty(oldId))
                    {
                        /**
                         * TODO: 
                         * Get API host, get clinical trial
                         * If API has trial ID, go to page on www.cancer.gov
                         * If not NCT, don't do anything
                         * Handle exceptions granularly 
                         */
                        oldId = oldId.Replace("/", "");
                        string cleanId = oldId.Trim();

                        if (!string.IsNullOrEmpty(cleanId))
                        {
                            string ctViewUrl = string.Format(SearchResultsPrettyUrl + "?id={0}", cleanId);
                            context.Response.Redirect(ctViewUrl, true);
                        }
                        else
                        {
                            log.DebugFormat("protocoloId not found in database for oldId {0}", oldId);

                            // If this is an NCT ID, redirect to the trial's page at CTGov.
                            if (IsNctID(oldId))
                            {
                                log.DebugFormat("NCTIDRedirectModule {0} is an NCT ID.", oldId);

                                // Format for a CTGov URL is https://clinicaltrials.gov/show/<<NCT_ID>>
                                String nlmUrl = String.Format("https://clinicaltrials.gov/show/{0}", oldId.Trim());

                                log.DebugFormat("Redirecting to {0}", nlmUrl);
                                context.Response.Redirect(nlmUrl, true);
                            }
                        }
                    }
                    else
                        log.Debug("oldId is null or empty");
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

        private bool IsValidTrial(string TrialId)
        {
            // Is it a valid NCTID
            // If it is valid, go to web service and see if trial exists
            // Only concerned about NCTID at the moment
            // Find link that is not to an NCTID
            return true;
        }

    }
}
