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

using NCI.Web.CDE.UI;
using NCI.Web.CDE.Modules;
using NCI.Web.CDE;
using NCI.Web;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace CancerGov.ClinicalTrials.Basic.v2.SnippetControls
{
    // TODO: Clean up unused methods
    public class TrialListingPageControl : BasicCTSBaseControl
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
            SearchParams = GetSearchParams();
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            /* TODO: 
             * - Update logic to handle invalid param names without breaking the page (!)
             * - Update filtering element values / type as needed 
             * - Update Search() to accept additional argument OR combine Jsonfilters + urlparamfilters in this control
             * - Get list of common filter keys
             * - Clean up GetUrlFilters() 
             * - Check velocity helper methods against what is actually used in the template
             */
            // Get the JSON blob from the XML in the content item (AppModule)
            String jsonFilters = BasicCTSPageInfo.JSONBodyRequest;
            JObject jsonParms = GetDeserializedJSON(jsonFilters);


            // Get the filter parameters from the URL. URL filter params should NOT override any matching params set in the JSON
            String urlParamFilters = GetUrlFilters();
            JObject urlParms = GetDeserializedJSON(urlParamFilters);

            //Do the search
            //TODO: add merge() method
            var results = _basicCTSManager.Search(SearchParams, jsonParms);
            //var results = _basicCTSManager.Search(SearchParams, urlParms);

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
        }


        #region Filter methods
        private string GetUrlFilters()
        {
            Dictionary<string, string> urlParams = new Dictionary<string, string>();
            Regex pattern = new Regex(@"filter\[([^]]*)\]", RegexOptions.Compiled | RegexOptions.IgnoreCase);
            List<string> values = new List<string>();

            foreach (string key in HttpContext.Current.Request.QueryString.AllKeys)
            {
                if (pattern.IsMatch(key))
                {
                    //ret += key + @"<spacer>";
                    Match match = pattern.Match(key);
                    values.Add(@"""" + match.Groups[1].Value + @""":[""" + HttpContext.Current.Request.QueryString[match.Value] + @"""]");
                }
            }
            string result = "{" + string.Join(",", values.ToArray()) + "}";


            //            return urlParams;
            //ret = String.Join("===", urlParams.Select(x => x.Key + ":::" + x.Value).ToArray());
            return result;
        }


        public JObject GetDeserializedJSON(String dynamicSearchParams)
        {
            JObject dynamicRequestBody = new JObject();

            //Add dynamic filter criteria
            if (!String.IsNullOrEmpty(dynamicSearchParams))
            {
                //Deserialize our JSON string into a dictionary object, then add it to our Json.NET object 
                Dictionary<string, object> dynFilters = JsonConvert.DeserializeObject<Dictionary<string, object>>(dynamicSearchParams);
                foreach (KeyValuePair<string, object> dynFilter in dynFilters)
                {
                    dynamicRequestBody.Add(new JProperty(dynFilter.Key, dynFilter.Value));
                }
            }
            return dynamicRequestBody;
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
