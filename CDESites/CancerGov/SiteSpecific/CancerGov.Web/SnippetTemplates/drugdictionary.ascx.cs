using System;
using System.Configuration;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using NCI.Text;
using NCI.Web.CDE.UI;
using CancerGov.Text;
using NCI.Web.CDE;
using Www.Common.UserControls;
using NCI.Web.CDE.WebAnalytics;
using CancerGov.CDR.TermDictionary;
using CancerGov.UI;
using NCI.Web.UI.WebControls.FormControls;
using CancerGov.CDR.DrugDictionary;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using NCI.Search;

namespace Www.Templates
{

    public partial class drugdictionary : SnippetControl
    {

        protected AlphaListBox alphaListBox;

        //These are the QueryString related variables
        private string _searchStr = string.Empty;
        private string _expand = string.Empty;
        private string _cdrid = string.Empty;
        private string _srcGroup = string.Empty;
        private int _currentPageIndex = 1;

        private string _pagePrintUrl = string.Empty;
        private string _pageOptionsBoxTitle = string.Empty;
        private string _prevText = string.Empty;
        private string _nextText = string.Empty;
        private string _pageUrl = string.Empty;
        private string _pagerHtml = string.Empty;

        private bool _bContains = false;
        private bool _bOtherNames = false;
        private int _numResults = 0;
        private int _pageSize = 100;
        private string _dictionaryURL = string.Empty;

        #region Page properties

        #region QueryString Props
        //QueryString Props
        public string SearchStr
        {
            get { return _searchStr; }
            set
            {
                if (value != null)
                    _searchStr = value.Trim();
            }
        }
        public string Expand
        {
            get { return _expand; }
            set { if (value != null) _expand = value; }
        }
        public string CdrID
        {
            get { return _cdrid; }
            set { if (value != null) _cdrid = value; }
        }
        public string SrcGroup
        {
            get { return _srcGroup; }
            set { if (value != null) _srcGroup = value; }
        }

        public int CurrentPageIndex
        {
            get { return _currentPageIndex; }
            set { _currentPageIndex = value; }
        }

        #endregion

        public string PageUrl
        {
            get { return _pageUrl; }
            set { _pageUrl = value; }
        }

        public string PagePrintUrl
        {
            get { return _pagePrintUrl; }
            set { _pagePrintUrl = value; }
        }
        public bool BContains
        {
            get { return _bContains; }
            set { _bContains = value; }
        }
        public bool BOtherNames
        {
            get { return _bOtherNames; }
            set { _bOtherNames = value; }
        }
        public int NumResults
        {
            get { return _numResults; }
            set { _numResults = value; }
        }

        public int PageSize
        {
            get { return _pageSize; }
            set { _pageSize = value; }
        }

        public string PagerHtml
        {
            get { return _pagerHtml; }
            set { _pagerHtml = value; }
        }

        #endregion

        public string DictionaryURL
        {
            get { return _dictionaryURL; }
            set { _dictionaryURL = value; }
        }


        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            GetQueryParams();
            DictionaryURL = PageAssemblyContext.Current.requestedUrl.ToString(); //ConfigurationSettings.AppSettings["DrugDictionaryURL"];

            SetupCommon();

            //Set is IE property to determine if browser is IE 
            AutoComplete1.IsIE = (Request.Browser.Browser.ToUpper() == "IE" ? true : false);

            //ConfigControlls();

            if (Page.Request.RequestType.Equals("POST")) //This is a POST(back)
            {
                SearchStr = Request.Params["AutoComplete1"];
                SearchStr = SearchStr.Replace("[", "[[]");
                CdrID = string.Empty;
                Expand = string.Empty;
                CurrentPageIndex = 1;
                BOtherNames = true;

                RadioButton rd = (RadioButton)FindControl("radioContains");

                if (rd.Checked == true)
                    BContains = true;

                if (string.IsNullOrEmpty(SearchStr))
                    ActivateDefaultView();
                else
                    LoadData();
            }
            else // This is a GET
            {
                if ((string.IsNullOrEmpty(CdrID)) && (string.IsNullOrEmpty(Expand)) && (string.IsNullOrEmpty(SearchStr)))
                    ActivateDefaultView();
                else
                    LoadData();
            }

