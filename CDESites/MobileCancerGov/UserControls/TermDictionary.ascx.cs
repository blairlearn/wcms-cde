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
using NCI.Web.CDE.CapabilitiesDetection;
using NCI.Web.CDE;
using NCI.Web.CDE.UI;
using CancerGov.CDR.TermDictionary;
using CancerGov.Text;
using NCI.Web.CDE.WebAnalytics;
using MobileCancerGov.Web.SnippetTemplates;


namespace MobileCancerGov.Web.UserControls
{
//    public partial class TermDictionary : System.Web.UI.UserControl
    public partial class TermDictionary : SnippetControl

    {
        //These are the QueryString related variables
        private string _searchStr = string.Empty;
        private string _expand = string.Empty;
        private string _cdrid = string.Empty;
        private string _queryStringLanguage = string.Empty;
        private string _srcGroup = string.Empty;
        private bool _isSpanish = false;
        private string _pagePrintUrl = string.Empty;
        private string _pageOptionsBoxTitle = string.Empty;
        private string _prevText = string.Empty;
        private string _nextText = string.Empty;
        private bool _bContains = false;
        private int _numResults = 0;
        private string _dictionaryURL = string.Empty;
        private string _dictionaryURLSpanish = string.Empty;
        private string _dictionaryURLEnglish = string.Empty;



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

        public string QueryStringLang
        {
            get { return _queryStringLanguage; }
            set { _queryStringLanguage = value; }
        }


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




        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            ValidateParams();
            GetQueryParams();

            
            if((DisplayDeviceDetector.DisplayDevice == DisplayDevices.AdvancedMobile) ||
               (DisplayDeviceDetector.DisplayDevice == DisplayDevices.Desktop) ||
               (DisplayDeviceDetector.DisplayDevice == DisplayDevices.Tablet)) 
            {
                advanced.Visible = true;
                basic.Visible = false;
            }
            else
            {
                advanced.Visible = false;
                basic.Visible = true;
            }

            if (Page.Request.RequestType.Equals("POST")) //This is a POST(back)
            {

                SearchStr = Request.Form["SearchStr"];
                SearchStr = SearchStr.Replace("[", "[[]");
                CdrID = string.Empty;
                Expand = string.Empty;

                if (string.IsNullOrEmpty(SearchStr))
                {
                    //ActivateDefaultView();
                }

                else
                {
                    string language = string.Empty;
                    //if (PageAssemblyContext.Current.PageAssemblyInstruction.Language == "en")
                    //{
                    language = "English";
                    //}
                    //else
                    //{
                    //    language = "Spanish";
                    //}
                  
                    TermDictionaryCollection dataCollection = TermDictionaryManager.Search(language, SearchStr,0,true);
                    if (dataCollection.Count == 1) //if there is only 1 record - go directly to definition view
                    {
                        // If only one definition found, redirect so URL contains CdrID
                        if (QueryStringLang == string.Empty)
                            Page.Response.Redirect(DictionaryURL + "?CdrID=" + dataCollection[0].GlossaryTermID.ToString(), true);
                        else
                            Page.Response.Redirect(DictionaryURL + "?CdrID=" + dataCollection[0].GlossaryTermID.ToString() + QueryStringLang, true);

                    }
                    else
                    {
                        resultListView.DataSource = dataCollection;
                        resultListView.DataBind();
                        NumResults = dataCollection.Count;
                        ActivateResultsListView();
                    }
                }
            }



        }

        private void ValidateParams()
        {
            CdrID = Strings.Clean(Request.Params["cdrid"]);
            if (!string.IsNullOrEmpty(CdrID.Trim()))
                try
                {
                    Int32.Parse(CdrID);
                }
                catch (Exception ex)
                {
                    throw new Exception("Invalid CDRID" + CdrID);

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
            //SrcGroup = Strings.Clean(Request.Params["sgroup"]);
        }


        private void ActivateResultsListView()
        {
            TermSearch.Visible = false;
            ViewResultsList.Visible = true;

            //litBackToTop.Visible = (NumResults > 1);
            //if (NumResults == 0)
            //{
            //    RenderNoResults();
            //}
        }


        protected string ResultListViewHrefOnclick(ListViewDataItem dataItem)
        {
            if (WebAnalyticsOptions.IsEnabled)
                return "onclick=\"NCIAnalytics.TermsDictionaryResults(this,'" + (dataItem.DataItemIndex + 1).ToString() + "');\""; // Load results onclick script
            else
                return "";
        }


        protected void btnToggle_Click(object sender, EventArgs e)
        {

        }

    }
}