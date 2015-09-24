using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

using CancerGov.CDR.TermDictionary;
using NCI.Util;
using NCI.Web.CDE;
using NCI.Web.CDE.UI;
using NCI.Web.CDE.WebAnalytics;
using NCI.Web.Dictionary;
using NCI.Web.Dictionary.BusinessObjects;
using System.Text;

namespace CancerGov.Web.SnippetTemplates
{
    public partial class DrugDictionaryResultsList : SnippetControl
    {
        protected class DrugPager
        {
            private int showPages = 10;
            private int currentPage = 0;
            private int recordsPerPage = 10;
            private int recordCount = 0;
            private string pageBaseUrlFormat = "javascript:page('{0}');";

            #region Properties

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

            public DrugPager(string pageBaseUrl, int pageIndex, int pageSize, int pageCount, int itemCount)
            {
                this.currentPage = pageIndex;
                this.recordCount = itemCount;
                this.recordsPerPage = pageSize;
                this.showPages = pageCount;
                this.pageBaseUrlFormat = pageBaseUrl + "&first={0}&page={1}";
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

                if (pages > 1)
                {
                    startIndex = currentPage - showPages > 0 ? currentPage - showPages : 1;
                    endIndex = currentPage + showPages > pages ? pages : currentPage + showPages;

                    for (int i = startIndex; i <= endIndex; i++)
                    {
                        if (currentPage != i)
                        {
                            result += "<li><a href=\"" + String.Format(pageBaseUrlFormat, (((i - 1) * this.recordsPerPage) + 1).ToString(), i) + "\">" + i.ToString() + "</a></li>";
                        }
                        else
                        {
                            result += "<li class='current'>" + i.ToString() + "</li>";
                        }
                    }

                    if (currentPage > 1)
                    {
                        result = "<li class='previous'><a href=\"" + String.Format(pageBaseUrlFormat, (((currentPage - 2) * this.recordsPerPage) + 1).ToString(), (currentPage - 1).ToString()) + "\">Previous</a></li>" + result;
                    }
                    if (currentPage < pages)
                    {
                        result += "<li class='next'><a href=\"" + String.Format(pageBaseUrlFormat, (((currentPage) * this.recordsPerPage) + 1).ToString(), (currentPage + 1).ToString()) + "\">Next</a></li>";
                    }

                    result = "<div class='pagination'><ul class='no-bullets'>" + result + "</ul></div>";
                }

                return result;
            }
        }

        
        public string SearchStr{ get; set; }

        public string Expand { get; set; }

        public string CdrID { get; set; }

        public string SrcGroup { get; set; }

        public bool BContains { get; set; }

        public int NumResults { get; set; }

        /// <summary>
        /// Sets the number of results to show per page.
        /// </summary>
        public int PageSize{get;set;}

        /// <summary>
        /// Which page of the search results
        /// </summary>
        public int CurrentPageIndex { get; set; }

        public string DictionaryURL { get; set; }

        protected void Page_Load(object sender, EventArgs e)
        {
            DictionaryURL = PageAssemblyContext.Current.requestedUrl.ToString();

            //base.OnLoad(e);
            GetQueryParams();
            ValidateParams();
            
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
                string pageURL = GetPageUrl();

                DrugPager objPager = new DrugPager(DictionaryURL + pageURL, CurrentPageIndex, PageSize, 2, NumResults);
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
                resultCollection = _dictionaryAppManager.Search(SearchStr, searchType, offset, PageSize, NCI.Web.Dictionary.DictionaryType.drug, PageAssemblyContext.Current.PageAssemblyInstruction.Language);
            }
            else if (!String.IsNullOrEmpty(Expand)) // A-Z expand provided - do an A-Z search
            {
                if (Expand.ToLower() == "all")
                    resultCollection = _dictionaryAppManager.Expand("%", "", offset, PageSize, NCI.Web.Dictionary.DictionaryType.drug, PageAssemblyContext.Current.PageAssemblyInstruction.Language, "v1");
                else
                    resultCollection = _dictionaryAppManager.Expand(Expand, "", offset, PageSize, NCI.Web.Dictionary.DictionaryType.drug, PageAssemblyContext.Current.PageAssemblyInstruction.Language, "v1");

                // Other places in the code expect to find the string that was searched for in SearchStr,
                // but if it was set prior to this point in the code, the wrong search would be performed.
                // This is a result of the hacky way variables are being used to both store search data,
                // and as a flag for which sort of search is supposed to be performed.
                SearchStr = Expand;
            }

