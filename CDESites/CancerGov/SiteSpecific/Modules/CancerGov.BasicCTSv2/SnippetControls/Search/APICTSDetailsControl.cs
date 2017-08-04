using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;

using Common.Logging;

using NCI.Web;
using NCI.Web.CDE.Application;
using NCI.Web.CDE.Modules;
using NCI.Web.CDE.UI;

using CancerGov.ClinicalTrials.Basic.v2.Configuration;
using CancerGov.ClinicalTrialsAPI;
using System.Threading;

namespace CancerGov.ClinicalTrials.Basic.v2.SnippetControls
{
    public class APICTSDetailsControl : BaseMgrAPICTSControl
    {
        static ILog log = LogManager.GetLogger(typeof(BasicCTSViewControl));

        /// <summary>
        /// Gets the path to the template
        /// </summary>
        /// <returns></returns>
        protected override string GetTemplatePath()
        {
            return this.Config.DetailedViewPageTemplatePath;
        }

        /// <summary>
        /// Goes and fetches the data from the API & Returns the results to base class to be bound to the template.
        /// </summary>
        /// <returns></returns>
        protected override object GetDataForTemplate()
        {
            string nctid = ValidateID();

            // Get Trial by ID
            ClinicalTrial trial;
            try
            {
                // Retrieve a Clinical Trial based on the Trial ID
                trial = this.CTSManager.Get(nctid);
            }
            catch (Exception ex)
            {
                // If we hit some other error when getting the trials, redirect to the error page 
                string errMessage = "CDE:APICTSDetailsControl.cs:OnLoad" + " Requested trial ID: " + nctid + "\nException thrown by CTSManager.get(nctid) call.";
                log.Error(errMessage, ex);
                ErrorPageDisplayer.RaisePageError(errMessage);

                //TODO: Make sure RaisePageError stops processing this page
                return null;                
            }

            RedirectIfTrialNotValid(trial);

            //TODO: Add Field Filters

            //TODO: Setup analytics


            return new
            {
                Trial = trial,
                Parameters = this.ParsedReqUrlParams.QueryParameters.ContainsKey("rl") ? SearchParams : null,
                Control = this,
                TrialTools = new TrialVelocityTools()
            };
        }

        /// <summary>
        /// Validated the ID parameter for A) existance and B) format.
        /// </summary>
        /// <exception cref="HttpException">Thrown when an id is missing or malformed.</exception>
        /// <returns>A string that represents the trial ID.</returns>
        private string ValidateID()
        {
            if (!IsInUrl(this.ParsedReqUrlParams, "id"))
            {
                throw new HttpException(400, "Missing trial ID.");
            }

            string nctid = this.ParsedReqUrlParams.QueryParameters["id"].Trim();

            if (!Regex.IsMatch(nctid, "^NCT[0-9]+$") && !Regex.IsMatch(nctid, "^NCI-"))
            {
                throw new HttpException(400, "Invalid trial ID.");
            }

            return nctid;
        }

        /// <summary>
        /// Send 404 error if the trial is invalid or does not have a view-able status.
        /// </summary>
        /// <param name="trial">The trial to check</param>
        private void RedirectIfTrialNotValid(ClinicalTrial trial)
        {
            // If trial value is null, redirect to the 404 page
            //TODO: Determine if a null trial should be a 404, or redirect to CT.gov
            if (trial == null)
            {
                throw new HttpException(404, "Trial cannot be found.");
            }

            if (Config.RedirectIfInactive)
            {
                string[] actives = BasicCTSManager.ActiveTrialStatuses;
                if (Array.IndexOf(actives, trial.CurrentTrialStatus) < 0)
                {
                    throw new HttpException(404, "Trial status is not active.");
                }
                if (string.IsNullOrWhiteSpace(trial.NCTID))
                {
                    throw new HttpException(404, "Trial does not have an NCT ID.");
                }
            }
        }

        #region Velocity Helpers


        public string GetShowNearbyUrl()
        {
            /*
            url.SetUrl(PageInstruction.GetUrl("CurrentUrl").ToString());
            if (url.QueryParameters.ContainsKey(LOCATION_ALL))
            {
                url.QueryParameters[LOCATION_ALL] = "0";
            }
            else
            {
                url.QueryParameters.Add(LOCATION_ALL, "0");
            }
             */
            return "/shownear";
        }

        public string GetShowAllUrl()
        {
            /*
            url.SetUrl(PageInstruction.GetUrl("CurrentUrl").ToString());
            if (url.QueryParameters.ContainsKey(LOCATION_ALL))
            {
                url.QueryParameters[LOCATION_ALL] = "1";
            }
            else
            {
                url.QueryParameters.Add(LOCATION_ALL, "1");
            }
             */
            return "/AllUrl";
        }

        public string GetBackToResultsUrl()
        {
            /*
            url.SetUrl(PageInstruction.GetUrl("CurrentUrl").ToString());
            url.UriStem = _basicCTSPageInfo.ResultsPagePrettyUrl;

            if (url.QueryParameters.ContainsKey(LOCATION_ALL))
            {
                url.QueryParameters.Remove(LOCATION_ALL);
            }

            if (url.QueryParameters.ContainsKey(NCT_ID))
            {
                url.QueryParameters.Remove(NCT_ID);
            }
             */
            return "/backtores";
        }

        #endregion

    }
}
