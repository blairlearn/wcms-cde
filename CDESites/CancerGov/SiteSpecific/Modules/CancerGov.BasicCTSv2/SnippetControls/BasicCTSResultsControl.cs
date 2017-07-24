using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using CancerGov.ClinicalTrialsAPI;

using NCI.Web;
using NCI.Web.CDE;
using NCI.Web.CDE.Modules;
using NCI.Web.CDE.UI;
using NCI.Web.CDE.WebAnalytics;

namespace CancerGov.ClinicalTrials.Basic.v2.SnippetControls
{
    public class BasicCTSResultsControl : BasicCTSBaseControl
    {
        /// <summary>
        /// Gets the Search Parameters for the current request.
        /// </summary>
        public BaseCTSSearchParam SearchParams { get; private set; }

        /// <summary>
        /// Retrieve the working URL of this control from the page XML.
        /// </summary>
        protected override String WorkingUrl
        {
            get { return BasicCTSPageInfo.ResultsPagePrettyUrl; }
        }

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);

            // Only present for BasicCTSResultsControl.
            // This call is disabled/removed in base.OnInit().
            HandleOldCancerTypeID();

            SearchParams = GetSearchParams();
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            //Do the search
            var results = _basicCTSManager.Search(SearchParams);

            // Copying the Title & Short Title logic from Advanced Form
            //set the page title as the protocol title
            PageInstruction.AddFieldFilter("long_title", (fieldName, data) =>
            {
                int maxPage = (int)Math.Ceiling((double)results.TotalResults / (double)SearchParams.ItemsPerPage);
                data.Value = "Results of Your Search";

                if (results.TotalResults == 0)
                {
                    data.Value = "No Trials Matched Your Search";
                }
                else if (hasInvalidSearchParam)
                {
                    data.Value = "No Results";
                }
                else if (SearchParams.Page > maxPage)
                {
                    data.Value = "No Results";
                }
            });

