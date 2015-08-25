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
using NCI.Web.Dictionary;
using NCI.Services.Dictionary;
using NCI.Web.Dictionary.BusinessObjects;

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

        public Language DictionaryLanguage { get; set; }

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
                DictionaryLanguage = Language.Spanish;
                lblResultsFor.Text = "resultados de:";
            }
            else
            {
                lblResultsFor.Text = "results found for:";
                DictionaryLanguage = Language.English;
            }

            SetupCommon();

            LoadData();

        }

        private void LoadData()
        {
            DictionaryAppManager _dictionaryAppManager = new DictionaryAppManager();

            SearchType searchType = SearchType.Begins;
            if (BContains)
                searchType = SearchType.Contains;

            ExpandReturn resultList = new ExpandReturn();

            if (!String.IsNullOrEmpty(SearchStr)) // SearchString provided, do a term search
            {

                resultList = _dictionaryAppManager.Search(SearchStr, searchType, 0, int.MaxValue, NCI.Services.Dictionary.DictionaryType.term, DictionaryLanguage);

            }
            else if (!String.IsNullOrEmpty(Expand)) // A-Z expand provided - do an A-Z search
            {

                if (Expand.ToLower() == "all")
                    resultList = _dictionaryAppManager.Search("%", searchType, 0, int.MaxValue, NCI.Services.Dictionary.DictionaryType.term, DictionaryLanguage);
                else
                    resultList = _dictionaryAppManager.Search(Expand, searchType, 0, int.MaxValue, NCI.Services.Dictionary.DictionaryType.term, DictionaryLanguage);
            }

            if ((resultList.Meta.ResultCount == 1) && string.IsNullOrEmpty(Expand)) //if there is only 1 record - go directly to definition view
            {
                string itemDefinitionUrl = DictionaryURL + "?cdrid=" + resultList.Result[0].ID;
                Page.Response.Redirect(itemDefinitionUrl);
            }
            else
            {
                resultListView.DataSource = resultList.Result;
                resultListView.DataBind();
                NumResults = resultList.Meta.ResultCount;
                lblWord.Text = SearchStr.Replace("[[]", "[");
                lblNumResults.Text = NumResults.ToString();
                if (NumResults == 0)
                {
                    RenderNoResults();
                }

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

        
    }

    
}