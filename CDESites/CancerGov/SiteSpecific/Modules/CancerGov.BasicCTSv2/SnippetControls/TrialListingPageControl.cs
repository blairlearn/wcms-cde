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
    /// <summary>
    /// Page control for Trial Listing pages.
    /// This is the code behind for TrialListingPage.ascx snippet template.
    /// </summary>
    public class TrialListingPageControl : TrialListingBaseControl
    {
        /// <summary>
        /// Set logging for this class.
        /// </summary>
        static ILog log = LogManager.GetLogger(typeof(TrialListingPageControl));

        /// <summary>
        /// Create the Regex pattern for finding filters in the URL.
        /// Match the following pattern: "filter" + open square bracket ([) + any string value + close square bracket (]).
        /// </summary>
        private readonly Regex FilterPattern = new Regex(@"filter\[([^]]*)\]", RegexOptions.Compiled | RegexOptions.IgnoreCase);

        /// <summary>
        /// Gets the Search Parameters for the current request.
        /// </summary>
        public BaseCTSSearchParam SearchParams { get; private set; }

        /// <summary>
        /// Retrieve the working URL of this control from the page XML.
        /// </summary>
        protected override String WorkingUrl
        {
            get { return TrialListingPageInfo.ResultsPagePrettyUrl; }
        }

        /// <summary>
        /// Override method to raise the Init event.
        /// </summary>
        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            //SearchParams = GetSearchParams();
            SearchParams = GetSearchParamsForListing();
        }

        /// <summary>
        /// Override method to raise the Load event.
        /// </summary>
        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            // Get the RequestFilters element that was set in the content item JSON. 
            // These filter params cannot be overridden by the URL filter params below.
            JObject dynamicParams = TrialListingPageInfo.RequestFilters;

            // Get the filter parameters from the URL. URL filter params should NOT override any matching params set in the JSON
            String urlFilters = GetUrlFilters();
            JObject urlParams = GetDeserializedJSON(urlFilters);

            // Merge both sets of dynamic filter params.'dynamicParams' override 'urlParams', meaning that any duplicate elements in urlParams 
            // will not be included, e.g., given
            // dynamicParams == { "key1":value1, "key2":["value2,value3"] }
            // and urlParms == { "key2":["value4,value5"], "key3":"value6" }
            // merged dynamicParams == { "key1":value1, "key2":["value2,value3"], "key3":"value6" }
            dynamicParams = MergeJObjects(dynamicParams, urlParams);

            // Set the number of items per page
            SearchParams.ItemsPerPage = GetItemsPerPage();

            // Do the search
            var results = _basicCTSManager.Search(SearchParams, dynamicParams);

            // Show search results
            LiteralControl ltl = new LiteralControl(VelocityTemplate.MergeTemplateWithResultsByFilepath(
                TrialListingPageInfo.ResultsPageTemplatePath,
                new
                {
                    Results = results,
                    Control = this,
                    TrialTools = new TrialVelocityTools()
                }
            ));
            Controls.Add(ltl);

        }


        #region JSON manipulation methods

        /// <summary>
        /// Get filter params from URL and format them.
        /// </summary>
        /// <returns>JSON-formatted string</returns>
        protected string GetUrlFilters()
        {
            Dictionary<string, string> urlParams = new Dictionary<string, string>();
            List<string> values = new List<string>();
            String result = "{}";

            // For each query param that matches the "filter[]" pattern, add it to the list of filter values
            foreach (string key in HttpContext.Current.Request.QueryString.AllKeys)
            {
                if (!string.IsNullOrWhiteSpace(key) && FilterPattern.IsMatch(key))
                {
                    Match match = FilterPattern.Match(key);
                    string queryValue = match.Groups[1].Value;
                    string queryParam = HttpContext.Current.Request.QueryString[match.Value];
                    if (!string.IsNullOrWhiteSpace(queryParam)) // Don't filter empty params
                    {
                        if (queryParam.IndexOf(",") > -1) // Splic comma-separated values
                        {
                            values.Add("\"" + queryValue + "\":[\"" + queryParam.Replace(",", "\",\"") + "\"]");
                        }
                        else
                        {
                            values.Add("\"" + queryValue + "\":\"" + queryParam + "\"");
                        }
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
        /// <param name="jsonBlob">JSON-formatted String object</param>
        /// <returns>JSON object</returns>
        protected JObject GetDeserializedJSON(String jsonBlob)
        {
            JObject result = new JObject();

            if (!String.IsNullOrWhiteSpace(jsonBlob))
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
        /// <param name="primary">Primary JSON object (overrides duplicates)</param>
        /// <param name="secondary">Secondary JSON object (duplicates are overridden)</param>
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
            url.SetUrl(TrialListingPageInfo.DetailedViewPagePrettyUrl);
            url.QueryParameters.Add("id", id);
            url.QueryParameters.Add("ni", SearchParams.ItemsPerPage.ToString()); //Items Per Page
            url.QueryParameters.Add("pn", SearchParams.Page.ToString()); //Page number
            url.QueryParameters.Add("r", "1"); //Add the "r" flag - this will allow us to add custom logic to CT View pages not linked from the CTS Results page
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

        /// <summary>
        /// Gets the "No Trials" HTML blob from the xml to pass to the velocity template
        /// </summary>
        /// <returns>HTML string</returns>
        public String GetNoTrialsHTML()
        {
            string htmlValue = string.Empty;
            if (TrialListingPageInfo.NoTrialsHTML != null)
            {
                htmlValue = TrialListingPageInfo.NoTrialsHTML;
            }
            return htmlValue;
        }

        /// <summary>
        /// Gets the items per page from DefaultItemsPerPage in the XML or sets default
        /// </summary>
        /// <returns>int - items per page</returns>
        public int GetItemsPerPage()
        {
            int number = 50;
            if (TrialListingPageInfo.DefaultItemsPerPage > 0)
            {
                number = TrialListingPageInfo.DefaultItemsPerPage;
            }
            return number;
        }

        #endregion

    }
}