            SetupPageUrl();
            SetupUrlFilters();

            // base URL for the A-Z links
            btnGo.PostBackUrl = DictionaryURL;
            alphaListBox.BaseUrl = DictionaryURL;
            //Page.Form.Action = DictionaryURL;

            lblNumResults.Text = NumResults.ToString();
            lblWord.Text = SearchStr.Replace("[[]", "[");

            ResetControls();

            if (WebAnalyticsOptions.IsEnabled)
            {
                this.PageInstruction.SetWebAnalytics(WebAnalyticsOptions.eVars.PageName, wbField =>
                {
                    string suffix = "";
                    if (Expand != "")
                        suffix = " - AlphaNumericBrowse";
                    else if (CdrID != "")
                        suffix = " - Definition";
                    wbField.Value = ConfigurationSettings.AppSettings["HostName"] + PageAssemblyContext.Current.requestedUrl.ToString() + suffix;
                });

                Page.Form.Attributes.Add("onsubmit", "NCIAnalytics.DrugDictionarySearch(this);"); // Load from onsubit script
                alphaListBox.WebAnalyticsFunction = "NCIAnalytics.DrugDictionarySearchAlphaList"; // Load A-Z list onclick script


            }


            BackTopLink();
            ResultsForText();
            //set up pager stuff
            if (NumResults > 0 && PageAssemblyContext.Current.DisplayVersion != DisplayVersions.Print)
            {
                ResultPager objPager = new ResultPager(DictionaryURL + PageUrl, CurrentPageIndex, PageSize, 2, NumResults);
                PagerHtml = "<p>" + objPager.RenderPager() + "</p>";
            }
            litPager.Text = PagerHtml;


        }
        protected void ResultsForText()
        {
            if (!string.IsNullOrEmpty(Expand))
                numResDiv.Visible = false;
            if (NumResults > 1)
                lblResultsFor.Text = "results found for:";
        }
        protected string ResultListViewHrefOnclick(ListViewDataItem dataItem)
        {
            int indexAdjust = ((PageSize * CurrentPageIndex) - PageSize) + 1;

            if (WebAnalyticsOptions.IsEnabled)
                return "onclick=\"NCIAnalytics.DrugDictionaryResults(this,'" + (dataItem.DataItemIndex + indexAdjust).ToString() + "');\""; // Load results onclick script
            else
                return "";
        }
        protected void BackTopLink()
        {
            //		RawUrl	"/drugdictionary?CdrID=42766"	string

            if (Request.RawUrl.Contains("?") == false && NumResults < 1)
            {

                litBackToTop.Visible = false;
            }
            else if (Request.RawUrl.Contains("?CdrID") == true)
            {
                litBackToTop.Visible = false;
            }
            else
            {
                litBackToTop.Visible = true;
                litBackToTop.Text = "<a href=\"#top\" class=\"backtotop-link\"><img src=\"/images/backtotop_red.gif\" alt=\"Back to Top\" border=\"0\">Back to Top</a>";

            }
        }

        /// <summary>
        /// Saves the quesry parameters to support old gets
        /// </summary>
        private void GetQueryParams()
        {
            Expand = Strings.Clean(Request.Params["expand"]);
            CdrID = Strings.Clean(Request.Params["cdrid"]);
            SearchStr = Strings.Clean(Request.Params["searchTxt"]);
            if (SearchStr.Contains("%"))
                SearchStr = string.Empty;
            SrcGroup = Strings.Clean(Request.Params["sgroup"]);
            CurrentPageIndex = Strings.ToInt(Request.Params["page"], 1);
        }

        #region Setup methods

