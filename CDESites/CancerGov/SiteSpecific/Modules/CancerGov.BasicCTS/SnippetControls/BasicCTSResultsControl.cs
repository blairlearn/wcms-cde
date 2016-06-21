﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using NCI.Web.CDE.UI;
using NCI.Web.CDE.Modules;
using NCI.Web.CDE;
using NCI.Web;

namespace CancerGov.ClinicalTrials.Basic.SnippetControls
{
    public class BasicCTSResultsControl : BasicCTSBaseControl
    {
        /// <summary>
        /// Gets the Search Parameters for the current request.
        /// </summary>
        public BaseCTSSearchParam SearchParams { get; private set; }
        public PhraseSearchParam PhraseSearchParams { get; set; }
        public CancerTypeSearchParam CancerTypeSearchParams { get; set; }

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);

            SearchParams = GetSearchParams();
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            //Do the search
            var results = _basicCTSManager.SearchTemplate(SearchParams);

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
                else if (invalidSearchParam)
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
                if ((_setFields & SetFields.Age) != 0)
                    url.QueryParameters.Add("a", SearchParams.Age.ToString());

                if ((_setFields & SetFields.Gender) != 0)
                {
                    if (SearchParams.Gender == BaseCTSSearchParam.GENDER_FEMALE)
                        url.QueryParameters.Add("g", "1");
                    else if (SearchParams.Gender == BaseCTSSearchParam.GENDER_MALE)
                        url.QueryParameters.Add("g", "2");
                }

                if ((_setFields & SetFields.ZipCode) != 0)
                    url.QueryParameters.Add("z", SearchParams.ZipLookup.PostalCode_ZIP);

                if ((_setFields & SetFields.ZipProximity) != 0)
                    url.QueryParameters.Add("zp", SearchParams.ZipRadius.ToString());

                //Phrase and type are based on the type of object
                if (SearchParams is CancerTypeSearchParam)
                {
                    CancerTypeSearchParams = (CancerTypeSearchParam)SearchParams;

                    if ((_setFields & SetFields.CancerType) != 0)
                        url.QueryParameters.Add("t", cancerTypeIDAndHash);
                }

                if (SearchParams is PhraseSearchParam)
                {
                    PhraseSearchParams = (PhraseSearchParam)SearchParams;
                    if ((_setFields & SetFields.Phrase) != 0)
                        url.QueryParameters.Add("q", HttpUtility.UrlEncode(PhraseSearchParams.Phrase));
                }

                //Items Per Page
                url.QueryParameters.Add("ni", SearchParams.ItemsPerPage.ToString());

            });

           
            // Show Results
            LiteralControl ltl = new LiteralControl(VelocityTemplate.MergeTemplateWithResultsByFilepath(
                BasicCTSPageInfo.ResultsPageTemplatePath, 
                new
                {
                    Results = results,
                    Control = this
                }
            ));
            Controls.Add(ltl);
        }

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
        public string HasType()
        {
            if (SearchParams is CancerTypeSearchParam)
            {
                CancerTypeSearchParams = (CancerTypeSearchParam)SearchParams;

                if (!string.IsNullOrWhiteSpace(CancerTypeSearchParams.CancerTypeDisplayName))
                    return CancerTypeSearchParams.CancerTypeDisplayName;
            }
            return null;
        }

        /// <summary>
        /// Returns the phrase the user searched for if the current search contains a phrase.
        /// </summary>
        /// <returns></returns>
        public string HasPhrase()
        {
            if (SearchParams is PhraseSearchParam)
            {
                PhraseSearchParams = (PhraseSearchParam)SearchParams;
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
            if ((this.invalidSearchParam == false) && (_setFields == SetFields.None))
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
            return this.invalidSearchParam;
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

            url.QueryParameters.Add("id", id);

            if ((_setFields & SetFields.ZipCode) != 0)
                url.QueryParameters.Add(ZIP_PARAM, SearchParams.ZipLookup.PostalCode_ZIP);

            if ((_setFields & SetFields.ZipProximity) != 0)
                url.QueryParameters.Add(ZIPPROX_PARAM, SearchParams.ZipRadius.ToString());

            if ((_setFields & SetFields.Age) != 0)
                url.QueryParameters.Add("a", SearchParams.Age.ToString());

            if ((_setFields & SetFields.Gender) != 0)
            {
                if (SearchParams.Gender == BaseCTSSearchParam.GENDER_FEMALE)
                    url.QueryParameters.Add("g", "1");
                else if (SearchParams.Gender == BaseCTSSearchParam.GENDER_MALE)
                    url.QueryParameters.Add("g", "2");
            }

            //Phrase and type are based on the type of object
            if (SearchParams is CancerTypeSearchParam)
            {
                CancerTypeSearchParams = (CancerTypeSearchParam)SearchParams;

                if ((_setFields & SetFields.CancerType) != 0)
                    url.QueryParameters.Add("t", cancerTypeIDAndHash);
            }

            if (SearchParams is PhraseSearchParam)
            {
                PhraseSearchParams = (PhraseSearchParam)SearchParams;
                if ((_setFields & SetFields.Phrase) != 0)
                    url.QueryParameters.Add("q", HttpUtility.UrlEncode(PhraseSearchParams.Phrase));
            }

            //Items Per Page
            url.QueryParameters.Add("ni", SearchParams.ItemsPerPage.ToString());

            // Page number
            url.QueryParameters.Add("pn", SearchParams.Page.ToString());

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

            if(!url.QueryParameters.ContainsKey("pn"))
            {
                url.QueryParameters.Add("pn", pageNum.ToString());
            }
            else
            {
                url.QueryParameters["pn"] = pageNum.ToString();
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
            int startPage = (SearchParams.Page - numLeft) >= 1 ? SearchParams.Page - numLeft : 1;
            int maxPage = (int)Math.Ceiling((double)totalResults / (double)SearchParams.ItemsPerPage);
            int endPage = (SearchParams.Page + numRight) <= maxPage ? SearchParams.Page + numRight : maxPage;
            if (SearchParams.Page > endPage)
                startPage = (endPage - numLeft) >= 1 ? endPage - numLeft : 1;

            // If maxPage == 1, then only one page of results is found. Therefore, return null for the pager items.
            // Otherwise, set up the pager accordingly.
            if (maxPage > 1)
            {
                List<object> items = new List<object>();

                if (SearchParams.Page != 1)
                    items.Add(
                        new
                        {
                            Text = "&lt; Previous",
                            PageUrl = GetPageUrl(SearchParams.Page - 1)
                        });

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

                if (SearchParams.Page < endPage)
                    items.Add(
                        new
                        {
                            Text = "Next &gt;",
                            PageUrl = GetPageUrl(SearchParams.Page + 1)
                        });



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
