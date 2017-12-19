using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using CancerGov.Dictionaries.Configuration;
using NCI.Util;
using NCI.Web;
using NCI.Web.CDE;
using NCI.Web.CDE.UI;
using NCI.Web.CDE.WebAnalytics;
using NCI.Web.Dictionary;
using NCI.Web.Dictionary.BusinessObjects;
using Microsoft.Security.Application;

namespace CancerGov.Dictionaries.SnippetControls.DrugDictionary
{
    public class DrugDictionaryResultsList : BaseDictionaryControl
    {
        protected DrugDictionaryHome dictionarySearchBlock;

        protected Panel numResDiv;

        protected Label lblNumResults;

        protected Label lblResultsFor;

        protected Label lblWord;

        protected ListView resultListView;

        protected Literal litPager;

        protected class DrugPager
        {
            private NciUrl pageBaseUrl = new NciUrl();
            private Dictionary<string, string> queryParams = new Dictionary<string, string>();
            private int showPages = 10;
            private int currentPage = 0;
            private int recordsPerPage = 10;
            private int recordCount = 0;

            #region Properties

            /// <summary>
            /// Property sets index of current page view
            /// </summary>
            public Dictionary<string, string> QueryParameters
            {
                get { return queryParams; }
                set { queryParams = value; }
            }

            /// <summary>
            /// Property sets index of current page view
            /// </summary>
            public int CurrentPage
            {
                get { return currentPage; }
                set { currentPage = value; }
            }

            /// <summary>
            /// Property sets number of records per page view
            /// </summary>
            public int RecordsPerPage
            {
                get { return recordsPerPage; }
                set { recordsPerPage = value; }
            }

            /// <summary>
            /// Property sets total number of records
            /// </summary>
            public int RecordCount
            {
                get { return recordCount; }
                set { recordCount = value; }
            }

            public int ShowPages
            {
                get { return showPages; }
                set { showPages = value; }
            }

            #endregion

            /// <summary>
            /// Default class constructor
            /// </summary>
            public DrugPager() { }

            public DrugPager(NciUrl pageBaseUrl, Dictionary<string, string> queryParams, int pageIndex, int pageSize, int pageCount, int itemCount)
            {
                this.pageBaseUrl = pageBaseUrl;
                this.queryParams = queryParams;
                this.currentPage = pageIndex;
                this.recordCount = itemCount;
                this.recordsPerPage = pageSize;
                this.showPages = pageCount;
            }

            /// <summary>
            /// Method that builds HTML paging constructs based on class properties
            /// </summary>
            /// <returns>Paging HTML links</returns>
            public string RenderPager()
            {
                string result = "";
                int startIndex = 0;
                int endIndex = 0;
                int pages = 0;

                //Get number of pages
                if (recordsPerPage > 0)
                {
                    pages = recordCount / recordsPerPage;
                    if (recordCount % recordsPerPage > 0)
                    {
                        pages += 1;
                    }
                }

                foreach (KeyValuePair<string, string> pair in queryParams)
                {
                    if(!pageBaseUrl.QueryParameters.ContainsKey(pair.Key))
                    {
                        pageBaseUrl.QueryParameters.Add(pair.Key, pair.Value);
                    }
                }

                if (pages > 1)
                {
                    startIndex = currentPage - showPages > 0 ? currentPage - showPages : 1;
                    endIndex = currentPage + showPages > pages ? pages : currentPage + showPages;
                    string first = "";
                    string page = "";
                    string url = pageBaseUrl.ToString();

                    for (int i = startIndex; i <= endIndex; i++)
                    {
                        if (currentPage != i)
                        {
                            first = (((i - 1) * this.recordsPerPage) + 1).ToString();
                            page = i.ToString();
                            url = GetPagerUrl(pageBaseUrl, first, page);

                            result += "<li><a href=\"" + url.ToString() + "\">" + i.ToString() + "</a></li>";
                            //result += "<li><a href=\"" + String.Format(pageBaseUrlFormat, (((i - 1) * this.recordsPerPage) + 1).ToString(), i) + "\">" + i.ToString() + "</a></li>";
                        }
                        else
                        {
                            result += "<li class='current'>" + i.ToString() + "</li>";
                        }
                    }

                    if (currentPage > 1)
                    {
                        first = (((currentPage - 2) * this.recordsPerPage) + 1).ToString();
                        page = (currentPage - 1).ToString();
                        url = GetPagerUrl(pageBaseUrl, first, page);

                        result = "<li class='previous'><a href=\"" + url.ToString() + "\">Previous</a></li>" + result;

                        //result = "<li class='previous'><a href=\"" + String.Format(pageBaseUrlFormat, (((currentPage - 2) * this.recordsPerPage) + 1).ToString(), (currentPage - 1).ToString()) + "\">Previous</a></li>" + result;
                    }
                    if (currentPage < pages)
                    {
                        first = (((currentPage) * this.recordsPerPage) + 1).ToString();
                        page = (currentPage + 1).ToString();
                        url = GetPagerUrl(pageBaseUrl, first, page);

                        result += "<li class='next'><a href=\"" + url.ToString() + "\">Next</a></li>";

                        //result += "<li class='next'><a href=\"" + String.Format(pageBaseUrlFormat, (((currentPage) * this.recordsPerPage) + 1).ToString(), (currentPage + 1).ToString()) + "\">Next</a></li>";
                    }

                    result = "<div class='pagination'><ul class='no-bullets'>" + result + "</ul></div>";
                }

                return result;
            }