        private void SetupCommon()
        {
            //litSendToPrinter.Text = "Send to Printer";

            // This sets the url and link text for close
            AutoComplete1.SearchURL = "/DrugDictionary.svc/SearchJSON?searchTerm=";

            radioStarts.Attributes["onclick"] = "toggleSearchMode(event, '" + AutoComplete1.ClientID + "', false)";
            radioContains.Attributes["onclick"] = "toggleSearchMode(event, '" + AutoComplete1.ClientID + "', true)";

            radioStarts.Attributes["onmouseover"] = "keepListBox(event, '" + AutoComplete1.ClientID + "', true)";
            radioStarts.Attributes["onmouseout"] = "keepListBox(event, '" + AutoComplete1.ClientID + "', false)";

            radioContains.Attributes["onmouseover"] = "keepListBox(event, '" + AutoComplete1.ClientID + "', true)";
            radioContains.Attributes["onmouseout"] = "keepListBox(event, '" + AutoComplete1.ClientID + "', false)";

            if (!string.IsNullOrEmpty(SrcGroup))
                BContains = SrcGroup.Equals("Contains");

            if (string.IsNullOrEmpty(Expand))
            {
                BOtherNames = true;
            }
            else
            {
                if (Expand.Trim() == "#")
                {
                    SearchStr = "[^a-zA-Z]";
                }
                else if (Expand.Trim() == "All")
                {
                    SearchStr = "%";
                }
                else
                {
                    SearchStr = Expand.Trim().ToUpper();
                }
            }

            if (PageAssemblyContext.Current.DisplayVersion == DisplayVersions.Print)
            {
                pnlDrugSearch2.Visible = false;
                //pnlSendToPrinter.Visible = true;
            }
            else
            {
                alphaListBox.TextOnly = (PageAssemblyContext.Current.DisplayVersion == DisplayVersions.Web) ? true : false;
                alphaListBox.Title = string.Empty;

                //if (PageAssemblyContext.Current.DisplayVersion == DisplayVersions.Image)
                //    searchboxContainer.Attributes.Add("style", "width:296px");
                //else
                //    searchboxContainer.Attributes.Add("style", "width:289px");

            }


            string pgSize = ConfigurationSettings.AppSettings["DrugDictionaryPageSize"];
            if (!string.IsNullOrEmpty(pgSize))
                PageSize = Int32.Parse(pgSize);

        }
        #endregion

        #region Data-related
        /// <summary>
        /// Handles calls to the Management layer to get data
        /// </summary>
        private void LoadData()
        {
            if (!string.IsNullOrEmpty(CdrID)) //this is a cdrid lookup for one term
            {
                DrugDictionaryDataItem dataItem = DrugDictionaryManager.GetDefinitionByTermID(Int32.Parse(CdrID));
                //DrugDictionaryManager.GetTermNeighbors(dataItem, 5);
                if (dataItem != null)
                {
                    NumResults = 1;
                    ActivateDefinitionView(dataItem);

                    // Web Analytics *************************************************
                    if (WebAnalyticsOptions.IsEnabled)
                        this.PageInstruction.SetWebAnalytics(WebAnalyticsOptions.Events.DrugDictionaryView, wbField =>
                        {
                            wbField.Value = null;
                        });

                }
                else
                {
                    ActivateDefaultView();
                }
            }
            else //This is a search
            {
                DrugDictionaryCollection dataCollection = dataCollection = DrugDictionaryManager.Search(SearchStr, PageSize, CurrentPageIndex, BOtherNames, BContains);
                if (dataCollection.Count == 1) //if there is only 1 record - go directly to definition view
                {
                    Page.Response.Redirect(DictionaryURL + "?CdrID=" + dataCollection[0].TermID.ToString(), true);
                }
                else
                {
                    resultListView.DataSource = dataCollection;
                    resultListView.DataBind();
                    NumResults = dataCollection.matchCount;
                    ActivateResultsListView(dataCollection);
                }
            }
        }
        #endregion


        #region View activation methods - corresponding to the multiview on the page

        private void ActivateDefaultView()
        {
            MultiView1.ActiveViewIndex = 0;
        }

        private void ActivateResultsListView(DrugDictionaryCollection col)
        {
            MultiView1.ActiveViewIndex = 1;
            litBackToTop.Visible = NumResults > 0;
        }

