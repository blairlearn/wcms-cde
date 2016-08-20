using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.UI;
using CancerGov.ClinicalTrialsAPI;
using NCI.Logging;
using NCI.Web;
using NCI.Web.CDE;
using NCI.Web.CDE.Modules;
using NCI.Web.CDE.UI;



namespace CancerGov.ClinicalTrials.Basic.v2.SnippetControls
{
    public partial class BasicCTSViewControl : BasicCTSBaseControl
    {
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
        /// Returns whether a user searched for all trials.
        /// </summary>
        /// <returns></returns>
        public bool GetSearchForAllTrials()
        {
            if ((this.hasInvalidSearchParam == false) && (_setFields == SetFields.None))
                return true;
            else
                return false;
        }

        public int GetShowAll()
        {
            return ParmAsInt("all", -1);
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
                    glossPhases.Add("unknown phase pairing: " + string.Join(", ", phases) + " (bits ="  + phaseBits + ")");
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
            string nctid = Request.Params["id"];
            if (String.IsNullOrWhiteSpace(nctid))
            {
                throw new HttpException(404, "Missing trial ID.");
            }

            nctid = nctid.Trim();

            if (!Regex.IsMatch(nctid, "^NCT[0-9]+$"))
            {
                throw new HttpException(404, "Invalid trial ID.");
            }

            // Get Trial by ID
            ClinicalTrial trial;
            try
            {
                trial = _basicCTSManager.Get(nctid);
            }
            catch (Exception ex)
            {
                string errMessage = "CDE:BasicCTSViewControl.cs:OnLoad" + " Requested NCTid: " + nctid + "\nException thrown by _basicCTSManager.get(nctid) call.";
                Logger.LogError(errMessage, NCIErrorLevel.Error, ex);
                ErrorPageDisplayer.RaisePageError(errMessage);
                return;
            }

            if (trial == null)
                throw new HttpException(404, "Trial cannot be found.");

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
                int i = 1;
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
                url.QueryParameters.Add("id", nctid);
                if (GetShowAll() > -1)
                {
                    url.QueryParameters.Add("all", GetShowAll().ToString());
                }

                if ((_setFields & SetFields.Age) != 0)
                    url.QueryParameters.Add(AGE_PARAM, SearchParams.Age.ToString());

                if ((_setFields & SetFields.Gender) != 0)
                {
                    if (SearchParams.Gender == BaseCTSSearchParam.GENDER_FEMALE)
                        url.QueryParameters.Add(GENDER_PARAM, "1");
                    else if (SearchParams.Gender == BaseCTSSearchParam.GENDER_MALE)
                        url.QueryParameters.Add(GENDER_PARAM, "2");
                }

                if ((_setFields & SetFields.ZipCode) != 0)
                    url.QueryParameters.Add(ZIP_PARAM, SearchParams.ZipLookup.PostalCode_ZIP);

                if ((_setFields & SetFields.ZipProximity) != 0)
                    url.QueryParameters.Add(ZIPPROX_PARAM, SearchParams.ZipRadius.ToString());

                //Phrase and type are based on the type of object
                if ((_setFields & SetFields.CancerType) != 0 && SearchParams is CancerTypeSearchParam)
                {
                    url.QueryParameters.Add(CANCERTYPE_PARAM, cancerTypeIDAndHash);
                }

                if ((_setFields & SetFields.Phrase) != 0 && SearchParams is PhraseSearchParam)
                {
                    if (((PhraseSearchParam)SearchParams).IsBrokenCTSearchParam)
                        url.QueryParameters.Add(CANCERTYPEASPHRASE_PARAM, HttpUtility.UrlEncode(((PhraseSearchParam)SearchParams).Phrase));
                    else
                        url.QueryParameters.Add(PRASE_PARAM, HttpUtility.UrlEncode(((PhraseSearchParam)SearchParams).Phrase));
                }

                // Page Number
                url.QueryParameters.Add(PAGENUM_PARAM, SearchParams.Page.ToString());

                //Items Per Page
                url.QueryParameters.Add(ITEMSPP_PARAM, SearchParams.ItemsPerPage.ToString());
            });

            PageInstruction.AddUrlFilter("ShowNearbyUrl", (name, url) =>
            {
                url.SetUrl(PageInstruction.GetUrl("CurrentUrl").ToString());
                if (url.QueryParameters.ContainsKey("all"))
                {
                    url.QueryParameters["all"] = "0";
                }
                else
                {
                    url.QueryParameters.Add("all", "0");
                }
            });

            PageInstruction.AddUrlFilter("ShowAllUrl", (name, url) =>
            {
                url.SetUrl(PageInstruction.GetUrl("CurrentUrl").ToString());
                if (url.QueryParameters.ContainsKey("all"))
                {
                    url.QueryParameters["all"] = "1";
                }
                else
                {
                    url.QueryParameters.Add("all", "1");
                }
            });

            PageInstruction.AddUrlFilter("ResultsUrl", (name, url) =>
            {
                url.SetUrl(PageInstruction.GetUrl("CurrentUrl").ToString());
                url.UriStem = _basicCTSPageInfo.ResultsPagePrettyUrl;

                if (url.QueryParameters.ContainsKey("all"))
                {
                    url.QueryParameters.Remove("all");
                }

                if (url.QueryParameters.ContainsKey("id"))
                {
                    url.QueryParameters.Remove("id");
                }
            });

            PageInstruction.AddUrlFilter("CanonicalUrl", (name, url) =>
            {
                // only the id should be provided for the canonical URL, so clear all query parameters and
                // then add back id
                url.QueryParameters.Clear();
                url.QueryParameters.Add("id", nctid);
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
                        //GlossifiedPhase = GetGlossifiedTrialPhase(trial.ProtocolPhases),
                        TrialTools = new TrialVelocityTools()
                    }
                )
            );
            Controls.Add(ltl);
        }
    }
}
