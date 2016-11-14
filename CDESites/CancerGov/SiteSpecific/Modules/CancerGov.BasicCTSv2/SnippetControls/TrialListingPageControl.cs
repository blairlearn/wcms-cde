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
using Common.Logging;

using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

using CancerGov.ClinicalTrialsAPI;
using NCI.Web;
using NCI.Web.CDE;
using NCI.Web.CDE.Application;
using NCI.Web.CDE.Modules;
using NCI.Web.CDE.UI;

namespace CancerGov.ClinicalTrials.Basic.v2.SnippetControls
{
    public class TrialListingPageControl : BasicCTSBaseControl
    {
        static ILog log = LogManager.GetLogger(typeof(TrialListingPageControl));

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
            SearchParams = GetSearchParams();
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            // Get the JSON blob from the XML in the content item (AppModule). This overrides anything passed via the URL
            String xmlFilters = BasicCTSPageInfo.JSONBodyRequest;
            JObject dynamicParams = GetDeserializedJSON(xmlFilters);

            // Get the filter parameters from the URL. URL filter params should NOT override any matching params set in the JSON
            String urlFilters = GetUrlFilters();
            JObject urlParams = GetDeserializedJSON(urlFilters);

            // Merge both sets of dynamic filter params (first arg is the override)
            dynamicParams = MergeJObjects(dynamicParams, urlParams);

            //Do the search
            var results = _basicCTSManager.Search(SearchParams, dynamicParams);

            /*TODO:
             * - Demo code - this may be removed depending on feedback 2016/11/15 
             * - Add params for max # of return values
             */
            bool isRedirectable = (BasicCTSPageInfo.RedirectOnNoResults) ? BasicCTSPageInfo.RedirectOnNoResults : false;
            int minResults = (BasicCTSPageInfo.ListingMinResults != null) ? BasicCTSPageInfo.ListingMinResults : 0;
            if (isRedirectable && results.TotalResults <= minResults) // If there are not enough results, raise the "No Results" error page (URL stays the same)
            {
                ErrorPageDisplayer.RaiseClinicalTrialsNoResults(this.GetType().ToString());
            }
            else // Show search results
            {
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
            }
        }


        #region JSON manipulation methods

        /// <summary>
        /// Get filter params from URL and format them.
        /// </summary>
        /// <returns>JSON-formatted string</returns>
        protected string GetUrlFilters()
        {
            Dictionary<string, string> urlParams = new Dictionary<string, string>();
            Regex pattern = new Regex(@"filter\[([^]]*)\]", RegexOptions.Compiled | RegexOptions.IgnoreCase);
            List<string> values = new List<string>();
            String result = "{}";

            // For each query param that matches the "filter[]" pattern, add it to the list of filter values
            foreach (string key in HttpContext.Current.Request.QueryString.AllKeys)
            {
                if (!string.IsNullOrEmpty(key) && pattern.IsMatch(key))
                {
                    Match match = pattern.Match(key);
                    string queryValue = match.Groups[1].Value;
                    string queryParam = HttpContext.Current.Request.QueryString[match.Value];
                    if (!string.IsNullOrEmpty(queryParam)) // Don't filter empty params
                    {
                        values.Add("\"" + queryValue + "\":[\"" + queryParam.Replace(",", "\",\"") + "\"]");
                    }
                }
            }
            // Join all of our valid key-value pairs 
            result = result.Insert(1, string.Join(",", values.ToArray()));
            
            return result;
        }

        /// <summary>
        /// Deserialize a JSON-formatted string that into a JObject.
        /// </summary>
        /// <param name="jsonBlob"></param>
        /// <returns>JSON object</returns>
        protected JObject GetDeserializedJSON(String jsonBlob)
        {
            JObject result = new JObject();

            //Add dynamic filter criteria
            if (!String.IsNullOrEmpty(jsonBlob))
            {
                //Deserialize our JSON string into a dictionary object, then add it to our Json.NET object 
                try
                {
                    Dictionary<string, object> kvps = JsonConvert.DeserializeObject<Dictionary<string, object>>(jsonBlob);
                    foreach (KeyValuePair<string, object> kvp in kvps)
                    {
                        result.Add(new JProperty(kvp.Key, kvp.Value));
                    }
                }
                catch (Exception ex)
                {
                    log.Error("TrialListingPageControl:GetDeserializedJSON() - Error converting String to JSON.", ex);
                    return null;
                }
            }
            return result;
        }

        /// <summary>
        /// Take two JObjects and merge, with the first arg being the override
        /// </summary>
        /// <param name="primary"></param>
        /// <param name="secondary"></param>
        /// <returns>Merged JSON object</returns>
        protected JObject MergeJObjects(JObject primary, JObject secondary)
        {
            //Add dynamic filter criteria
            if (primary != null && secondary != null)
            {
                //Merge objects (primary overrides secondary)
                secondary.Merge(primary, new JsonMergeSettings { MergeArrayHandling = MergeArrayHandling.Concat });
            }
            return secondary;
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
            url.QueryParameters.Add("id", id);
            url.QueryParameters.Add("ni", SearchParams.ItemsPerPage.ToString()); //Items Per Page
            url.QueryParameters.Add("pn", SearchParams.Page.ToString()); //Page number
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

            // For each query param that matches the "filter[]" pattern, add it to our query parameters 
            // Logic borrowed from GetUrlFilters() 
            Regex pattern = new Regex(@"filter\[([^]]*)\]", RegexOptions.Compiled | RegexOptions.IgnoreCase);
            foreach (string key in HttpContext.Current.Request.QueryString.AllKeys)
            {
                if (!string.IsNullOrEmpty(key) && pattern.IsMatch(key))
                {
                    Match match = pattern.Match(key);
                    string queryValue = key;
                    string queryParam = HttpContext.Current.Request.QueryString[match.Value];
                    // Don't carry over empty or preexisting contained queries
                    if (!string.IsNullOrEmpty(queryParam) && !url.QueryParameters.ContainsKey(key))
                    {
                        url.QueryParameters.Add(queryValue, queryParam);
                    }
                }
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