        private void ActivateDefinitionView(DrugDictionaryDataItem dataItem)
        {
            MultiView1.ActiveViewIndex = 2;
            //pnlPrevNext.Visible = true;
            RenderDefinition(dataItem);
        }

        #endregion

        #region Rendering methods

        private void RenderDefinition(DrugDictionaryDataItem dataItem)
        {
            StringBuilder htmlText = new StringBuilder();
            string strMetaText = string.Empty;

            string width = "100%"; //for non-print width
            if (PageAssemblyContext.Current.DisplayVersion == DisplayVersions.Print)
            {
                width = "100%"; //for print width
            }

            if (dataItem.DisplayNames.Count > 0)
            {
                htmlText.Append("<table width=\"" + width + "\" cellspacing=\"0\" cellpadding=\"0\" border=\"0\">");

                foreach (KeyValuePair<string, List<string>> displayPair in dataItem.DisplayNames)
                {
                    StringBuilder syn = new StringBuilder();
                    string displayName = displayPair.Key;

                    if (displayPair.Value.Count > 1)
                        displayName += "s";

                    foreach (string otherName in displayPair.Value)
                    {
                        bool chemStruct = displayName.Trim().ToLower().StartsWith("chemical structure name");
                        if (chemStruct)
                        {
                            syn.Append("<li style=\"margin-left:-25px;\">");
                        }
                        syn.Append(otherName);
                        if (displayPair.Value.Count > 1 && !chemStruct)
                        {
                            syn.Append("<br />");
                        }

                        if (displayName.Trim().ToLower().StartsWith("us brand name"))
                        {
                            strMetaText += ", " + otherName;
                        }
                    }
                    AppendOtherNameHtml(htmlText, displayName, syn, displayPair.Value.Count);
                }

                htmlText.Append("</table>");
            }

            if (!string.IsNullOrEmpty(dataItem.PreferredName))
            {
                PageAssemblyContext.Current.PageAssemblyInstruction.AddFieldFilter("short_title", (name, data) =>
                {
                    data.Value = "Definition of " + dataItem.PreferredName + " - National Cancer Institute Drug Dictionary";
                });

                this.Page.Header.Title = PageAssemblyContext.Current.PageAssemblyInstruction.GetField("short_title");

                PageAssemblyContext.Current.PageAssemblyInstruction.AddFieldFilter("meta_description", (name, data) =>
                {
                    data.Value = "definition, " + dataItem.PreferredName + strMetaText;
                });


                PageAssemblyContext.Current.PageAssemblyInstruction.AddFieldFilter("meta_keywords", (name, data) =>
                {
                    data.Value = strMetaText;

                });

            }

            lblTermName.Text = dataItem.PreferredName;
            if (!string.IsNullOrEmpty(dataItem.PrettyURL))
            {
                //ibtnPatientInfo.PostBackUrl = dataItem.PrettyURL;
                //ibtnPatientInfo.Visible = true;
                hlPatientInfo.NavigateUrl = dataItem.PrettyURL;
                hlPatientInfo.Visible = true;
            }

            litDefHtml.Text = dataItem.DefinitionHTML;
            litOtherNames.Text = htmlText.ToString();

            //RenderPrevNextTerms(dataItem);
        }