            PageInstruction.AddUrlFilter("CurrentUrl", (name, url) =>
            {
                if (_setFields.HasFlag(QueryFieldsSetByUser.Age))
                    url.QueryParameters.Add(AGE_PARAM, SearchParams.Age.ToString());

                if (_setFields.HasFlag(QueryFieldsSetByUser.Gender))
                {
                    if (SearchParams.Gender == BaseCTSSearchParam.GENDER_FEMALE)
                        url.QueryParameters.Add(GENDER_PARAM, "1");
                    else if (SearchParams.Gender == BaseCTSSearchParam.GENDER_MALE)
                        url.QueryParameters.Add(GENDER_PARAM, "2");
                }

                if (_setFields.HasFlag(QueryFieldsSetByUser.ZipCode))
                    url.QueryParameters.Add(ZIP_PARAM, SearchParams.ZipLookup.PostalCode_ZIP);

                if (_setFields.HasFlag(QueryFieldsSetByUser.ZipProximity))
                    url.QueryParameters.Add(ZIPPROX_PARAM, SearchParams.ZipRadius.ToString());

                //Phrase and type are based on the type of object
                if (SearchParams is CancerTypeSearchParam)
                {
                    if (_setFields.HasFlag(QueryFieldsSetByUser.CancerType))
                        url.QueryParameters.Add(CANCERTYPE_PARAM, cancerTypeIDAndHash);
                }
                if (SearchParams is PhraseSearchParam)
                {
                    if (_setFields.HasFlag(QueryFieldsSetByUser.Phrase))
                    {
                        if (((PhraseSearchParam)SearchParams).IsBrokenCTSearchParam)
                            url.QueryParameters.Add(CANCERTYPEASPHRASE_PARAM, HttpUtility.UrlEncode(((PhraseSearchParam)SearchParams).Phrase));
                        else
                            url.QueryParameters.Add(PHRASE_PARAM, HttpUtility.UrlEncode(((PhraseSearchParam)SearchParams).Phrase));
                    }
                }

                // Add subtypes, stage, and findings to query params
                if (_setFields.HasFlag(QueryFieldsSetByUser.CancerSubtype))
                    url.QueryParameters.Add(CANCERTYPE_SUBTYPE, subtypeCCode);
                if (_setFields.HasFlag(QueryFieldsSetByUser.CancerStage))
                    url.QueryParameters.Add(CANCERTYPE_STAGE, stageCCode);
                if (_setFields.HasFlag(QueryFieldsSetByUser.CancerFindings))
                    url.QueryParameters.Add(CANCERTYPE_FINDINGS, findingsCCode);

                if (_setFields.HasFlag(QueryFieldsSetByUser.Country))
                    url.QueryParameters.Add(LOCATION_COUNTRY, SearchParams.Country);
                
                if (_setFields.HasFlag(QueryFieldsSetByUser.City))
                    url.QueryParameters.Add(LOCATION_CITY, SearchParams.City);
                
                if (_setFields.HasFlag(QueryFieldsSetByUser.State))
                    url.QueryParameters.Add(LOCATION_STATE, SearchParams.State);
                
                if (_setFields.HasFlag(QueryFieldsSetByUser.Hospital))
                    url.QueryParameters.Add(HOSPITAL_INSTITUTION, SearchParams.HospitalOrInstitution);
                
                if (_setFields.HasFlag(QueryFieldsSetByUser.AtNIH))
                    url.QueryParameters.Add(AT_NIH, SearchParams.AtNIH.ToString());
                
                if (_setFields.HasFlag(QueryFieldsSetByUser.TrialType))
                    url.QueryParameters.Add(TRIAL_TYPE, SearchParams.TrialType);
                
                if (_setFields.HasFlag(QueryFieldsSetByUser.DrugCode))
                    url.QueryParameters.Add(DRUG_CODE, String.Join(",", SearchParams.DrugIDs));
                
                if (_setFields.HasFlag(QueryFieldsSetByUser.DrugName))
                    url.QueryParameters.Add(DRUG_NAME, SearchParams.DrugName);
                
                if (_setFields.HasFlag(QueryFieldsSetByUser.TreatmentCode))
                    url.QueryParameters.Add(TREATMENT_CODE, String.Join(",", SearchParams.TreatmentInterventionCodes));
                
                if (_setFields.HasFlag(QueryFieldsSetByUser.TreatmentName))
                    url.QueryParameters.Add(TREATMENT_NAME, SearchParams.TreatmentInterventionTerm);
                
                if (_setFields.HasFlag(QueryFieldsSetByUser.TrialPhase))
                    url.QueryParameters.Add(TRIAL_PHASE, SearchParams.TrialPhase);
                
                if (_setFields.HasFlag(QueryFieldsSetByUser.NewTrialsOnly))
                {
                    var test = true.ToString();
                    url.QueryParameters.Add(NEW_TRIALS_ONLY, SearchParams.NewTrialsOnly.ToString());
                }
                // This needs to be converted into the csv format.
                if (_setFields.HasFlag(QueryFieldsSetByUser.TrialIDs))
                    url.QueryParameters.Add(TRIAL_IDS, String.Join(",", SearchParams.TrialIDs));
                
                if (_setFields.HasFlag(QueryFieldsSetByUser.TrialInvestigator))
                    url.QueryParameters.Add(TRIAL_INVESTIGATOR, SearchParams.PrincipalInvestigator);
                
                if (_setFields.HasFlag(QueryFieldsSetByUser.LeadOrganization))
                    url.QueryParameters.Add(LEAD_ORGANIZATION, SearchParams.LeadOrganization);


                //Items Per Page
                url.QueryParameters.Add(ITEMSPP_PARAM, SearchParams.ItemsPerPage.ToString());

            });


