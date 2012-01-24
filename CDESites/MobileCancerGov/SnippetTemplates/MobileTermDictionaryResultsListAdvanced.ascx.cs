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
using NCI.Web;

namespace MobileCancerGov.Web.SnippetTemplates
{
    public partial class MobileTermDictionaryResultsListAdvanced : System.Web.UI.UserControl
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

        public string SpitOutJavaScript()
        {
   //         StringBuilder javaScriptBlock = new StringBuilder();
   //         javaScriptBlock.AppendLine("<script type=\"text/javascript\">");
   //         javaScriptBlock.AppendLine("   jQuery(document).ready(");
   //         javaScriptBlock.AppendLine("      function($) {");
   //         javaScriptBlock.AppendLine(" ");		
   //         javaScriptBlock.AppendLine("         $('.scroll_container').data('pageNumber', 1);");
   //         javaScriptBlock.AppendLine("         $('.scroll_container').onScrollBeyond(function(target) {");
   //         javaScriptBlock.AppendLine("		   //Add loading indicator to dom...");
   //         javaScriptBlock.AppendLine(" ");										
   //         javaScriptBlock.AppendLine("		   ajaxSettings = {");
   //         javaScriptBlock.AppendLine("		      'success': function(data, textStatus) {");
   //         javaScriptBlock.AppendLine(" ");							
   //         javaScriptBlock.AppendLine("			     $(data.TermDictionaryItems).each(");
   //         javaScriptBlock.AppendLine("				    function() { // output block structure for each row");
   //         javaScriptBlock.AppendLine("					   var localContainer = $(\"<li>\");");
   //         javaScriptBlock.AppendLine("					   localContainer.appendTo($('.scroll_container'));");
   //         javaScriptBlock.AppendLine(" ");						
   //         javaScriptBlock.AppendLine("					   $(\"<a>\") ");
   //                                     .attr('href', '/foo?id=' + this.id)
   //                                     .html(this.item)
   //                                     .appendTo(localContainer);
									
   //                                 $("<p>")
   //                                     .html(this.TermDictionaryDetail.DefinitionHTML)
   //                                     .appendTo(localContainer);
   //                             }
   //                         );
   //                         $('.scroll_container').data().pageNumber++;
   //                     },
   //                     'dataType': 'json',
   //                     'url': 'http://localhost:7069/TermDictionary.svc/GetTermDictionaryListJSON/English?searchTerm=' + $('#litSearchStr').text() + '&Contains=false&MaxRows=10&PageNumber=' + $('.scroll_container').data().pageNumber,
   //                 }
   //                 jQuery.ajax( ajaxSettings );
   //             }
   //         );		
   //     }
   // );			
   //</script>

            return "<script type=\"text/javascript\">alert('SpitOutJavaScript: [" + SearchString + "] ')</script>";
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            // Dictionary query parameters
            string expandParam = Strings.Clean(Request.QueryString["expand"]);
            //string languageParam = Strings.Clean(Request.QueryString["language"]);
            string languageParam = ""; //disable language selection by query parameter 
            _searchStr = Strings.Clean(Request.QueryString["search"]);
            if (!String.IsNullOrEmpty(_searchStr))
                _searchStr = _searchStr.Replace("[", "[[]");
            
            //FOR TESTING !!@!!!
            _searchStr = "can";
            litSearchStr.Text = "can";

            // Pager query parameter variables
            _currentPage = Strings.ToInt(Request.Params["PageNum"], 1);
            _recordsPerPage = Strings.ToInt(Request.Params["RecordsPerPage"], 10);
            _offSet = Strings.ToInt(Request.Params["OffSet"], 0);

            //Determine Language - set language related values
            string pageTitle;
            string buttonText;
            string language;
            string reDirect;
            MobileTermDictionary.DetermineLanguage(languageParam, out language, out pageTitle, out buttonText, out reDirect);
            _language = language;

            _dictionaryURL = PageAssemblyContext.Current.PageAssemblyInstruction.GetUrl("CurrentUrl").ToString();
            litPageUrl.Text = _dictionaryURL;
            litSearchBlock.Text = MobileTermDictionary.SearchBlock(_dictionaryURL, SearchString, language, pageTitle, buttonText, true);


            //TermDictionaryCollection dataCollection = null;
            //if (!String.IsNullOrEmpty(SearchString)) // SearchString provided, do a term search
            //{
            //    PageAssemblyContext.Current.PageAssemblyInstruction.AddUrlFilter(PageAssemblyInstructionUrls.AltLanguage, (name, url) =>
            //    {
            //        url.QueryParameters.Add("search", SearchString);
            //    });
            //    PageAssemblyContext.Current.PageAssemblyInstruction.AddUrlFilter("CurrentUrl", (name, url) =>
            //    {
            //        url.QueryParameters.Add("search", SearchString);
            //    });

            //    dataCollection = TermDictionaryManager.GetTermDictionaryList(language, SearchString, false, pager_RowsPerPage, _currentPage, ref pager_MaxRows);
            //}
            //else if (!String.IsNullOrEmpty(expandParam)) // A-Z expand provided - do an A-Z search
            //{
            //    _showDefinition = false;
            //    _expand = true;
            //    _expandText = expandParam;
            //    if (_expandText == "#")
            //    {
            //        _expandText = "[0-9]";
            //    }

            //    PageAssemblyContext.Current.PageAssemblyInstruction.AddUrlFilter(PageAssemblyInstructionUrls.AltLanguage, (name, url) =>
            //    {
            //        url.QueryParameters.Add("expand", _expandText);
            //    });
            //    PageAssemblyContext.Current.PageAssemblyInstruction.AddUrlFilter("CurrentUrl", (name, url) =>
            //    {
            //        url.QueryParameters.Add("expand", _expandText);
            //    });

            //    dataCollection = TermDictionaryManager.GetTermDictionaryList(language, _expandText.Trim().ToUpper(), false, pager_RowsPerPage, _currentPage, ref pager_MaxRows);
            //}

            //if (dataCollection != null)
            //{
            //    if (dataCollection.Count == 1 && !Expand) //if there is only 1 record - go directly to definition view
            //    {
            //        //string itemDefinitionUrl = DictionaryURL + "?cdrid=" + dataCollection[0].GlossaryTermID + "&language=" + Language;
            //        string itemDefinitionUrl = DictionaryURL + "?cdrid=" + dataCollection[0].GlossaryTermID;
            //        Page.Response.Redirect(itemDefinitionUrl);
            //    }
            //    else // bind results 
            //    {
            //        //Load first rows 

            //    }

            //}
        }
    }
}