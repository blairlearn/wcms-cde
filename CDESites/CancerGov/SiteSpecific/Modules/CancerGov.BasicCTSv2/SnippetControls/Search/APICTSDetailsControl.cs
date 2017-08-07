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

        private bool _showingAll = false;
        private string _trialID = string.Empty;

        public bool ShowingAll { get { return _showingAll; } }
        public string TrialID { get { return _trialID;  } }

        protected override void Init()
        {
            //MAKE SURE THE BASE IS CALLED!!!
            base.Init();

            if (IsInUrl(ParsedReqUrlParams, "all"))
            {
                this._showingAll = ParamAsBool(ParsedReqUrlParams.QueryParameters["all"], false);
            }

        }

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
            ParseAndValidateID();

            // Get Trial by ID
            ClinicalTrial trial;
            try
            {
                // Retrieve a Clinical Trial based on the Trial ID
                trial = this.CTSManager.Get(TrialID);
            }
            catch (Exception ex)
            {
                // If we hit some other error when getting the trials, redirect to the error page 
                string errMessage = "CDE:APICTSDetailsControl.cs:OnLoad" + " Requested trial ID: " + TrialID + "\nException thrown by CTSManager.get(nctid) call.";
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
        private void ParseAndValidateID()
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

            this._trialID = nctid;
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
            //Create a new url for the current details page.
            NciUrl url = new NciUrl();
            url.SetUrl(this.Config.DetailedViewPagePrettyUrl);

            //Convert the current search parameters into a NciUrl
            NciUrl paramsUrl = CTSSearchParamFactory.ConvertParamsToUrl(this.SearchParams);

            //Copy the params 
            url.QueryParameters = paramsUrl.QueryParameters;

            url.QueryParameters.Add("id", TrialID);
            url.QueryParameters.Add("pn", this.PageNum.ToString());
            url.QueryParameters.Add("ni", this.ItemsPerPage.ToString());
            url.QueryParameters.Add("all", "0");

            return url.ToString();
        }

        public string GetShowAllUrl()
        {
            //Create a new url for the current details page.
            NciUrl url = new NciUrl();
            url.SetUrl(this.Config.DetailedViewPagePrettyUrl);

            //Convert the current search parameters into a NciUrl
            NciUrl paramsUrl = CTSSearchParamFactory.ConvertParamsToUrl(this.SearchParams);

            //Copy the params 
            url.QueryParameters = paramsUrl.QueryParameters;

            url.QueryParameters.Add("id", TrialID);
            url.QueryParameters.Add("pn", this.PageNum.ToString());
            url.QueryParameters.Add("ni", this.ItemsPerPage.ToString());
            url.QueryParameters.Add("all", "1");

            return url.ToString();
        }

        public string GetBackToResultsUrl()
        {
            //Create a new url for the current details page.
            NciUrl url = new NciUrl();
            url.SetUrl(this.Config.ResultsPagePrettyUrl);

            //Convert the current search parameters into a NciUrl
            NciUrl paramsUrl = CTSSearchParamFactory.ConvertParamsToUrl(this.SearchParams);

            //Copy the params 
            url.QueryParameters = paramsUrl.QueryParameters;

            url.QueryParameters.Add("pn", this.PageNum.ToString());
            url.QueryParameters.Add("ni", this.ItemsPerPage.ToString());

            return url.ToString();
        }

        #endregion

    }
}