            public string GetPagerUrl(NciUrl basePageUrl, string first, string page)
            {
                NciUrl pagerUrl = basePageUrl;

                if(!pagerUrl.QueryParameters.ContainsKey("first"))
                {
                    pagerUrl.QueryParameters.Add("first", first);
                }
                else
                {
                    pagerUrl.QueryParameters.Remove("first");
                    pagerUrl.QueryParameters.Add("first", first);
                }

                if (!pagerUrl.QueryParameters.ContainsKey("page"))
                {
                    pagerUrl.QueryParameters.Add("page", page);
                }
                else
                {
                    pagerUrl.QueryParameters.Remove("page");
                    pagerUrl.QueryParameters.Add("page", page);
                }

                return pagerUrl.ToString();
            }
        }

        public string SearchStr { get; set; }

        public string SrcGroup { get; set; }

        public bool BContains { get; set; }

        public int NumResults { get; set; }

        /// <summary>
        /// Sets the number of results to show per page.
        /// </summary>
        public int PageSize { get; set; }

        /// <summary>
        /// Which page of the search results
        /// </summary>
        public int CurrentPageIndex { get; set; }

        public string DictionaryURL { get; set; }

        public string DictionaryPrettyURL { get; set; }

        protected void Page_Load(object sender, EventArgs e)
        {
            DictionaryURL = PageAssemblyContext.Current.requestedUrl.ToString();
            DictionaryPrettyURL = this.PageInstruction.GetUrl(PageAssemblyInstructionUrls.PrettyUrl).ToString();

            GetQueryParams();
            SetDoNotIndex();

            //Set display props according to lang
            if (PageAssemblyContext.Current.PageAssemblyInstruction.Language == "es")
            {
                lblResultsFor.Text = "resultados de:";
            }
            else
            {
                lblResultsFor.Text = "results found for:";
            }

            SetupCommon();

            LoadData();

            //set up pager stuff
            string pageHtml = string.Empty;
            if (NumResults > 0 && PageAssemblyContext.Current.DisplayVersion != DisplayVersions.Print)
            {
                Dictionary<string, string> queryParams = GetPageQueryParams();

                NciUrl pagerUrl = new NciUrl();
                pagerUrl.SetUrl(DictionaryURL);

                DrugPager objPager = new DrugPager(pagerUrl, queryParams, CurrentPageIndex, PageSize, 2, NumResults);
                pageHtml = objPager.RenderPager();
            }

            litPager.Text = pageHtml;
        }

        private void LoadData()
        {
            DictionaryAppManager _dictionaryAppManager = new DictionaryAppManager();

            SearchType searchType = SearchType.Begins;
            if (BContains)
                searchType = SearchType.Contains;

            DictionarySearchResultCollection resultCollection = null;

            // Translate page number into offset.
            // One less than the current page number (with sanity check), times the page size
            // translates from a "first page is 1" page number to a "first record is zero" offset into the
            // list of results.
            int offset = ((CurrentPageIndex > 0 ? CurrentPageIndex : 1) - 1) * PageSize;

            if (!String.IsNullOrEmpty(SearchStr)) // SearchString provided, do a term search
            {
                SearchStr = Sanitizer.GetSafeHtmlFragment(SearchStr);
                resultCollection = _dictionaryAppManager.Search(SearchStr, searchType, offset, PageSize, NCI.Web.Dictionary.DictionaryType.drug, PageAssemblyContext.Current.PageAssemblyInstruction.Language);
            }

            if (resultCollection != null && resultCollection.Count() > 0)
            {
                //if there is only 1 record - go directly to definition view
                //check if expand equals "A", as this is the default value
                if (resultCollection.ResultsCount == 1)
                {
                    // Get the first (only) item so we can redirect to it specifically
                    IEnumerator<DictionarySearchResult> itemPtr = resultCollection.GetEnumerator();
                    itemPtr.MoveNext();

                    string urlItem = GetFriendlyName(itemPtr.Current.ID);

                    string itemDefinitionUrl = DictionaryPrettyURL + "/def/" + urlItem;
                    Page.Response.Redirect(itemDefinitionUrl);
                }
                else
                {
                    resultListView.DataSource = resultCollection;
                    resultListView.DataBind();
                    NumResults = resultCollection.ResultsCount;
                    lblWord.Text = SearchStr.Replace("[[]", "[");
                    lblNumResults.Text = NumResults.ToString();
                    if (NumResults == 0)
                    {
                        RenderNoResults();
                    }
                }
            }
            else
            {
                RenderNoResults();
            }
        }

