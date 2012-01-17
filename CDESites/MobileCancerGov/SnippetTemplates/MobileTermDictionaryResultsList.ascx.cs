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
        // Query parameter values
        private string _searchStr = "";
        private int _currentPage = 0;
        private int _recordsPerPage = 0;
        private int _offSet = 0;
        private string _dictionaryURL = "";

        // Property variables 
        private string _language = "";
        private bool _showDefinition = true;
        private bool _expand = false;
        private string _expandText = "";

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
        public string Language
        {
            get { return _language; }
            set { _language = value; }
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
        public bool IsSpanish
        {
            get { return (Language == MobileTermDictionary.SPANISH); }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            // Setup private variables 
            _searchStr = Strings.Clean(Request.QueryString["search"]);
            _currentPage = Strings.ToInt(Request.Params["PageNum"], 1);
            _recordsPerPage = Strings.ToInt(Request.Params["RecordsPerPage"], 10);
            _offSet = Strings.ToInt(Request.Params["OffSet"], 0);
            _dictionaryURL = MobileTermDictionary.RawUrlClean(Page.Request.RawUrl);

            // Setup local variables
            //string languageParam = Strings.Clean(Request.QueryString["language"]);
            string languageParam = ""; //disable language selection by query parameter 
            string expandParam = Strings.Clean(Request.QueryString["expand"]);
            // Pager variables 
            int maxrows = 0;
            int rowsPerPage = 10; 


            // Define canonical URL
            PageAssemblyContext.Current.PageAssemblyInstruction.AddUrlFilter(PageAssemblyInstructionUrls.CanonicalUrl, (name, url) =>
            {
                url.SetUrl(Page.Request.RawUrl);
            }); 

            //Determine Language - set language related values
            string pageTitle;
            string buttonText;
            string language;
            MobileTermDictionary.DetermineLanguage(languageParam, out language, out pageTitle, out buttonText);
            _language = language;

            // Add search block (search input and button)
            litSearchBlock.Text = MobileTermDictionary.SearchBlock(MobileTermDictionary.RawUrlClean(Page.Request.RawUrl), SearchString,language, pageTitle, buttonText, true);
            litPageUrl.Text = MobileTermDictionary.RawUrlClean(Page.Request.RawUrl);

            TermDictionaryCollection dataCollection = null;
            if (!String.IsNullOrEmpty(SearchString)) // SearchString provided, do a term search
            {
                PageAssemblyContext.Current.PageAssemblyInstruction.AddUrlFilter(PageAssemblyInstructionUrls.AltLanguage, (name, url) =>
                {
                    url.SetUrl(url.ToString() + "?search=" + SearchString);
                }); 

                dataCollection = TermDictionaryManager.GetTermDictionaryList(language, SearchString, false, rowsPerPage, _currentPage, ref maxrows);
            }
            else if (!String.IsNullOrEmpty(expandParam)) // A-Z expand provided - do an A-Z search
            {
                _expand = true;
                _showDefinition = false;

                PageAssemblyContext.Current.PageAssemblyInstruction.AddUrlFilter(PageAssemblyInstructionUrls.AltLanguage, (name, url) =>
                {
                    url.SetUrl(url.ToString() + "?expand=" + _expandText.Trim());
                }); 
                
                _expandText = expandParam;
                if (_expandText == "#")
                {
                    _expandText = "[0-9]";
                }
                dataCollection = TermDictionaryManager.GetTermDictionaryList(language, _expandText.Trim().ToUpper(), false, rowsPerPage, _currentPage, ref maxrows);
 
            }

            if (dataCollection != null)
            {
                if (dataCollection.Count == 1 && !Expand) //if there is only 1 record - go directly to definition view
                {
                    string itemDefinitionUrl = DictionaryURL + "?cdrid=" + dataCollection[0].GlossaryTermID + "&language=" + Language;
                    Page.Response.Redirect(itemDefinitionUrl);
                }
                else
                {
                    resultListView.DataSource = dataCollection;
                    resultListView.DataBind();
                }

                // Setup Pager 
                int startRecord = 0;
                int endRecord = 0;
                SimplePager.GetFirstItemLastItem(_currentPage, rowsPerPage, maxrows, out startRecord, out endRecord);
                spPager.RecordCount = maxrows;
                spPager.RecordsPerPage = rowsPerPage;
                spPager.CurrentPage = _currentPage;
                if (Expand)
                    spPager.BaseUrl = litPageUrl.Text + "?expand=" + _expandText;
                else
                    spPager.BaseUrl = litPageUrl.Text + "?search=" + SearchString;
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