            if (resultCollection != null && resultCollection.Count() > 0)
            {
                //if there is only 1 record - go directly to definition view
                if ((resultCollection.ResultsCount == 1) && string.IsNullOrEmpty(Expand))
                {
                    // Get the first (only) item so we can redirect to it specifically
                    IEnumerator<DictionarySearchResult> itemPtr = resultCollection.GetEnumerator();
                    itemPtr.MoveNext();

                    string itemDefinitionUrl = DictionaryURL + "?cdrid=" + itemPtr.Current.ID;
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
        /// Recreates the page's query string parameters for use in paging.
        /// </summary>
        /// <returns>A query string, starting with "/?", containing the parameters and values used
        /// to create the page.</returns>
        private string GetPageUrl()
        {
            string url;

            // URL base.
            url = "/?";

            //add expand
            if (!string.IsNullOrEmpty(Expand))
            {
                if (Expand.Trim() == "#")
                    url += "&expand=%23";
                else
                    url += "&expand=" + Expand.Trim();
            }

            //add cdrid or searchstr
            if (!string.IsNullOrEmpty(CdrID))
            {
                url += "&cdrid=" + CdrID;
            }
            else if (!string.IsNullOrEmpty(SearchStr))
            {
                url += "&searchTxt=" + SearchStr;
                if (BContains)
                    url += "&sgroup=Contains";
            }

            return url;
        }

        private void RenderNoResults()
        {
            //to display EmptyDataTemplate the ListView datasource needs to be set to null
            resultListView.DataSource = null;
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

            if (!string.IsNullOrEmpty(Expand))
            {
                if (Expand.Trim() == "#")
                {
                    Expand = "[0-9]";
                }
                else
                {
                    Expand = Expand.Trim().ToUpper();
                }
            }

            // Initialize number of results per page.
            string pgSize = ConfigurationSettings.AppSettings["DrugDictionaryPageSize"];
            if (string.IsNullOrEmpty(pgSize))
                PageSize = 100;
            else
                PageSize = Int32.Parse(pgSize);
        }

        private void ValidateParams()
        {
            CdrID = Strings.Clean(Request.Params["cdrid"]);
            if (!string.IsNullOrEmpty(CdrID))
            {
                try
                {
                    Int32.Parse(CdrID);
                }
                catch (Exception)
                {
                    throw new Exception("Invalid CDRID" + CdrID);

                }
            }
        }

        /// <summary>
        /// Saves the quesry parameters to support old gets
        /// </summary>
        private void GetQueryParams()
        {
            Expand = Strings.Clean(Request.Params["expand"]);
            CdrID = Strings.Clean(Request.Params["cdrid"]);
            SearchStr = Strings.Clean(Request.Params["search"]);
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

            if (string.IsNullOrEmpty(Expand) // Only do this if we're not responding to an "Expand" click.
                    && termInfo != null      // Don't do this unless there are aliases to list.
                    && termInfo.Term.Aliases.Length > 0
                )
            {
                StringBuilder sb = new StringBuilder();

                // Get the original search string, forcing it to be non-null and removing any
                // leading/trailing spaces.
                string searchstr = Strings.Clean(Request.Params["search"]) ?? String.Empty;
                searchstr = SearchStr.Trim().ToLower();

                // Roll up the list of terms.
                foreach (Alias alias in termInfo.Term.Aliases)
                {
                    if (alias.Name != null)
                    {
                        // Mimic legacy logic for choosing which aliases to display.
                        string name = HiLite(alias.Name.Trim());
                        string compareName = alias.Name.Trim().ToLower();

                        // name contains the search string
                        if ( BContains && compareName.Contains(searchstr))
                            sb.AppendFormat("{0}; ", name);
                        // name starts with the search string
                        else if( !BContains && compareName.StartsWith(searchstr))
                            sb.AppendFormat("{0}; ", name);
                        // else -- not a match.
                    }
                }

                // If terms were found, trim off the final semicolon and space before
                // wrapping the whole thing in square brackets.
                aliases = sb.ToString();
                if(!String.IsNullOrEmpty(aliases))
                    aliases = String.Format("[ {0} ]", aliases.TrimEnd(' ', ';'));
            }

            return aliases;
        }

        public string HiLite(object word)
        {
            string marked = string.Empty;

            string original = word.ToString();
            string seq = SearchStr.ToLower();
            marked = original;
            int len = seq.Length;
            string style = "<span class=\"dictionary-partial-match\">";

            if (string.IsNullOrEmpty(Expand))
            {
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
            }
            return marked;
        }

    }

    
}