        /// <summary>
        /// Retrieve pipe | separated list of "OtherNameType" values for the drug dictionary.
        /// </summary>
        /// <returns></returns>
        private string GetDrugDictionaryFilter()
        {
            string filter = ConfigurationManager.AppSettings["DrugDictionaryFilter"];
            if (string.IsNullOrEmpty(filter))
                filter = String.Empty;
            else
                filter = filter.Trim();

            return filter;
        }

        /// <summary>
        /// Returns the page's query parameters for use in paging.
        /// </summary>
        private Dictionary<string, string> GetPageQueryParams()
        {
            Dictionary<string, string> queryParams = new Dictionary<string, string>();

            if (!string.IsNullOrEmpty(SearchStr))
            {
                queryParams.Add("q", SearchStr);
                if (BContains)
                    queryParams.Add("contains", "true");
            }

            return queryParams;
        }

        private void SetDoNotIndex()
        {
            PageInstruction.AddFieldFilter("meta_robots", (name, data) =>
            {
                data.Value = "noindex, nofollow";
            });
        }

        private void RenderNoResults()
        {
            //to display EmptyDataTemplate the ListView datasource needs to be set to null
            resultListView.DataSource = new DictionarySearchResultCollection(new DictionarySearchResult[0]);
            resultListView.DataBind();
            numResDiv.Visible = false;
        }

        //update the EmptyDataTemplate label text based on the Dictionary Language
        protected void GetNoDataMessage(object sender, EventArgs e)
        {
            Label lblNoDataMessage = sender as Label;

            if (lblNoDataMessage != null)
            {
                if (PageAssemblyContext.Current.PageAssemblyInstruction.Language == "es")
                    lblNoDataMessage.Text = "No se encontraron resultados para lo que usted busca. Revise si escribi&oacute; correctamente e inténtelo de nuevo. También puede escribir las primeras letras de la palabra o frase que busca o hacer clic en la letra del alfabeto y revisar la lista de términos que empiezan con esa letra.";
                else
                    lblNoDataMessage.Text = "No matches were found for the word or phrase you entered. Please check your spelling, and try searching again. You can also type the first few letters of your word or phrase, or click a letter in the alphabet and browse through the list of terms that begin with that letter.";
            }
        }

        /// <summary>
        /// Setup shared by English and Spanish versions
        /// </summary>
        private void SetupCommon()
        {
            if (!string.IsNullOrEmpty(SrcGroup))
                BContains = Convert.ToBoolean(SrcGroup);

            // Initialize number of results per page.
            string pgSize = ConfigurationManager.AppSettings["DrugDictionaryPageSize"];
            if (string.IsNullOrEmpty(pgSize))
                PageSize = 100;
            else
                PageSize = Int32.Parse(pgSize);
        }

        /// <summary>
        /// Saves the query parameters
        /// </summary>
        private void GetQueryParams()
        {
            SearchStr = Sanitizer.GetSafeHtmlFragment(Request.Params["q"]);
            SearchStr = Strings.Clean(SearchStr);
            SrcGroup = Strings.Clean(Request.Params["contains"]);
            CurrentPageIndex = Strings.ToInt(Request.Params["page"], 1);
        }

        protected string ResultListViewHrefOnclick(ListViewDataItem dataItem)
        {
            if (WebAnalyticsOptions.IsEnabled)
                return "onclick=\"NCIAnalytics.TermsDictionaryResults(this,'" + (dataItem.DataItemIndex + 1).ToString() + "');\""; // Load results onclick script
            else
                return "";
        }

