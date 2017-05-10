using System;
using System.Configuration;
using System.Text.RegularExpressions;
using System.Linq;
using System.Collections.Generic;
using System.Web;
using System.IO;
using CancerGov.ClinicalTrials.Basic.v2.Configuration;
using Common.Logging;
using NCI.Web;
using NCI.Web.CDE;
using NCI.Web.CDE.Modules;
using NCI.Web.CDE.UI;
using CancerGov.ClinicalTrials.Basic.v2.SnippetControls.Configs;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Serialization;
using System.Web.UI;

namespace CancerGov.ClinicalTrials.Basic.v2.SnippetControls
{
    /// <summary>
    /// Serves as the base class for the different types of trial listing pages.
    /// </summary>
    public abstract class BaseTrialListingControl : SnippetControl
    {
        /// <summary>
        /// basic CTS query parameters
        /// </summary>
        protected const string PAGENUM_PARAM = "pn";
        protected const string ITEMSPP_PARAM = "ni";
        protected const string REDIRECTED_FLAG = "r";
        protected const string RESULTS_LINK_FLAG = "rl";

        // This control still shares BasicCTSManager with the CTS controls
        private BasicCTSManager _basicCTSManager = null;
        private string _APIURL = string.Empty;
        private bool hasInvalidSearchParam;

        static ILog log = LogManager.GetLogger(typeof(BaseTrialListingControl));

        /// <summary>
        /// Gets the configuration type of the derrieved class
        /// </summary>
        /// <returns></returns>
        protected abstract Type GetConfigType();

        /// <summary>
        /// Gets the base query for the trial listing.  
        /// <remarks>This should not have pagination, nor should it have additional filters added from Query Parameters</remarks>
        /// </summary>
        /// <returns></returns>
        protected abstract JObject GetTrialQuery();

        /// <summary>
        /// Internal method to get the "No Trials" HTML blob from the xml to pass to the velocity template
        /// </summary>
        /// <returns>HTML string</returns>
        protected abstract String InternalGetNoTrialsHtml();

        /// <summary>
        /// The configuration from the page xml
        /// </summary>
        protected BaseTrialListingConfig Config { get; private set; }

        /// <summary>
        /// Gets the view page pretty URL
        /// </summary>
        private string DetailedViewPagePrettyUrl
        { 
            get
            {
                return "/clinicaltrials/{0}";
            }
        }

        /// <summary>
        /// The number of items per page (for pager)
        /// </summary>
        private int ItemsPerPage { get; set; }

        /// <summary>
        /// The current page number (for pager) 
        /// </summary>
        private int PageNum { get; set; }

        /// <summary>
        /// Gets the URL for the ClinicalTrials API from BasicClinicalTrialSearchAPISection:GetAPIUrl()
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

        protected sealed override void OnInit(EventArgs e)
        {
            base.OnInit(e);

            this.LoadConfig();

            _basicCTSManager = new BasicCTSManager(APIURL);
        }

        protected sealed override void OnPreRender(EventArgs e)
        {
            base.OnPreRender(e);
            
            //Get Trial Query, inject pager stuff, call manager
            JObject query = new JObject(this.GetTrialQuery());
            
            //add in URL filters to query

            //Setup the pager.
            BaseCTSSearchParam searchParams = this.GetSearchParamsForListing();

            //fetch results
            var results = _basicCTSManager.Search(searchParams, query);
            

            //Load VM File and show search results
            LiteralControl ltl = new LiteralControl(VelocityTemplate.MergeTemplateWithResultsByFilepath(
                this.Config.ResultsPageTemplatePath,
                new
                {
                    Results = results,
                    Control = this,
                    TrialTools = new TrialVelocityTools()
                }
            ));
            Controls.Add(ltl);

            //Set Analytics
        }

