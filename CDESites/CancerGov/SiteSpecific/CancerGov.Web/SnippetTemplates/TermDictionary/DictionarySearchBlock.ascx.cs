using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using NCI.Web.CDE.UI;
using CancerGov.CDR.TermDictionary;
using NCI.Web.CDE;
using NCI.Util;

namespace CancerGov.Web.SnippetTemplates
{
    public partial class DictionarySearchBlock : SnippetControl
    {
        public DictionaryType Dictionary { get; set; }

        public string SearchStr{ get; set; }

        public string Expand { get; set; }

        public string CdrID { get; set; }

        public string SrcGroup { get; set; }

        public bool BContains { get; set; }

        public int TotalCount = 0;

        protected void Page_Load(object sender, EventArgs e)
        {
            base.OnLoad(e);
            ValidateParams();
            GetQueryParams();

            
            switch (Dictionary)
            {
                case DictionaryType.Term:
                    SetUpTermDictionary();
                    break;
                case DictionaryType.Genetic:
                    SetUpGeneticsDictionary();
                    break;
                case DictionaryType.Drug:
                    SetUpDrugDictionary();
                    break;
                default:
                    SetUpTermDictionary();
                    break;
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

        #region "Term Dictionary Methods"

        private void SetUpTermDictionary()
        {
            string language = "";

            if (PageAssemblyContext.Current.PageAssemblyInstruction.Language == "es")
                language = "Spanish";
            else
                language = "English";

            TermDictionaryCollection dataCollection = TermDictionaryManager.Search(language, "_", 0, false);
            TotalCount = dataCollection.Count;

            //Set display props according to lang
            if (PageAssemblyContext.Current.PageAssemblyInstruction.Language == "es")
            {
                SetupSpanish();
            }
            else
            {
                SetupEnglish();
            }
        }

        /// <summary>
        /// Set up Spanish Properties
        /// </summary>
        private void SetupSpanish()
        {
            //_isSpanish = true;

            //Controls
            AutoComplete1.Attributes.Add("aria-label", "Escriba frase o palabra clave");
            AutoComplete1.Attributes.Add("placeholder", "Escriba frase o palabra clave");

            lblStartsWith.Text = "Empieza con";
            lblContains.Text = "Contiene";

            pnlIntroEnglish.Visible = false;
            pnlIntroSpanish.Visible = true;

            btnGo.Text = "Buscar";
            btnGo.ToolTip = "Buscar";
                        
            ////common display features
            SetupCommon();
        }

        /// <summary>
        /// Set up English Properties
        /// </summary>
        private void SetupEnglish()
        {
            //Controls            
            AutoComplete1.Attributes.Add("aria-label", "Enter keywords or phrases");
            AutoComplete1.Attributes.Add("placeholder", "Enter keywords or phrases");

            btnGo.Text = "Search";

            pnlIntroEnglish.Visible = true;
            pnlIntroSpanish.Visible = false;

          
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

            radioStarts.InputAttributes.Add("onchange", "autoFunc();");
            radioContains.InputAttributes.Add("onchange", "autoFunc();");

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
            
            if (PageAssemblyContext.Current.DisplayVersion == DisplayVersions.Print)
            {
                pnlTermSearch.Visible = false;

            }
            else
            {
                alphaListBox.TextOnly = (PageAssemblyContext.Current.DisplayVersion == DisplayVersions.Web) ? true : false;
                alphaListBox.Title = string.Empty;
            }
        } 

        #endregion
        

        private void SetUpGeneticsDictionary() 
        {
            SetupEnglish();
        }

        private void SetUpDrugDictionary() 
        {
            SetupEnglish();
        }
    }
}