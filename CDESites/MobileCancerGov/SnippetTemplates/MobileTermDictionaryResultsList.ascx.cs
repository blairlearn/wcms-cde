using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;
using NCI.Web.CDE.UI;
using NCI.Web.CDE.WebAnalytics;
using CancerGov.Text;
using CancerGov.Common;
using CancerGov.CDR.TermDictionary;
using MobileCancerGov.Web.SnippetTemplates;
using NCI.Web.CDE.UI.SnippetControls;


namespace MobileCancerGov.Web.SnippetTemplates
{
    public partial class MobileTermDictionaryResultsList : SnippetControl
    {
        private const string ENGLISH = "english";
        private const string SPANISH = "spanish";

        private string _dictionaryURL = "";
        private string _queryStringLanguage = "English";
        private string _searchStr = "";
        private int _numResults = 0;
        private string _language = "";
        private string _page = "";
        private string _previousPagerOnclick = "";
        private string _nextPagerOnclick = "";
        private string _pagerInfo = "";

        //Properties 
        public string DictionaryURL
        {
            get { return _dictionaryURL; }
            set { _dictionaryURL = value; }
        }
        public int NumResults
        {
            get { return _numResults; }
            set { _numResults = value; }
        }
        public string QueryStringLang
        {
            get { return _queryStringLanguage; }
            set { _queryStringLanguage = value; }
        }
        public string SearchString
        {
            get { return _searchStr; }
            set { _searchStr = value; }
        }
        public string Language
        {
            get { return _searchStr; }
            set { _searchStr = value; }
        }
        public string NextPagerOnclick
        {
            get { return _nextPagerOnclick; }
        }
        public string PreviousPagerOnclick
        {
            get { return _previousPagerOnclick; }
        }
        public string PagerInfo
        {
            get { return _pagerInfo; }
        }


        protected void Page_Load(object sender, EventArgs e)
        {

            String searchStr = Strings.Clean(Request.QueryString["search"]);
            String expand = Strings.Clean(Request.QueryString["expand"]);
            _language = Strings.Clean(Request.QueryString["language"]);
            _page = Strings.Clean(Request.QueryString["page"]);
            
            if (!String.IsNullOrEmpty(searchStr))
                _searchStr = "&lastSearch=" + searchStr;
            else
                _searchStr = "";

            litPageUrl.Text = MobileTermDictionary.RawUrlClean(Page.Request.RawUrl);
            litSearchBlock.Text = MobileTermDictionary.SearchBlock(MobileTermDictionary.RawUrlClean(Page.Request.RawUrl), searchStr);
            
            bool expandNumbers = false;
            _dictionaryURL = MobileTermDictionary.RawUrlClean(Page.Request.RawUrl);
            TermDictionaryCollection dataCollection = null;
            if (!String.IsNullOrEmpty(searchStr)) // search string provide, do a term search
            {
                dataCollection = TermDictionaryManager.Search("English", searchStr, 0, false);
            }
            else if(!String.IsNullOrEmpty(expand)) // A-Z expand provided - do an A-Z search
            {
                string unFixedExpand = expand;
                if (expand == "#")
                {
                    expandNumbers = true;
                    expand = "[0-9]";
                    unFixedExpand = "%23";
                }

                int rowsPerPage = 5;
                int page = 1;
                
                if (!String.IsNullOrEmpty(_page))
                    page = Convert.ToInt32(_page);
                int prePage = 1;
                
                if(page >= 2)
                    prePage = page - 1;
                
                int nextPage = page + 1;

                //dataCollection = TermDictionaryManager.Search("English", expand.Trim().ToUpper(), 0, false);
                pnlPager.Visible = true;
                int maxrows = 0;
                dataCollection = TermDictionaryManager.GetTermDictionaryList("English", expand.Trim().ToUpper(), true, rowsPerPage, page, ref maxrows);
                int maxpages = maxrows / rowsPerPage;

                _nextPagerOnclick = MobileTermDictionary.RawUrlClean(Page.Request.RawUrl) + "?expand=" + unFixedExpand + "&page=" + nextPage.ToString();
                _previousPagerOnclick = MobileTermDictionary.RawUrlClean(Page.Request.RawUrl) + "?expand=" + unFixedExpand + "&page=" + prePage.ToString();
                _pagerInfo = "Page " + page + " of " + maxpages;

            }

            if (dataCollection != null)
            {
                if (String.IsNullOrEmpty(_language))
                    _language = ENGLISH; //default to English

                if (dataCollection.Count == 1) //if there is only 1 record - go directly to definition view
                {
                }
                else
                {
                    if (expandNumbers)
                    {
                        // Expand # displays results without decription
                        resultListViewNoDescription.Visible = true;
                        resultListView.Visible = false;
                        resultListViewNoDescription.DataSource = dataCollection;
                        resultListViewNoDescription.DataBind();
                    }
                    else
                    {
                        resultListView.Visible = true;
                        resultListViewNoDescription.Visible = false;
                        resultListView.DataSource = dataCollection;
                        resultListView.DataBind();
                    }
                }

                ControlCollection cc = null;
                if (expandNumbers)
                {
                    cc = resultListViewNoDescription.Controls[0].Controls;                    
                    if (_language == SPANISH)
                    {
                        cc[3].Visible = true;
                        cc[1].Visible = false;
                    }
                    else
                    {
                        cc[1].Visible = true;
                        cc[3].Visible = false;
                    }
                }
                else
                {
                    cc = resultListView.Controls[0].Controls;
                    if (_language == SPANISH)
                    {
                        cc[3].Visible = true;
                        cc[1].Visible = false;
                    }
                    else
                    {
                        cc[1].Visible = true;
                        cc[3].Visible = false;
                    }
                }

            }
        }

        protected string ResultListViewHrefOnclick(ListViewDataItem dataItem)
        {
            if (WebAnalyticsOptions.IsEnabled)
                return "onclick=\"NCIAnalytics.TermsDictionaryResults(this,'" + (dataItem.DataItemIndex + 1).ToString() + "');\""; // Load results onclick script
            else
                return "";
        }

        protected string LimitText(ListViewDataItem item, int numberOfCharacters)
        {
            const int MAX_WALKBACK = 30;
            string definition = DataBinder.Eval(item.DataItem, "DefinitionHTML").ToString();

            if (definition.Length > numberOfCharacters)
            {
                if (definition.Substring(numberOfCharacters - 1, 1) == " ")
                    definition = definition.Substring(0, numberOfCharacters - 1) + "...";
                else //walk back to next space
                {
                    int i;
                    for (i = numberOfCharacters; i > 0; i--)
                    {
                        if(i == MAX_WALKBACK)
                        {
                            i=0;
                            break;
                        }
                        if (definition.Substring(i, 1) == " ")
                            break;
                    }
                    if (i <= 0)
                        definition = definition.Substring(0, numberOfCharacters - 1) + "...";
                    else
                        definition = definition.Substring(0, i) + "...";
                }
            }

            return definition;
        }

    }
}