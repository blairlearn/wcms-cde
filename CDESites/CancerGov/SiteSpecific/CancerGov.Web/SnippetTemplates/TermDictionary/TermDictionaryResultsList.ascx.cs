using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using NCI.Web.CDE.UI;
using NCI.Util;
using NCI.Web.CDE;
using CancerGov.CDR.TermDictionary;
using NCI.Web.CDE.WebAnalytics;
using System.Configuration;

namespace CancerGov.Web.SnippetTemplates
{
    public partial class TermDictionaryResultsList : SnippetControl
    {
        public string SearchStr{ get; set; }

        public string Expand { get; set; }

        public string CdrID { get; set; }

        public string SrcGroup { get; set; }

        public bool BContains { get; set; }

        public int NumResults { get; set; }

        public string DictionaryURL { get; set; }

        public DisplayLanguage Language { get; set; }

        protected void Page_Load(object sender, EventArgs e)
        {
            dictionarySearchBlock.Dictionary = DictionaryType.Term;
            dictionarySearchBlock.DictionaryURL = PageAssemblyContext.Current.requestedUrl.ToString();
            DictionaryURL = PageAssemblyContext.Current.requestedUrl.ToString();

            //base.OnLoad(e);
            GetQueryParams();
            ValidateParams();
            
            //Set display props according to lang
            if (PageAssemblyContext.Current.PageAssemblyInstruction.Language == "es")
            {
                Language = DisplayLanguage.Spanish;
                lblResultsFor.Text = "resultados de:";
            }
            else
            {
                lblResultsFor.Text = "results found for:";
                Language = DisplayLanguage.English;
            }

            SetupCommon();

            LoadData();

        }

        private void LoadData()
        {
            string language = string.Empty;

            TermDictionaryCollection dataCollection = TermDictionaryManager.Search(Language.ToString(), SearchStr, 0, BContains);
            
                resultListView.DataSource = dataCollection;
                resultListView.DataBind();
                NumResults = dataCollection.Count;
                lblWord.Text = SearchStr.Replace("[[]", "[");
                lblNumResults.Text = NumResults.ToString();
                if (NumResults == 0)
                {
                    RenderNoResults();
                }
            

        }

        private void RenderNoResults()
        {
            Control c = resultListView.Controls[0];
            Panel noDataEngPanel = (Panel)c.FindControl("pnlNoDataEnglish");
            Panel noDataSpanPanel = (Panel)c.FindControl("pnlNoDataSpanish");

            if (PageAssemblyContext.Current.PageAssemblyInstruction.Language == "es")
                noDataSpanPanel.Visible = true;
            else
                noDataEngPanel.Visible = true;
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
                    SearchStr = "[0-9]";
                }
                else
                {
                    SearchStr = Expand.Trim().ToUpper();
                }
            }
            
        }

        private void ValidateParams()
        {
            CdrID = Strings.Clean(Request.Params["cdrid"]);
            if (!string.IsNullOrEmpty(CdrID))
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
            SearchStr = Strings.Clean(Request.Params["search"]);
            SrcGroup = Strings.Clean(Request.Params["contains"]);
        }

        protected string ResultListViewHrefOnclick(ListViewDataItem dataItem)
        {
            if (WebAnalyticsOptions.IsEnabled)
                return "onclick=\"NCIAnalytics.TermsDictionaryResults(this,'" + (dataItem.DataItemIndex + 1).ToString() + "');\""; // Load results onclick script
            else
                return "";
        }

        protected string AudioMediaHTML(object objData)
        {
            string audioMediaHTML = String.Empty;
            if (objData != null)
            {
                audioMediaHTML = objData.ToString();
                audioMediaHTML = audioMediaHTML.Replace("[_audioMediaLocation]", ConfigurationSettings.AppSettings["CDRAudioMediaLocation"]);
            }

            return audioMediaHTML;
        }
    }

    
}