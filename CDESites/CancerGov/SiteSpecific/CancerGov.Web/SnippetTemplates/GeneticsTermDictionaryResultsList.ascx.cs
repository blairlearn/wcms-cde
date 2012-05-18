using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Linq;
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
using CancerGov.Web.SnippetTemplates;
using NCI.Web.CDE.UI.SnippetControls;
using NCI.Web.UI.WebControls;
using NCI.Web.CDE;
using NCI.Web;

namespace CancerGov.Web.SnippetTemplates
{
    public partial class GenerticsTermDictionaryResultsList : SnippetControl
    {
        // Query parameter values
        private string _searchStr = "";
        private int _currentPage = 0;
        private int _recordsPerPage = 0;
        private int _offSet = 0;
        private string _dictionaryURL = "";
        private string _version = "";
        private string _term = "";

        // Property variables 
        private string _language = "";
        private bool _showDefinition = true;
        private bool _expand = false;
        private string _expandText = "";
        private int _results = 0;

        //Properties 
        public string DictionaryURL
        {
            get { return _dictionaryURL; }
            set { _dictionaryURL = value; }
        }
        public string SearchString
        {
            get { return _searchStr; }
            set { _searchStr = value; }
        }
        public string Term
        {
            get { return _term; }
            set { _term = value; }
        }
        public string Language
        {
            get { return _language; }
            set { _language = value; }
        }
        public string Version
        {
            get { return _version; }
            set { _version = value; }
        }
        public bool ShowDefinition
        {
            get { return _showDefinition; }
        }
        public bool Expand
        {
            get { return _expand; }
        }
        public string ExpandText
        {
            get { return _expandText; }
        }
        public int Results
        {
            get { return _results; }
        }
        public bool IsSpanish
        {
            get { return (Language == GeneticsTermDictionaryHelper.SPANISH); }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            // Dictionary query parameters
            string expandParam = Strings.Clean(Request.QueryString["expand"]);
            //string languageParam = Strings.Clean(Request.QueryString["language"]);
            string languageParam = ""; //disable language selection by query parameter 
            _searchStr = Strings.Clean(Request.QueryString["search"]);
            _term = Strings.Clean(Request.QueryString["term"]);
            _version = Strings.Clean(Request.QueryString["version"]);

            if (!String.IsNullOrEmpty(_searchStr))
                _searchStr = _searchStr.Replace("[", "[[]");

            // Pager query parameter variables
            _currentPage = Strings.ToInt(Request.Params["PageNum"], 1);
            _recordsPerPage = Strings.ToInt(Request.Params["RecordsPerPage"], 10);
            _offSet = Strings.ToInt(Request.Params["OffSet"], 0);

            //Determine Language - set language related values
            string pageTitle;
            string buttonText;
            string language;
            string reDirect;
            GeneticsTermDictionaryHelper.DetermineLanguage(languageParam, out language, out pageTitle, out buttonText, out reDirect);
            _language = language;


//Commented out for testing 
            //_dictionaryURL = PageAssemblyContext.Current.PageAssemblyInstruction.GetUrl("CurrentUrl").ToString();

            // Setup Pager variables  
            int pager_StartRecord = 0;
            int pager_EndRecord = 0;
            int pager_MaxRows = 0;
            int pager_RowsPerPage = 0;

            TermDictionaryCollection _dc = null;
            if (!String.IsNullOrEmpty(SearchString)) // SearchString provided, do a term search
            {
//Commented out for Testing 
                //PageAssemblyContext.Current.PageAssemblyInstruction.AddUrlFilter(PageAssemblyInstructionUrls.AltLanguage, (name, url) =>
                //{
                //    url.QueryParameters.Add("search", SearchString);
                //});
                //PageAssemblyContext.Current.PageAssemblyInstruction.AddUrlFilter("CurrentUrl", (name, url) =>
                //{
                //    url.QueryParameters.Add("search", SearchString);
                //});
                
                //_dc = TermDictionaryManager.GetTermDictionaryList(language, SearchString, false, pager_RowsPerPage, _currentPage, ref pager_MaxRows);
                _dc = TermDictionaryManager.Search(language, SearchString, 0, false);

            }
            else if (!String.IsNullOrEmpty(Term))
            {
                NCI.Web.CDE.DisplayLanguage dl;
                if (Language.ToLower().Trim() == "spanish")
                    dl = NCI.Web.CDE.DisplayLanguage.Spanish;
                else
                    dl = NCI.Web.CDE.DisplayLanguage.English;

                TermDictionaryDataItem di = TermDictionaryManager.GetDefinitionByTermName(dl, Term, Version, 2);
                string itemDefinitionUrl = DictionaryURL + "?cdrid=" + di.GlossaryTermID;
                Page.Response.Redirect(itemDefinitionUrl);

            }
            else if (!String.IsNullOrEmpty(expandParam)) // A-Z expand provided - do an A-Z search
            {
                _showDefinition = true;
                _expand = true;
                _expandText = expandParam;
                _searchStr = _expandText;
                if (_expandText == "#")
                {
                    _expandText = "[0-9]";
                }
                
//Commented out for Testing 
                //PageAssemblyContext.Current.PageAssemblyInstruction.AddUrlFilter(PageAssemblyInstructionUrls.AltLanguage, (name, url) =>
                //{
                //    url.QueryParameters.Add("expand", _expandText);
                //});
                //PageAssemblyContext.Current.PageAssemblyInstruction.AddUrlFilter("CurrentUrl", (name, url) =>
                //{
                //    url.QueryParameters.Add("expand", _expandText);
                //});

                //_dc = TermDictionaryManager.GetTermDictionaryList(language, _expandText.Trim().ToUpper(), false, pager_RowsPerPage, _currentPage, ref pager_MaxRows);
                _dc = TermDictionaryManager.Search(language,_expandText.Trim().ToUpper(),0,false);
            
            }

            if (_dc != null)
            {
                if (_dc.Count == 1 && !Expand) //if there is only 1 record - go directly to definition view
                {
                    //string itemDefinitionUrl = DictionaryURL + "?cdrid=" + _dc[0].GlossaryTermID + "&language=" + Language;
                    string itemDefinitionUrl = DictionaryURL + "?cdrid=" + _dc[0].GlossaryTermID;
                    Page.Response.Redirect(itemDefinitionUrl);
                }
                else // bind results 
                {
                    _results = _dc.Count;
                    resultListView.DataSource = _dc;
                    resultListView.DataBind();
                }

//Commented out for Testing 
                //SimplePager.GetFirstItemLastItem(_currentPage, pager_RowsPerPage, pager_MaxRows, out pager_StartRecord, out pager_EndRecord);
                //spPager.RecordCount = pager_MaxRows;
                //spPager.RecordsPerPage = pager_RowsPerPage;
                //spPager.CurrentPage = _currentPage;
                //if (Expand)
                //    spPager.BaseUrl = DictionaryURL + "?expand=" + _expandText;
                //else
                //    spPager.BaseUrl = DictionaryURL + "?search=" + SearchString;

                //if (IsSpanish)
                //{
                //    spPager.PagerStyleSettings.NextPageText = "Siguiente&nbsp;&gt;";
                //    spPager.PagerStyleSettings.PrevPageText = "&lt;&nbsp;Anterior";
                //}

            }
        }

