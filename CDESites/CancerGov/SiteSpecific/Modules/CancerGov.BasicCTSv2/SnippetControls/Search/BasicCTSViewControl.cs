﻿using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.UI;
using CancerGov.ClinicalTrialsAPI;
using Common.Logging;
using NCI.Web.CDE;
using NCI.Web.CDE.Application;
using NCI.Web.CDE.Modules;
using NCI.Web.CDE.WebAnalytics;

namespace CancerGov.ClinicalTrials.Basic.v2.SnippetControls
{
    public partial class BasicCTSViewControl : BasicCTSBaseControl
    {
        static ILog log = LogManager.GetLogger(typeof(BasicCTSViewControl));

        /// <summary>
        /// Retrieve the working URL of this control from the page XML.
        /// </summary>
        public BaseCTSSearchParam SearchParams { get; private set; }
        
        private const string _phaseI = "Phase I";
        private const string _phaseII = "Phase II";
        private const string _phaseIII = "Phase III";
        private const string _phaseIV = "Phase IV";

        private const string _phaseI_II = "Phase I/II";
        private const string _phaseII_III = "Phase II/III";

        /// <summary>
        /// Get value of inactive trial redirection flag
        /// </summary>
        private bool IsRedirectable
        {
            get { return BasicCTSPageInfo.RedirectIfInactive; }
        }

        /// <summary>
        /// Get the working URL of this control for additional modifications
        /// </summary>
        protected override String WorkingUrl
        {
            get { return BasicCTSPageInfo.DetailedViewPagePrettyUrl; }
        }

        
        /// <summary>
        /// Returns the cancer type the user searched for if the current search contains a type/condition.
        /// </summary>
        /// <returns></returns>
        public string GetCancerType()
        {
            if (SearchParams is CancerTypeSearchParam)
            {
                var CancerTypeSearchParams = (CancerTypeSearchParam)SearchParams;

                if (!string.IsNullOrWhiteSpace(CancerTypeSearchParams.CancerTypeDisplayName))
                    return CancerTypeSearchParams.CancerTypeDisplayName;
            }
            return null;
        }

        /// <summary>
        /// Returns the phrase the user searched for if the current search contains a phrase.
        /// </summary>
        /// <returns></returns>
        public string GetPhrase()
        {
            if (SearchParams is PhraseSearchParam)
            {
                var PhraseSearchParams = (PhraseSearchParam)SearchParams;
                if (!string.IsNullOrWhiteSpace(PhraseSearchParams.Phrase))
                    return PhraseSearchParams.Phrase;
            }
            return null;
        }

        /// <summary>
        /// Determines if the current search has a Zip or not.
        /// </summary>
        /// <returns></returns>
        public bool HasZip()
        {
            return SearchParams.ZipLookup != null;
        }

        /// <summary>
        /// Check for the presence of the RESULTS_LINK_FLAG ("rl=" param in the URL). 
        /// If it is present, then this page load is the result of a CTS Results Page link.
        /// </summary>
        /// <returns>bool - true if "rl" query param exists</returns>
        public bool IsSearchResult()
        {
            bool rtn = (Request.QueryString[RESULTS_LINK_FLAG] == null) ? false : true;
            return rtn;
        }

        /// <summary>
        /// Returns whether a user searched for all trials.
        /// </summary>
        /// <returns></returns>
        public bool GetSearchForAllTrials()
        {
            if ((this.hasInvalidSearchParam == false) && (_setFields == QueryFieldsSetByUser.None))
                return true;
            else
                return false;
        }

        public int GetShowAll()
        {
            return ParamAsInt(LOCATION_ALL, -1);
        }

