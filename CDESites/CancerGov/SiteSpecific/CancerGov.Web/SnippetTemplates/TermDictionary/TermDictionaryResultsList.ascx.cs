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

        protected void Page_Load(object sender, EventArgs e)
        {
            dictionarySearchBlock.Dictionary = DictionaryType.Term;

            base.OnLoad(e);
            ValidateParams();
            GetQueryParams();

            //Set display props according to lang
            if (PageAssemblyContext.Current.PageAssemblyInstruction.Language == "es")
            {
                SetupSpanish();
            }
            else
            {
                SetupEnglish();
            }

            LoadData();

        }

        private void LoadData()
        {
            string language = string.Empty;
            if (PageAssemblyContext.Current.PageAssemblyInstruction.Language == "es")
            {
                language = "Spanish";
            }
            else
            {
                language = "English";
            }

            TermDictionaryCollection dataCollection = TermDictionaryManager.Search(language, SearchStr, 0, BContains);
            
                resultListView.DataSource = dataCollection;
                resultListView.DataBind();
                NumResults = dataCollection.Count;
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
        /// Set up Spanish Properties
        /// </summary>
        private void SetupSpanish()
        {
            lblResultsFor.Text = "resultados de:";
           
            ////common display features
            SetupCommon();
        }

        /// <summary>
        /// Set up English Properties
        /// </summary>
        private void SetupEnglish()
        {
         
            lblResultsFor.Text = "results found for:";
            
            //common display features
            SetupCommon();
        }

        /// <summary>
        /// Setup shared by English and Spanish versions
        /// </summary>
        private void SetupCommon()
        {
            string language = string.Empty;
            if (PageAssemblyContext.Current.PageAssemblyInstruction.Language == "es")
            {
                language = "Spanish";
            }
            else
            {
                language = "English";
            }

            if (!string.IsNullOrEmpty(SrcGroup))
                BContains = SrcGroup.Equals("Contains");

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
            SrcGroup = Strings.Clean(Request.Params["sgroup"]);
        }
    }

    
}