        /// <summary>
        /// Loads the JSON configuration from the SnippetInfo's Data
        /// </summary>
        private void LoadConfig()
        {
            Type configType = this.GetConfigType();

            // Read the basic CTS page information JSON
            string sidata = this.SnippetInfo.Data;

            try
            {
                if (string.IsNullOrWhiteSpace(sidata))
                {
                    throw new Exception("TrialListingConfig not present in JSON, associate an Application Module item with this page in Percussion");
                }
                sidata = sidata.Trim();

                // Get our TrialListingPageInfo object this is JSON data that includes template and URL paths and result count parameters.
                // It also includes a nested, JSON-formatted string "RequestFilters", which represents the JSON passed in with the API body request - this is
                // deserialized in TrialListingPageControl.
                // TODO: handle all deserialization in once place, if possible. This will avoid having to go through the process twice
                this.Config = (BaseTrialListingConfig)JsonConvert.DeserializeObject(sidata, configType);

            }
            catch (Exception ex)
            {
                log.Error("Could not load the TrialListingPageInfo; check the config info of the Application Module item in Percussion", ex);
                throw ex;
            }
        }

        /// <summary>
        /// Assemble the default (non-dynamic) search parameters for a listing page.
        /// </summary>
        /// <returns>Search params object</returns>
        private BaseCTSSearchParam GetSearchParamsForListing()
        {
            //Parse Parameters
            int pageNum = this.ParmAsInt(PAGENUM_PARAM, 1);
            int itemsPerPage = this.ParmAsInt(ITEMSPP_PARAM, this.Config.DefaultItemsPerPage);

            //BaseCTSSearchParam searchParams = null;
            BaseCTSSearchParam searchParams = new ListingSearchParam();

            // Set Page and Items Per Page
            if (pageNum < 1)
            {
                searchParams.Page = 1;
            }
            else
            {
                searchParams.Page = pageNum;
            }
            searchParams.ItemsPerPage = itemsPerPage;

            // Set ItemsPerPage and PageNum for use in Velocity Helpers
            this.ItemsPerPage = itemsPerPage;
            this.PageNum = pageNum;

            return searchParams;
        }

        /// <summary>
        /// Gets a query parameter as a string or uses a default
        /// </summary>
        /// <param name="param"></param>
        /// <param name="def"></param>
        /// <returns></returns>
        private string ParmAsStr(string param, string def)
        {
            string paramval = Request.QueryString[param];

            if (string.IsNullOrWhiteSpace(paramval))
                return def;
            else
                return paramval.Trim();
        }

        /// <summary>
        /// Gets a query parameter as an int or uses a default
        /// </summary>
        /// <param name="param"></param>
        /// <param name="def"></param>
        /// <returns></returns>
        private int ParmAsInt(string param, int def)
        {
            string paramval = Request.QueryString[param];

            if (string.IsNullOrWhiteSpace(paramval))
            {
                return def;
            }
            else
            {
                int tmpInt = 0;
                if (int.TryParse(paramval.Trim(), out tmpInt))
                {
                    if (tmpInt == 0)
                        hasInvalidSearchParam = true;

                    return tmpInt;
                }
                else
                {
                    hasInvalidSearchParam = true;
                    return def;
                }
            }
        }


        #region Velocity Helpers

        /// <summary>
        /// Gets the Starting Number for the Results Being Displayed
        /// </summary>
        /// <returns></returns>
        public int GetStartItemNum()
        {
            return (((this.PageNum - 1) * this.ItemsPerPage) + 1);
        }

        /// <summary>
        /// Gets the ending number for the results being displayed
        /// </summary>
        /// <param name="totalResults"></param>
        /// <returns></returns>
        public int GetEndItemNum(long totalResults)
        {
            long possibleLast = (this.PageNum * this.ItemsPerPage);
            if (possibleLast > totalResults)
            {
                possibleLast = totalResults;
            }

            // Convert long value into int 
            try
            {
                int val = Convert.ToInt32(possibleLast);
                return val;
            }
            catch (OverflowException ex)
            {
                log.Error("TrialListingPageControl:GetEndItemNum() - " + possibleLast.ToString() + "is outside the range of the Int32 type.", ex);
                return 0;
            }
        }

