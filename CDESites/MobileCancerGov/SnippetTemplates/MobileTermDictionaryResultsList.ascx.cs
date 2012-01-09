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
using NCI.Web.UI.WebControls;
using NCI.Web.CDE;


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
        private string _languageParam = "";
        private string _language = "";
        
        private string _previousPagerOnclick = "";
        private string _nextPagerOnclick = "";
        private string _pagerInfo = "";
        private int _rowsPerPage = 10;
        private int _maxrows = 0;


        private int _currentPage = 0;
        private int _recordsPerPage = 0;
        private int _offSet = 0;
        private string _pageTitle = "";
        private string _buttonText = "";


 

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
            get { return _language; }
            set { _language = value; }
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
            _languageParam = Strings.Clean(Request.QueryString["language"]);
            //if (_languageParam == null)
            //    _languageParam = "spanish";
            _currentPage = Strings.ToInt(Request.Params["PageNum"], 1);
            _recordsPerPage = Strings.ToInt(Request.Params["RecordsPerPage"], 10);
            _offSet = Strings.ToInt(Request.Params["OffSet"], 0);
            _dictionaryURL = MobileTermDictionary.RawUrlClean(Page.Request.RawUrl);


            string pageTitle;
            string buttonText;
            string language;
            MobileTermDictionary.DetermineLanguage(_languageParam, out language, out pageTitle, out buttonText);
            _language = language;
            litSearchBlock.Text = MobileTermDictionary.SearchBlock(MobileTermDictionary.RawUrlClean(Page.Request.RawUrl), searchStr, pageTitle, buttonText);
            
            litPageUrl.Text = MobileTermDictionary.RawUrlClean(Page.Request.RawUrl);
            
            bool noDefinition = false;
            TermDictionaryCollection dataCollection = null;
            if (!String.IsNullOrEmpty(searchStr)) // search string provide, do a term search
            {
                dataCollection = TermDictionaryManager.Search(language, searchStr, 0, false);
            }
            else if(!String.IsNullOrEmpty(expand)) // A-Z expand provided - do an A-Z search
            {
                noDefinition = true;
                string unFixedExpand = expand;
                if (expand == "#")
                {
                    expand = "[0-9]";
                    unFixedExpand = "%23";
                }

                dataCollection = TermDictionaryManager.GetTermDictionaryList(language, expand.Trim().ToUpper(), false, _rowsPerPage, _currentPage, ref _maxrows);
                int maxpages = _maxrows / _rowsPerPage;

            }

            if (dataCollection != null)
            {
                if (dataCollection.Count == 1) //if there is only 1 record - go directly to definition view
                {

                }
                else
                {
                    if (noDefinition)
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


                int startRecord = 0;
                int endRecord = 0;
                SimplePager.GetFirstItemLastItem(_currentPage, _rowsPerPage, _maxrows, out startRecord, out endRecord);

                spPager.RecordCount = _maxrows;
                spPager.RecordsPerPage = _rowsPerPage;
                spPager.CurrentPage = _currentPage;
                if(expand !="")
                    spPager.BaseUrl = litPageUrl.Text + "?expand=" + expand;
                else
                    spPager.BaseUrl = litPageUrl.Text + "?search=" + searchStr;


                ControlCollection cc = null;
                if (noDefinition)
                {
                    cc = resultListViewNoDescription.Controls[0].Controls;                    
                    if (language == SPANISH)
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
                    if (language == SPANISH)
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