        protected void resultListView_OnItemDataBound(object sender, ListViewItemEventArgs e)
        {
            ListViewDataItem dataItem = (ListViewDataItem)e.Item;

            if (e.Item.ItemType == ListViewItemType.DataItem)
            {
                DictionarySearchResult dictionaryResult = (DictionarySearchResult)dataItem.DataItem;

                if (dictionaryResult != null)
                {
                    PlaceHolder phPronunciation = (PlaceHolder)dataItem.FindControl("phPronunciation");
                    if (dictionaryResult.Term.HasPronunciation && phPronunciation != null)
                    {
                        phPronunciation.Visible = true;
                        HtmlAnchor pronunciationLink = (HtmlAnchor)dataItem.FindControl("pronunciationLink");
                        if (pronunciationLink != null && dictionaryResult.Term.Pronunciation.HasAudio)
                        {
                            pronunciationLink.Visible = true;
                            pronunciationLink.HRef = dictionaryResult.Term.Pronunciation.Audio;
                        }
                        else
                            pronunciationLink.Visible = false;

                        Literal pronunciationKey = (Literal)dataItem.FindControl("pronunciationKey");

                        if (pronunciationKey != null && dictionaryResult.Term.Pronunciation.HasKey)
                            pronunciationKey.Text = " " + dictionaryResult.Term.Pronunciation.Key;

                    }
                    else
                        phPronunciation.Visible = false;
                }
            }
        }

        /// <summary>
        /// Turn a term's list of aliases into a semi-colon separated list.
        /// </summary>
        /// <param name="word">A DictionarySearchResult object containing a Term data structure.</param>
        /// <returns> A semi-colon separated list of term aliases, enclosed in square brackets.
        /// <remarks>
        /// Only returns those aliases which met the search criteria.
        /// Returns an empty string if an Expand search was performed.</remarks>
        /// </returns>
        public string GetTermAliasList(object termSearchResult)
        {
            string aliases = String.Empty;

            DictionarySearchResult termInfo = termSearchResult as DictionarySearchResult;

            if (termInfo != null      // Don't do this unless there are aliases to list.
                && termInfo.Term.Aliases.Length > 0
            )
            {
                StringBuilder sb = new StringBuilder();

                // Get the original search string, forcing it to be non-null and removing any
                // leading/trailing spaces.
                string searchstr = Strings.Clean(Request.Params["q"]) ?? String.Empty;
                searchstr = SearchStr.Trim().ToLower();

                // Handle the possibility that two alias types might have the same value
                // (e.g. US Brand Name and Foreign Brand Name)
                IEnumerable<Alias> uniqueAliases = termInfo.Term.Aliases.Distinct();

                // Roll up the list of terms.
                foreach (Alias alias in uniqueAliases)
                {
                    if (alias.Name != null)
                    {
                        // Mimic legacy logic for choosing which aliases to display.
                        string name = HiLite(alias.Name.Trim());
                        string compareName = alias.Name.Trim().ToLower();

                        // name contains the search string
                        if (BContains && compareName.Contains(searchstr))
                            sb.AppendFormat("{0}; ", name);
                        // name starts with the search string
                        else if (!BContains && compareName.StartsWith(searchstr))
                            sb.AppendFormat("{0}; ", name);
                        // else -- not a match.
                    }
                }

                // If terms were found, trim off the final semicolon and space before
                // wrapping the whole thing in square brackets.
                aliases = sb.ToString();
                if (!String.IsNullOrEmpty(aliases))
                    aliases = String.Format("[ {0} ]", aliases.TrimEnd(' ', ';'));
            }

            return aliases;
        }

        public string GetTermDefinition(object termSearchResult)
        {
            string definition = String.Empty;

            DictionarySearchResult termInfo = termSearchResult as DictionarySearchResult;

            if (termInfo != null)
            {
                definition = termInfo.Term.Definition.Html;
            }

            return definition;
        }

        public string HiLite(object word)
        {
            string marked = string.Empty;

            string original = word.ToString();
            string seq = SearchStr.ToLower();
            marked = original;
            int len = seq.Length;
            string style = "<span class=\"dictionary-partial-match\">";

            if (!BContains)  //only higlight if it begins with sequence 
            {
                if (original.ToLower().StartsWith(seq))
                {
                    marked = marked.Insert(len, "</span>");
                    marked = marked.Insert(0, style);
                }
            }
            else
            {
                int pos = original.ToLower().IndexOf(seq, 0);
                if (pos > -1)
                {
                    marked = marked.Insert(pos + len, "</span>");
                    marked = marked.Insert(pos, style);
                }
            }

            return marked;
        }
    }
}