        protected string AudioPronounceLink(ListViewDataItem dataItemList)
        {
            string termPronunciation= (string)DataBinder.Eval(dataItemList.DataItem, "TermPronunciation");
            string audioMediaHTML=(string)DataBinder.Eval(dataItemList.DataItem, "AudioMediaHTML");
           
            if (!String.IsNullOrEmpty(audioMediaHTML))
            {
                audioMediaHTML = audioMediaHTML.Replace("[_audioMediaLocation]", ConfigurationSettings.AppSettings["CDRAudioMediaLocation"]);
                if (String.IsNullOrEmpty(termPronunciation))
                    return "<span class=\"CDR_audiofile\">" + audioMediaHTML + "</span>";
                else
                    return "<span class=\"CDR_audiofile\">" + audioMediaHTML + "</span>&nbsp;&nbsp;<span class=\"mtd_pronounce\">" + termPronunciation + "</span>";
            }
            else
            {
                if (String.IsNullOrEmpty(termPronunciation))
                    return "";
                else
                    return "<span class=\"mtd_pronounce\">" + termPronunciation + "</span>";
            }
        }


        protected string ResultListViewHrefOnclick(ListViewDataItem dataItemList)
        {
            if (WebAnalyticsOptions.IsEnabled)
                return "onclick=\"NCIAnalytics.TermsDictionaryResults(this,'" + (dataItemList.DataItemIndex + 1).ToString() + "');\""; // Load results onclick script
            else
                return "";
        }

        protected string LimitText(ListViewDataItem item, int numberOfCharacters)
        {
            const int MAX_WALKBACK = 30;
            string definition = DataBinder.Eval(item.DataItem, "DefinitionHTML").ToString();
            string term = DataBinder.Eval(item.DataItem, "TermName").ToString();

            numberOfCharacters = numberOfCharacters - term.Length;


            if (definition.Length > numberOfCharacters)
            {
                if (definition.Substring(numberOfCharacters - 1, 1) == " ")
                    definition = definition.Substring(0, numberOfCharacters - 1) + "...";
                else //walk back to next space
                {
                    int i;
                    for (i = numberOfCharacters; i > 0; i--)
                    {
                        if (i == MAX_WALKBACK)
                        {
                            i = 0;
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