        protected string GetGlossifiedTrialPhase(string[] phases)
        {
            int phaseBits = 0x00;
            List<string> glossPhases = new List<string>();

            foreach (string phase in phases)
            {
                switch (phase)
                {
                    case _phaseI:
                        phaseBits |= 0x01;
                        break;
                    case _phaseII:
                        phaseBits |= 0x02;
                        break;
                    case _phaseIII:
                        phaseBits |= 0x04;
                        break;
                    case _phaseIV:
                        phaseBits |= 0x08;
                        break;
                    default:
                        glossPhases.Add(phase);
                        break;
                }
            }


            SortedDictionary<int, string> termIds = new SortedDictionary<int, string>();

            switch (phaseBits)
            {
                case 0x00: // no phases recognized, just use glossPhases
                    break;
                case 0x01: //"phase I":
                    termIds.Add(45830, _phaseI);
                    break;
                case 0x02: //"phase II":
                    termIds.Add(45831, _phaseII);
                    break;
                case 0x03: //"phase I/II":
                    termIds.Add(45832, _phaseI_II);
                    break;
                case 0x04: //"phase III":
                    termIds.Add(45833, _phaseIII);
                    break;
                case 0x06: //"phase II/III":
                    termIds.Add(45834, _phaseII_III);
                    break;
                case 0x08: //"phase IV":
                    termIds.Add(45835, _phaseIV);
                    break;
                default: // unknown, combine all phases
                    glossPhases.Add("unknown phase pairing: " + string.Join(", ", phases) + " (bits =" + phaseBits + ")");
                    return string.Join(", ", glossPhases);
            }

            foreach (KeyValuePair<int, string> pair in termIds)
            {
                glossPhases.Add("<a onclick=\"javascript:popWindow('defbyid','CDR00000" + pair.Key.ToString() + "&amp;version=Patient&amp;language=English'); return false;\" " +
                "href=\"/Common/PopUps/popDefinition.aspx?id=CDR00000" + pair.Key.ToString() + "&amp;version=Patient&amp;language=English\" " +
                "class=\"definition\">" + pair.Value + "</a>");
            }

            return string.Join(", ", glossPhases);
        }

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);