            // Show Results
            LiteralControl ltl = new LiteralControl(VelocityTemplate.MergeTemplateWithResultsByFilepath(
                BasicCTSPageInfo.ResultsPageTemplatePath,
                new
                {
                    Results = results,
                    Control = this,
                    TrialTools = new TrialVelocityTools()
                }
            ));
            Controls.Add(ltl);

            // Set analytics page load values
            SetAnalytics(results);

        }

        /// <summary>
        /// Looks for bookmarked searches where CANCERTYPE_PARAM contains a CDRID instead of a concept ID.
        /// When such searches are located, redirect to the main search page, with a flag notifying that
        /// this is a redirection.
        /// </summary>
        protected void HandleOldCancerTypeID()
        {
            string cancerTypeID = this.ParamAsStr(CANCERTYPE_PARAM);

            // Detect legacy CDRID.
            if (!String.IsNullOrWhiteSpace(cancerTypeID) &&
                cancerTypeID.ToLower().StartsWith("cdr"))
            {
                // Redirect to search page
                NciUrl redirectURL = new NciUrl();
                redirectURL.SetUrl(BasicCTSPageInfo.SearchPagePrettyUrl);

                // Copy querystring parameters from the request.
                foreach (string key in Request.QueryString.AllKeys)
                    redirectURL.QueryParameters.Add(key, Request.QueryString[key]);

                // Add redirection flag.
                redirectURL.QueryParameters.Add(REDIRECTED_FLAG, String.Empty);

                DoPermanentRedirect(Response, redirectURL.ToString());
            }
        }


        #region Analytics methods
        /// <summary>
        /// Set default pageLoad analytics for this page
        /// </summary>
        protected void SetAnalytics(ClinicalTrialsCollection results)
        {
            string val = "clinicaltrials_basic";
            string desc = "Clinical Trials: Basic";
            string count = results.TotalResults.ToString();
            string waParm = GetParamsForAnalytics();

            // Set event
            this.PageInstruction.SetWebAnalytics(WebAnalyticsOptions.Events.event2, wbField =>
            {
                wbField.Value = WebAnalyticsOptions.Events.event2.ToString();
            });

            // Set props
            this.PageInstruction.SetWebAnalytics(WebAnalyticsOptions.Props.prop10, wbField =>
            {
                wbField.Value = count;
            });
            this.PageInstruction.SetWebAnalytics(WebAnalyticsOptions.Props.prop11, wbField =>
            {
                wbField.Value = val;
            });
            this.PageInstruction.SetWebAnalytics(WebAnalyticsOptions.Props.prop22, wbField =>
            {
                wbField.Value = waParm;
            });
            this.PageInstruction.SetWebAnalytics(WebAnalyticsOptions.Props.prop62, wbField =>
            {
                wbField.Value = desc;
            });

            // Set eVars
            this.PageInstruction.SetWebAnalytics(WebAnalyticsOptions.eVars.evar11, wbField =>
            {
                wbField.Value = val;
            });
            this.PageInstruction.SetWebAnalytics(WebAnalyticsOptions.eVars.evar22, wbField =>
            {
                wbField.Value = waParm;
            });
            this.PageInstruction.SetWebAnalytics(WebAnalyticsOptions.eVars.evar62, wbField =>
            {
                wbField.Value = desc;
            });

        }

        /// <summary>
        /// Get search query params from URL and format into an analytics-friendly string.
        /// </summary>
        /// <returns>Formatted string</returns>
        protected string GetParamsForAnalytics()
        {
            List<string> values = new List<string>();
            string result = string.Empty;
            HttpRequest request = HttpContext.Current.Request;

            // Add Cancer Type / Keyword 
            if(!String.IsNullOrWhiteSpace(request.QueryString[CANCERTYPE_PARAM]))
            {
                values.Add("typecondition|" + request.QueryString[CANCERTYPE_PARAM]);
            }
            else if (!String.IsNullOrWhiteSpace(request.QueryString[PHRASE_PARAM]))
            {
                values.Add("keyword|" + request.QueryString[PHRASE_PARAM]);
            }
            else
            {
                values.Add("none");
            }

            // Add Zipcode
            if (!String.IsNullOrWhiteSpace(request.QueryString[ZIP_PARAM]))
            {
                values.Add(request.QueryString[ZIP_PARAM]);
            }
            else
            {
                values.Add("none");
            }

            // Add age
            if (!String.IsNullOrWhiteSpace(request.QueryString[AGE_PARAM]))
            {
                values.Add(request.QueryString[AGE_PARAM]);
            }
            else
            {
                values.Add("none");
            }

            // Join all valid query values 
            result = String.Join("|", values);
            return result;
        }

        #endregion

        #region Velocity Helpers

        /// <summary>
        /// Gets the Starting Number for the Results Being Displayed
        /// </summary>
        /// <returns></returns>
        public string GetStartItemNum()
        {            
            return (((SearchParams.Page - 1) * SearchParams.ItemsPerPage) + 1).ToString();
        }

        /// <summary>
        /// Gets the ending number for the results being displayed
        /// </summary>
        /// <param name="totalResults"></param>
        /// <returns></returns>
        public string GetEndItemNum(long totalResults)
        {
            long possibleLast = (SearchParams.Page * SearchParams.ItemsPerPage);
            if (possibleLast > totalResults)
                return totalResults.ToString();
            else
                return possibleLast.ToString();
        }

        /// <summary>
        /// Returns the cancer type the user searched for if the current search contains a type/condition.
        /// </summary>
        /// <returns></returns>
        public string GetCancerType()
        {
            string type = SearchParams is CancerTypeSearchParam ? ((CancerTypeSearchParam)SearchParams).CancerTypeDisplayName : null;
            if (string.IsNullOrWhiteSpace(type))
                type = null;
            return type;
        }

        /// <summary>
        /// Returns the phrase the user searched for if the current search contains a phrase.
        /// </summary>
        /// <returns></returns>
        public string GetPhrase()
        {
            string phrase = SearchParams is PhraseSearchParam ? ((PhraseSearchParam)SearchParams).Phrase : null;
            if (string.IsNullOrWhiteSpace(phrase))
                phrase = null;
            return phrase;
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
            if ((this.hasInvalidSearchParam == false) && (_setFields == QueryFieldsSetByUser.None))
                return true;
            else
                return false;
        }

        /// <summary>
        /// Determine whether the page number provided is above the maximum number of pages available with results.
        /// </summary>
        /// <param name="totalResults"></param>
        /// <returns></returns>
        public bool OutOfBounds(long totalResults)
        {
            int maxPage = (int)Math.Ceiling((double)totalResults / (double)SearchParams.ItemsPerPage);
            if (SearchParams.Page > maxPage)
                return true;
            else
                return false;
        }

        /// <summary>
        /// Returns a boolean that defines whether an invalid search parameter has been entered or not.
        /// </summary>
        /// <returns></returns>
        public bool HasInvalidParams()
        {
            return this.hasInvalidSearchParam;
        }

        /// <summary>
        /// Returns a boolean that defines whether the cancer type is being searched for as a phrase.
        /// This will happen when autosuggest is broken.
        /// </summary>
        /// <returns></returns>
        public bool HasBrokenCTSearchParam()
        {
            return SearchParams is PhraseSearchParam ? ((PhraseSearchParam)SearchParams).IsBrokenCTSearchParam : false;
        }


        /// <summary>
        /// Gets the View URL for an ID
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public string GetDetailedViewUrl(string id)
        {
            NciUrl url = new NciUrl();
            url.SetUrl(BasicCTSPageInfo.DetailedViewPagePrettyUrl);

            url.QueryParameters.Add(NCT_ID, id);

            if (_setFields.HasFlag(QueryFieldsSetByUser.ZipCode))
                url.QueryParameters.Add(ZIP_PARAM, SearchParams.ZipLookup.PostalCode_ZIP);

            if (_setFields.HasFlag(QueryFieldsSetByUser.ZipProximity))
                url.QueryParameters.Add(ZIPPROX_PARAM, SearchParams.ZipRadius.ToString());

            if (_setFields.HasFlag(QueryFieldsSetByUser.Age))
                url.QueryParameters.Add(AGE_PARAM, SearchParams.Age.ToString());

            if (_setFields.HasFlag(QueryFieldsSetByUser.Gender))
            {
                if (SearchParams.Gender == BaseCTSSearchParam.GENDER_FEMALE)
                    url.QueryParameters.Add(GENDER_PARAM, "1");
                else if (SearchParams.Gender == BaseCTSSearchParam.GENDER_MALE)
                    url.QueryParameters.Add(GENDER_PARAM, "2");
            }

            if (_setFields.HasFlag(QueryFieldsSetByUser.Country))
            {
                url.QueryParameters.Add(LOCATION_COUNTRY, SearchParams.Country);
            }
            if (_setFields.HasFlag(QueryFieldsSetByUser.City))
            {
                url.QueryParameters.Add(LOCATION_CITY, SearchParams.City);
            }
            if (_setFields.HasFlag(QueryFieldsSetByUser.State))
            {
                url.QueryParameters.Add(LOCATION_STATE, SearchParams.State);
            }
            if (_setFields.HasFlag(QueryFieldsSetByUser.Hospital))
            {
                url.QueryParameters.Add(HOSPITAL_INSTITUTION, SearchParams.HospitalOrInstitution);
            }
            if (_setFields.HasFlag(QueryFieldsSetByUser.AtNIH))
            {
                url.QueryParameters.Add(AT_NIH, SearchParams.AtNIH.ToString());
            }
            if (_setFields.HasFlag(QueryFieldsSetByUser.TrialType))
            {
                url.QueryParameters.Add(TRIAL_TYPE, SearchParams.TrialType);
            }
            if (_setFields.HasFlag(QueryFieldsSetByUser.DrugCode))
            {
                url.QueryParameters.Add(DRUG_CODE, String.Join(",", SearchParams.DrugIDs));
            }
            if (_setFields.HasFlag(QueryFieldsSetByUser.DrugName))
            {
                url.QueryParameters.Add(DRUG_NAME, SearchParams.DrugName);
            }
            if (_setFields.HasFlag(QueryFieldsSetByUser.TreatmentCode))
            {
                url.QueryParameters.Add(TREATMENT_CODE, String.Join(",", SearchParams.TreatmentInterventionCodes));
            }
            if (_setFields.HasFlag(QueryFieldsSetByUser.TreatmentName))
            {
                url.QueryParameters.Add(TREATMENT_NAME, SearchParams.TreatmentInterventionTerm);
            }
            if (_setFields.HasFlag(QueryFieldsSetByUser.TrialPhase))
            {
                url.QueryParameters.Add(TRIAL_PHASE, SearchParams.TrialPhase);
            }
            if (_setFields.HasFlag(QueryFieldsSetByUser.NewTrialsOnly))
            {
                var test = true.ToString();
                url.QueryParameters.Add(NEW_TRIALS_ONLY, SearchParams.NewTrialsOnly.ToString());
            }
            // This needs to be converted into the csv format.
            if (_setFields.HasFlag(QueryFieldsSetByUser.TrialIDs))
            {
                url.QueryParameters.Add(TRIAL_IDS, String.Join(",", SearchParams.TrialIDs));
            }
            if (_setFields.HasFlag(QueryFieldsSetByUser.TrialInvestigator))
            {
                url.QueryParameters.Add(TRIAL_INVESTIGATOR, SearchParams.PrincipalInvestigator);
            }
            if (_setFields.HasFlag(QueryFieldsSetByUser.LeadOrganization))
            {
                url.QueryParameters.Add(LEAD_ORGANIZATION, SearchParams.LeadOrganization);
            }

            //Phrase and type are based on the type of object
            if (SearchParams is CancerTypeSearchParam)
            {
                if (_setFields.HasFlag(QueryFieldsSetByUser.CancerType))
                    url.QueryParameters.Add(CANCERTYPE_PARAM, cancerTypeIDAndHash);
            }
            if (SearchParams is PhraseSearchParam)
            {
                if (_setFields.HasFlag(QueryFieldsSetByUser.Phrase))
                {
                    if (((PhraseSearchParam)SearchParams).IsBrokenCTSearchParam)
                        url.QueryParameters.Add(CANCERTYPEASPHRASE_PARAM, HttpUtility.UrlEncode(((PhraseSearchParam)SearchParams).Phrase));
                    else
                        url.QueryParameters.Add(PHRASE_PARAM, HttpUtility.UrlEncode(((PhraseSearchParam)SearchParams).Phrase));
                }
            }

            // Add subtypes, stage, and findings to query params
            if (_setFields.HasFlag(QueryFieldsSetByUser.CancerSubtype))
                url.QueryParameters.Add(CANCERTYPE_SUBTYPE, subtypeCCode);
            if (_setFields.HasFlag(QueryFieldsSetByUser.CancerStage))
                url.QueryParameters.Add(CANCERTYPE_STAGE, stageCCode);
            if (_setFields.HasFlag(QueryFieldsSetByUser.CancerFindings))
                url.QueryParameters.Add(CANCERTYPE_FINDINGS, findingsCCode);

            //Items Per Page
            url.QueryParameters.Add(ITEMSPP_PARAM, SearchParams.ItemsPerPage.ToString());

            // Page number
            url.QueryParameters.Add(PAGENUM_PARAM, SearchParams.Page.ToString());

            // Add the "rl" flag, indicating that this is a link coming from the CTS Results Page
            url.QueryParameters.Add(RESULTS_LINK_FLAG, "1"); 

            return url.ToString();
        }

        /// <summary>
        /// Gets URL for a Single Page of Results
        /// </summary>
        /// <param name="pageNum"></param>
        /// <returns></returns>
        public string GetPageUrl(int pageNum)
        {
            NciUrl url = this.PageInstruction.GetUrl("CurrentURL");

            if(!url.QueryParameters.ContainsKey(PAGENUM_PARAM))
            {
                url.QueryParameters.Add(PAGENUM_PARAM, pageNum.ToString());
            }
            else
            {
                url.QueryParameters[PAGENUM_PARAM] = pageNum.ToString();
            }

            return url.ToString();
        }

        /// <summary>
        /// Gets the Urls and Labels for all the pages of results from Curr Page - numLeft to Curr Page + numRight
        /// </summary>
        /// <param name="numLeft">The number of pages to display left of the selected page</param>
        /// <param name="numRight">The number of pages to display to the right of the selected page</param>
        /// <param name="totalResults">The total number of results </param>
        /// <returns></returns>
        public IEnumerable<object> GetPagerItems(int numLeft, int numRight, long totalResults)
        {
            int startPage = (SearchParams.Page - numLeft) >= 1 ? SearchParams.Page - numLeft : 1; // Current page minus left limit (numLeft)
            int maxPage = (int)Math.Ceiling((double)totalResults / (double)SearchParams.ItemsPerPage); // Highest available page
            int endPage = (SearchParams.Page + numRight) <= maxPage ? SearchParams.Page + numRight : maxPage; // Current page plus right limit (numRight)

            // The maximum number of elements that can be drawn in this pager. his would be:
            // ("< Previous" + 1 + 2/ellipsis + numLeft + current + numRight + next-to-last/ellipsis + "Next >") 
            int itemsTotalMax = 7 + numLeft + numRight;

            // If the pageNumber parameter is set above the highest available page number, set the start page to the endPage value.
            if (SearchParams.Page > endPage)
            {
                startPage = (endPage - numLeft) >= 1 ? endPage - numLeft : 1;
            }

            // If maxPage == 1, then only one page of results is found. Therefore, return null for the pager items.
            // Otherwise, set up the pager accordingly.
            if (maxPage > 1)
            {
                // Create a list of pager item objects:
                //  Text (string) = link text
                //  PageUrl (string) = href value for item 
                //  IsLink (bool) - whether or not this item will be used as a link
                List<object> items = new List<object>();

                // Draw text and links for first & previous pages
                if (SearchParams.Page != 1)
                {
                    // Draw link to previous page
                    string PrevUrl = GetPageUrl(SearchParams.Page - 1);
                    items.Add(
                    new
                    {
                        Text = "&lt; Previous",
                        PageUrl = PrevUrl
                    });

                    // add previous url to filter
                    this.PageInstruction.AddUrlFilter("RelPrev", (name, url) =>
                    {
                        url.SetUrl(PrevUrl);
                    });

                    // Draw first page links and text
                    if (SearchParams.Page > (numLeft + 1))
                    {
                        // Draw link to first page
                        items.Add(
                        new
                        {
                            Text = "1",
                            PageUrl = GetPageUrl(1)
                        });

                        // Draw elipses to delimit first page. 
                        // If the ellipses only represent a single digit, draw that instead (in this case it will always be 2).
                        if (SearchParams.Page > (numLeft + 2))
                        {
                            if (SearchParams.Page == (numLeft + 3))
                            {
                                items.Add(
                                new
                                {
                                    Text = "2",
                                    PageUrl = GetPageUrl(2)
                                });
                            }
                            else
                            {
                                items.Add(
                                new
                                {
                                    Text = "...",
                                    IsLink = false
                                });
                            }
                        }
                    }
                }

                // Draw links before and after current
                for (int i = startPage; i <= endPage; i++)
                {
                    items.Add(
                        new
                        {
                            Text = i.ToString(),
                            PageUrl = GetPageUrl(i)
                        }
                    );
                }

                // Draw last page links and text
                if (SearchParams.Page < endPage)
                {
                    // Draw iink to last page
                    if (SearchParams.Page < (maxPage - numRight))
                    {
                        // Draw elipses to delimit last page. 
                        // If the ellipses only represent a single digit, draw that instead (in this case it will always maxmimum page minus one).
                        if (SearchParams.Page < (maxPage - numRight - 1))
                        {
                            if (SearchParams.Page == (maxPage - numRight - 2))
                            {
                                items.Add(
                                new
                                {
                                    Text = (maxPage - 1).ToString(),
                                    PageUrl = GetPageUrl(maxPage - 1)
                                });
                            }
                            else
                            {
                                items.Add(
                                new
                                {
                                    Text = "...",
                                    IsLink = false
                                });
                            }
                        }

                        // Draw link to last page
                        items.Add(
                        new
                        {
                            Text = maxPage.ToString(),
                            PageUrl = GetPageUrl(maxPage)
                        });
                    }

                    // Draw link to next page
                    string NextUrl = GetPageUrl(SearchParams.Page + 1);
                    items.Add(
                    new
                    {
                        Text = "Next &gt;",
                        PageUrl = NextUrl
                    });

                    this.PageInstruction.AddUrlFilter("RelNext", (name, url) =>
                    {
                        url.SetUrl(NextUrl);
                    });

                }

                // Remove any duplicate links that may have slipped though. This only occurs in cases where the URL query param 
                // is greater than the last available page.
                // Doing this after the fact to prevent mucking up the code above. This is an edge case and should be handled outside of
                // the general logic. 
                if (SearchParams.Page > maxPage)
                {
                    items = items.Distinct().ToList();
                    // Remove ellipsis if it shows betweeen two consecutive numbers
                    if (items.Count - 2 == maxPage)
                    {
                        items.RemoveAt(2);
                    }
                }

                return items;
            }
            else
            {
                return null;
            }
        }

        #endregion

    }
}