        /// <summary>
        /// Determine whether the page number provided is above the maximum number of pages available with results.
        /// </summary>
        /// <param name="totalResults"></param>
        /// <returns></returns>
        public bool OutOfBounds(long totalResults)
        {
            int maxPage = (int)Math.Ceiling((double)totalResults / (double)this.ItemsPerPage);
            if (this.PageNum > maxPage)
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
        //public bool HasBrokenCTSearchParam()
        //{
        //    return SearchParams is PhraseSearchParam ? ((PhraseSearchParam)SearchParams).IsBrokenCTSearchParam : false;
        //}

        /// <summary>
        /// Gets the View URL for an ID
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public string GetDetailedViewUrl(string id)
        {
            NciUrl url = new NciUrl();
            url.SetUrl(string.Format(this.DetailedViewPagePrettyUrl, id));
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

            if (!url.QueryParameters.ContainsKey("pn"))
            {
                url.QueryParameters.Add("pn", pageNum.ToString());
            }
            else
            {
                url.QueryParameters["pn"] = pageNum.ToString();
            }

            /*// For each query param that matches the "filter[]" pattern, add it to our query parameters 
            foreach (string key in HttpContext.Current.Request.QueryString.AllKeys)
            {
                if (!string.IsNullOrWhiteSpace(key) && FilterPattern.IsMatch(key))
                {
                    Match match = FilterPattern.Match(key);
                    string queryValue = key;
                    string queryParam = HttpContext.Current.Request.QueryString[match.Value];
                    // Don't carry over empty or preexisting contained queries
                    if (!string.IsNullOrWhiteSpace(queryParam) && !url.QueryParameters.ContainsKey(key))
                    {
                        url.QueryParameters.Add(queryValue, queryParam);
                    }
                }
            }*/

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
            int startPage = (this.PageNum - numLeft) >= 1 ? this.PageNum - numLeft : 1; // Current page minus left limit (numLeft)
            int maxPage = (int)Math.Ceiling((double)totalResults / (double)this.ItemsPerPage); // Highest available page
            int endPage = (this.PageNum + numRight) <= maxPage ? this.PageNum + numRight : maxPage; // Current page plus right limit (numRight)

            // The maximum number of elements that can be drawn in this pager. his would be:
            // ("< Previous" + 1 + 2/ellipsis + numLeft + current + numRight + next-to-last/ellipsis + "Next >") 
            int itemsTotalMax = 7 + numLeft + numRight;

            // If the pageNumber parameter is set above the highest available page number, set the start page to the endPage value.
            if (this.PageNum > endPage)
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
                if (this.PageNum != 1)
                {
                    // Draw link to previous page
                    string PrevUrl = GetPageUrl(this.PageNum - 1);
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
                    if (this.PageNum > (numLeft + 1))
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
                        if (this.PageNum > (numLeft + 2))
                        {
                            if (this.PageNum == (numLeft + 3))
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
                if (this.PageNum < endPage)
                {
                    // Draw iink to last page
                    if (this.PageNum < (maxPage - numRight))
                    {
                        // Draw elipses to delimit last page. 
                        // If the ellipses only represent a single digit, draw that instead (in this case it will always maxmimum page minus one).
                        if (this.PageNum < (maxPage - numRight - 1))
                        {
                            if (this.PageNum == (maxPage - numRight - 2))
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
                    string NextUrl = GetPageUrl(this.PageNum + 1);
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
                if (this.PageNum > maxPage)
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

        /// <summary>
        /// Gets the "No Trials" HTML blob from the xml to pass to the velocity template
        /// </summary>
        /// <returns>HTML string</returns>
        public String GetNoTrialsHTML()
        {
            string htmlValue = InternalGetNoTrialsHtml();
            return string.IsNullOrWhiteSpace(htmlValue) ? string.Empty : htmlValue;
        }

        /// <summary>
        /// Gets the items per page from DefaultItemsPerPage in the XML or sets default
        /// </summary>
        /// <returns>int - items per page</returns>
        public int GetItemsPerPage()
        {
            int number = 50;
            if (this.Config.DefaultItemsPerPage > 0)
            {
                number = this.Config.DefaultItemsPerPage;
            }
            return number;
        }

        #endregion
    }
}