            SearchParams = GetSearchParams();
            HandleLegacyCancerTypeID(); // Redirect for URLs containing "t=CDRXXXX"
        }

        protected override void OnLoad(EventArgs e)
        {

            base.OnLoad(e);

            // Get ID
            string nctid = Request.Params[NCT_ID];
            if (String.IsNullOrWhiteSpace(nctid))
            {
                throw new HttpException(404, "Missing trial ID.");
            }

            nctid = nctid.Trim();

            if (!Regex.IsMatch(nctid, "^NCT[0-9]+$") && !Regex.IsMatch(nctid, "^NCI-"))
            {
                throw new HttpException(404, "Invalid trial ID.");
            }

            // Get Trial by ID
            ClinicalTrial trial;
            try
            {
                // Retrieve a Clinical Trial based on the Trial ID
                trial = _basicCTSManager.Get(nctid);
            }
            catch (ArgumentNullException ex)
            {
                // If we hit a null exception when getting a trial from the API, redirect to the "ID not Found" page
                string errMessage = "CDE:BasicCTSViewControl.cs:OnLoad" + " Requested trial ID: " + nctid + "\nArgumentNullException thrown by _basicCTSManager.get() call.";
                log.Debug(errMessage, ex);
                ErrorPageDisplayer.RaisePageNotFound(errMessage);
                return;
            }
            catch (Exception ex)
            {
                // If we hit some other error when getting the trials, redirect to the error page 
                string errMessage = "CDE:BasicCTSViewControl.cs:OnLoad" + " Requested trial ID: " + nctid + "\nException thrown by _basicCTSManager.get(nctid) call.";
                log.Error(errMessage, ex);
                ErrorPageDisplayer.RaisePageError(errMessage);
                return;
            }

            // If trial value is null, redirect to the 404 page
            if (trial == null)
            {
                throw new HttpException(404, "Trial cannot be found.");
            }

            // If the IsRedirectable flag is set to true, check trial status. If inactive or has no NCT ID, go to 404 page.
            if (IsRedirectable)
            {
                string[] actives = BasicCTSManager.ActiveTrialStatuses;
                if(Array.IndexOf(actives, trial.CurrentTrialStatus) < 0)
                { 
                    throw new HttpException(404, "Trial status is not active.");
                }
                if (string.IsNullOrWhiteSpace(trial.NCTID))
                {
                    throw new HttpException(404, "Trial does not have an NCT ID.");
                }
            }

            // get zip from search parameters
            string zip = "";
            if (SearchParams.ZipLookup != null)
            {
                zip = SearchParams.ZipLookup.PostalCode_ZIP;
                int zipProximity = SearchParams.ZipRadius; //In miles
            }

            // Show Trial

            // Copying the Title & Short Title logic from Advanced Form
            //set the page title as the protocol title
            PageInstruction.AddFieldFilter("long_title", (fieldName, data) =>
            {
                data.Value = trial.BriefTitle;
            });

            PageInstruction.AddFieldFilter("short_title", (fieldName, data) =>
            {
                //Eh, When would this happen???
                if (!string.IsNullOrWhiteSpace(trial.NCTID))
                    data.Value = "View Clinical Trial " + trial.NCTID;
                else
                    data.Value = "View Clinical Trial";

            });

            PageInstruction.AddUrlFilter("CurrentUrl", (name, url) =>
            {
                //NOTE: If you add more params, please remove them from CanonicalURL,
                //unless they substantially change the rendered HTML markup.  (e.g. like id does)
                url.QueryParameters.Add(NCT_ID, nctid);
                if (GetShowAll() > -1)
                {
                    url.QueryParameters.Add(LOCATION_ALL, GetShowAll().ToString());
                }

                if ((_setFields & QueryFieldsSetByUser.Age) != 0)
                    url.QueryParameters.Add(AGE_PARAM, SearchParams.Age.ToString());

                if ((_setFields & QueryFieldsSetByUser.Gender) != 0)
                {
                    if (SearchParams.Gender == BaseCTSSearchParam.GENDER_FEMALE)
                        url.QueryParameters.Add(GENDER_PARAM, "1");
                    else if (SearchParams.Gender == BaseCTSSearchParam.GENDER_MALE)
                        url.QueryParameters.Add(GENDER_PARAM, "2");
                }

                if ((_setFields & QueryFieldsSetByUser.ZipCode) != 0)
                    url.QueryParameters.Add(ZIP_PARAM, SearchParams.ZipLookup.PostalCode_ZIP);

                if ((_setFields & QueryFieldsSetByUser.ZipProximity) != 0)
                    url.QueryParameters.Add(ZIPPROX_PARAM, SearchParams.ZipRadius.ToString());

                //Phrase and type are based on the type of object
                if ((_setFields & QueryFieldsSetByUser.CancerType) != 0 && SearchParams is CancerTypeSearchParam)
                {
                    url.QueryParameters.Add(CANCERTYPE_PARAM, cancerTypeIDAndHash);
                }
                if ((_setFields & QueryFieldsSetByUser.Phrase) != 0 && SearchParams is PhraseSearchParam)
                {
                    if (((PhraseSearchParam)SearchParams).IsBrokenCTSearchParam)
                        url.QueryParameters.Add(CANCERTYPEASPHRASE_PARAM, HttpUtility.UrlEncode(((PhraseSearchParam)SearchParams).Phrase));
                    else
                        url.QueryParameters.Add(PHRASE_PARAM, HttpUtility.UrlEncode(((PhraseSearchParam)SearchParams).Phrase));
                }

                // Add subtypes, stage, and findings to query params
                if (_setFields.HasFlag(QueryFieldsSetByUser.CancerSubtype))
                    url.QueryParameters.Add(CANCERTYPE_SUBTYPE, subtypeCCode);
                if (_setFields.HasFlag(QueryFieldsSetByUser.CancerStage))
                    url.QueryParameters.Add(CANCERTYPE_STAGE, stageCCode);
                if (_setFields.HasFlag(QueryFieldsSetByUser.CancerFindings))
                    url.QueryParameters.Add(CANCERTYPE_FINDINGS, findingsCCode);

                // Page Number
                url.QueryParameters.Add(PAGENUM_PARAM, SearchParams.Page.ToString());

                //Items Per Page
                url.QueryParameters.Add(ITEMSPP_PARAM, SearchParams.ItemsPerPage.ToString());

                // Add the "rl" flag, indicating that this is a link coming from the CTS Results Page
                url.QueryParameters.Add(RESULTS_LINK_FLAG, SearchParams.ResultsLinkFlag.ToString());
            });

            PageInstruction.AddUrlFilter("ShowNearbyUrl", (name, url) =>
            {
                url.SetUrl(PageInstruction.GetUrl("CurrentUrl").ToString());
                if (url.QueryParameters.ContainsKey(LOCATION_ALL))
                {
                    url.QueryParameters[LOCATION_ALL] = "0";
                }
                else
                {
                    url.QueryParameters.Add(LOCATION_ALL, "0");
                }
            });

            PageInstruction.AddUrlFilter("ShowAllUrl", (name, url) =>
            {
                url.SetUrl(PageInstruction.GetUrl("CurrentUrl").ToString());
                if (url.QueryParameters.ContainsKey(LOCATION_ALL))
                {
                    url.QueryParameters[LOCATION_ALL] = "1";
                }
                else
                {
                    url.QueryParameters.Add(LOCATION_ALL, "1");
                }
            });

            PageInstruction.AddUrlFilter("ResultsUrl", (name, url) =>
            {
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
            });

            PageInstruction.AddUrlFilter("CanonicalUrl", (name, url) =>
            {
                // only the id should be provided for the canonical URL, so clear all query parameters and
                // then add back id
                url.QueryParameters.Clear();
                url.QueryParameters.Add(NCT_ID, nctid);
            });

            // Override the social media URL (og:url)
            PageInstruction.AddFieldFilter("og:url", (fieldName, data) =>
            {
                //Ok, this is weird, but...  The OpenGraph URL is actually a field. It kind of makes sense,
                //and it kind of does not.  Really it should be a field that gets the og:url instead of the 
                //pretty URL.
                //BUt here we are, and it is what we have.  So let's replace the og:url with the canonical URL.

                data.Value = PageInstruction.GetUrl(PageAssemblyInstructionUrls.CanonicalUrl).ToString();
            });


            //TODO: Fix Glossified Phase
            LiteralControl ltl = new LiteralControl(VelocityTemplate.MergeTemplateWithResultsByFilepath(
                    BasicCTSPageInfo.DetailedViewPageTemplatePath, 
                    new
                    {
                        Trial = trial,
                        Control = this,
                        TrialTools = new TrialVelocityTools()
                    }
                )
            );
            Controls.Add(ltl);

            // Set analytics page load values
            SetAnalytics();
        }

        #region Analytics methods
        /// <summary>
        /// Set default pageLoad analytics for this page
        /// </summary>
        protected void SetAnalytics()
        {
            string desc = GetParamsForAnalytics();

            // Set props and evars
            this.PageInstruction.SetWebAnalytics(WebAnalyticsOptions.Props.prop62, wbField =>
            {
                wbField.Value = desc;
            });
            this.PageInstruction.SetWebAnalytics(WebAnalyticsOptions.eVars.evar62, wbField =>
            {
                wbField.Value = desc;
            });
        }

        /// <summary>
        /// Get query params from URL and format for use in analytics.
        /// </summary>
        /// <returns>Formatted string</returns>
        protected String GetParamsForAnalytics()
        {
            string result;
            HttpRequest request = HttpContext.Current.Request;

            // Set result value based on referrer
            if (request.QueryString[RESULTS_LINK_FLAG] == null)
            {
                result = "Clinical Trials: Custom";
            }
            else
            {
                result = "Clinical Trials: Basic";
            }

            return result;
        }
        #endregion

        #region Velocity helpers
        public string GetSearchFormUrl()
        {
            if (SearchParams.ResultsLinkFlag == 2)
            {
                return BasicCTSPageInfo.AdvSearchPagePrettyUrl;
            }
            else
            {
                return BasicCTSPageInfo.BasicSearchPagePrettyUrl;
            }
        }
        #endregion
    }
}