        /// <summary>
        /// Renders a table that contains Other names for the drug, right under the definition
        /// </summary>
        /// <param name="htmlT"></param>
        /// <param name="dispName"></param>
        /// <param name="syn"></param>
        /// <param name="synCnt"></param>
        private void AppendOtherNameHtml(StringBuilder htmlT, string dispName, StringBuilder syn, int synCnt)
        {
            htmlT.Append("<tr><td valign=\"top\" width=\"28%\"><b>");
            htmlT.Append(dispName);
            htmlT.Append(":</b></td>");
            htmlT.Append("<td valign=\"top\" width=\"10\"><img src=\"/images/spacer.gif\" width=\"10\" height=\"1\"  border=\"0\" alt=\"\"></td>");

            htmlT.Append("<td valign=\"top\" width=\"68%\">");
            if (dispName.Trim().ToLower().StartsWith("chemical structure name"))
            {
                if (synCnt > 1)
                {
                    htmlT.Append("<ul style=\"margin-bottom:0px;margin-top:0px;\">");
                    htmlT.Append(syn);
                    htmlT.Append("</ul>");
                }
                else
                {
                    htmlT.Append(Regex.Replace(syn.ToString(), "<li.*>", ""));
                }
            }
            else
            {
                htmlT.Append(syn);
            }
            htmlT.Append("</td> ");

            htmlT.Append("<td valign=\"top\" width=\"10\"></td>");
            //htmlT.Append("<tr> ");
            htmlT.Append("<tr><td valign=\"top\" colspan=\"4\"><img src=\"/images/spacer.gif\" width=\"10\" height=\"6\"  border=\"0\" alt=\"\"></td></tr>");
        }

        #endregion


        /// <summary>
        /// In case we had a GET, we want to mimic old behaviour and set the controls
        /// to positions corresponding to query parameters
        /// </summary>
        private void ResetControls()
        {
            radioContains.Checked = BContains;
            AutoComplete1.Text = (string.IsNullOrEmpty(Expand)) ? SearchStr.Replace("[[]", "[") : string.Empty;
            AutoComplete1.SearchCriteria = (BContains) ? AutoComplete.SearchCriteriaEnum.Contains : AutoComplete.SearchCriteriaEnum.BeginsWith;
        }

        #region Utility methods
        public string AddBrackets(object word)
        {
            string bracketed = string.Empty;
            if (word != null && string.IsNullOrEmpty(Expand))
            {
                string original = word.ToString();
                string[] pieces = original.Split(';');
                foreach (string piece in pieces)
                {
                    bracketed += HiLite(piece.Trim()) + "; ";
                }
                bracketed = bracketed.TrimEnd(' ', ';');
                bracketed = "[ " + bracketed + " ]";
            }

            return bracketed;
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

        private void SetupPageUrl()
        {
            PagePrintUrl = "?print=1";
            PageUrl = "/?";
            //add expand
            if (!string.IsNullOrEmpty(Expand))
            {
                if (Expand.Trim() == "#")
                {
                    PagePrintUrl += "&expand=%23";
                    PageUrl += "&expand=%23";
                }
                else
                {
                    PagePrintUrl += "&expand=" + Expand.Trim().ToUpper();
                    PageUrl += "&expand=" + Expand.Trim();
                }
            }

            //Language stuff
            //PagePrintUrl += QueryStringLang;

            //add cdrid or searchstr
            if (!string.IsNullOrEmpty(CdrID))
            {
                PagePrintUrl += "&cdrid=" + CdrID;
                PageUrl += "&cdrid=" + CdrID;
            }
            else if (!string.IsNullOrEmpty(SearchStr))
            {
                PagePrintUrl += "&searchTxt=" + SearchStr;
                PageUrl += "&searchTxt=" + SearchStr;
                if (BContains)
                {
                    PagePrintUrl += "&sgroup=Contains";
                    PageUrl += "&sgroup=Contains";
                }
            }

        }

        private void SetupUrlFilters()
        {
            PageAssemblyContext.Current.PageAssemblyInstruction.AddUrlFilter("Print", (name, url) =>
            {
                url.SetUrl(PageAssemblyContext.Current.PageAssemblyInstruction.GetUrl("CurrentURL").ToString() + "/" + PagePrintUrl);
            });

            PageAssemblyContext.Current.PageAssemblyInstruction.AddUrlFilter(PageAssemblyInstructionUrls.CanonicalUrl, (name, url) =>
            {
                if (CdrID != "")
                    url.SetUrl(url.ToString() + "?cdrid=" + CdrID);
                else if (Expand != "")
                    url.SetUrl(url.ToString() + "?expland=" + Expand);
                else
                    url.SetUrl(url.ToString());
            });

        }
        #endregion
    }
}