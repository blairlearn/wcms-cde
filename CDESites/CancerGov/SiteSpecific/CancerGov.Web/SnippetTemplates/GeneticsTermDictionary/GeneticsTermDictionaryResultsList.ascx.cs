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
using NCI.Services.Dictionary;
using NCI.Web.Dictionary;
using NCI.Web.Dictionary.BusinessObjects;

namespace CancerGov.Web.SnippetTemplates
{
    public partial class GenerticsTermDictionaryResultsList : SnippetControl
    {
        public string SearchStr { get; set; }

        public string Expand { get; set; }

        public string CdrID { get; set; }

        public string SrcGroup { get; set; }

        public bool BContains { get; set; }

        public int NumResults { get; set; }

        public string DictionaryURL { get; set; }

        public Language DictionaryLanguage { get; set; }

        protected void Page_Load(object sender, EventArgs e)
        {
            dictionarySearchBlock.Dictionary = DictionaryType.Genetic;
            dictionarySearchBlock.DictionaryURL = PageAssemblyContext.Current.requestedUrl.ToString();
            DictionaryURL = PageAssemblyContext.Current.requestedUrl.ToString();

            //base.OnLoad(e);
            GetQueryParams();
            ValidateParams();

            //For Genetics dictionary language is always English
            DictionaryLanguage = Language.English;

            ExpandReturn resultList = new ExpandReturn();
            DictionaryAppManager _dictionaryAppManager = new DictionaryAppManager();

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
                    if (!Expand.Trim().ToUpper().Equals("ALL"))
                        SearchStr = Expand.Trim().ToUpper();
                }
            }


            SearchType searchType = SearchType.Begins;
            if (BContains)
                searchType = SearchType.Contains;



            if (!String.IsNullOrEmpty(SearchStr)) // SearchString provided, do a term search
            {

                resultList = _dictionaryAppManager.Search(SearchStr, searchType, 0, int.MaxValue, NCI.Services.Dictionary.DictionaryType.genetic, DictionaryLanguage);

            }
            else if (!String.IsNullOrEmpty(Expand)) // A-Z expand provided - do an A-Z search
            {

                if (Expand.ToLower() == "all")
                    resultList = _dictionaryAppManager.Search("%", searchType, 0, int.MaxValue, NCI.Services.Dictionary.DictionaryType.genetic, DictionaryLanguage);
                else
                    resultList = _dictionaryAppManager.Search(Expand, searchType, 0, int.MaxValue, NCI.Services.Dictionary.DictionaryType.genetic, DictionaryLanguage);
            }

            if (resultList != null)
            {
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
                    if (!string.IsNullOrEmpty(SearchStr))
                        lblWord.Text = SearchStr.Replace("[[]", "[");
                    else if (!string.IsNullOrEmpty(Expand))
                        lblWord.Text = Expand;
                    lblNumResults.Text = NumResults.ToString();
                    if (NumResults == 0)
                    {
                        Control c = resultListView.Controls[0];
                        Panel noMatched = (Panel)c.FindControl("noMatched");
                        if (noMatched != null)
                            noMatched.Visible = true;
                    }
                
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


        protected string ResultListViewHrefOnclick(ListViewDataItem dataItemList)
        {
            if (WebAnalyticsOptions.IsEnabled)
                return "onclick=\"NCIAnalytics.GeneticsDictionaryResults(this,'" + (dataItemList.DataItemIndex + 1).ToString() + "');\""; // Load results onclick script
            else
                return "";
        }

        
    }
}