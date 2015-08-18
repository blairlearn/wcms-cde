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
using NCI.Web.CDE.WebAnalytics;
using System.Configuration;
using NCI.Web.CDE.Modules;

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

        public DisplayLanguage Language { get; set; }

        public string DictionaryURLSpanish { get; set; }

        public string DictionaryURLEnglish { get; set; }

        public string DictionaryURL { get; set; }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            GetQueryParams();
            ValidateParams();
           

            
            switch (Dictionary)
            {
                case DictionaryType.Term:
                    phTermDictionarySearchBlockText.Visible = true;
                    SetUpTermDictionary();
                    break;
                case DictionaryType.Genetic:
                    phGeneticsTermDictionarySearchBlockText.Visible = true;
                    SetUpGeneticsDictionary();
                    break;
                case DictionaryType.Drug:
                    phDrugDictionarySearchBlockText.Visible = true;
                    SetUpDrugDictionary();
                    break;
                default:
                    phTermDictionarySearchBlockText.Visible = true;
                    SetUpTermDictionary();
                    break;
            }

        }

        /// <summary>
        /// Saves the quesry parameters to support old gets
        /// </summary>
        private void GetQueryParams()
        {
            Expand = "";
            CdrID = "";
            SearchStr = "";
            SrcGroup = "";
            Expand = Strings.Clean(Request.Params["expand"]);
            CdrID = Strings.Clean(Request.Params["cdrid"]);
            SearchStr = Strings.Clean(Request.Params["search"]);
            SrcGroup = Strings.Clean(Request.Params["contains"]);
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

    
        #region "Term Dictionary Methods"

        private void SetUpTermDictionary()
        {
            //dictionarySearchForm 
            bool _isSpanish = false;
                                   
            if (PageAssemblyContext.Current.PageAssemblyInstruction.Language == "es")
            {
                Language = DisplayLanguage.Spanish;
                SetupSpanish();
                _isSpanish = true;
            }
            else
            {
                Language = DisplayLanguage.English;
                SetupEnglish();
            }

            TermDictionaryCollection dataCollection = TermDictionaryManager.Search(Language.ToString(), "_", 0, false);
            TotalCount = dataCollection.Count;

            
            if (WebAnalyticsOptions.IsEnabled)
            {
                // Add page name to analytics
                this.PageInstruction.SetWebAnalytics(WebAnalyticsOptions.eVars.evar1, wbField =>
                {
                    string suffix = "";
                    if (Expand != "")
                        suffix = " - AlphaNumericBrowse";
                    else if (CdrID != "")
                        suffix = " - Definition";
                    wbField.Value = ConfigurationSettings.AppSettings["HostName"] + PageAssemblyContext.Current.requestedUrl.ToString() + suffix;
                });

                Page.Form.Attributes.Add("onsubmit", "NCIAnalytics.TermsDictionarySearch(this," + _isSpanish.ToString().ToLower() + ");"); // Load from onsubit script
                if (_isSpanish)
                    alphaListBox.WebAnalyticsFunction = "NCIAnalytics.TermsDictionarySearchAlphaListSpanish"; // Load A-Z list onclick script
                else
                    alphaListBox.WebAnalyticsFunction = "NCIAnalytics.TermsDictionarySearchAlphaList"; // Load A-Z list onclick script
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
            radioStarts.InputAttributes.Add("onchange", "autoFunc();");
            radioContains.InputAttributes.Add("onchange", "autoFunc();");

            if (!string.IsNullOrEmpty(SrcGroup))
            {
                BContains = Convert.ToBoolean(SrcGroup);

                RadioButton rd = (RadioButton)FindControl("radioContains");

                if (BContains)
                    rd.Checked = BContains;
            }

            if (!string.IsNullOrEmpty(SearchStr))
                AutoComplete1.Text = SearchStr;

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

          alphaListBox.BaseUrl = DictionaryURL;
            
        }

        private void LoadData() 
        { 

        
        }

        #endregion
        

        private void SetUpGeneticsDictionary() 
        {
            SetupEnglish();
            SetupCommon();
        }

        private void SetUpDrugDictionary() 
        {
            SetupEnglish();
            SetupCommon();
        }

        protected void btnGo_OnClick(object sender, EventArgs e)
        {
            SearchStr = AutoComplete1.Text;// Request.Params[AutoComplete1.UniqueID];
            SearchStr = SearchStr.Replace("[", "[[]");
            CdrID = string.Empty;
            Expand = string.Empty;

            if (!string.IsNullOrEmpty(SearchStr))
                DictionaryURL = DictionaryURL + "?search=" + SearchStr;

            RadioButton rd = (RadioButton)FindControl("radioContains");

            if (rd.Checked == true)
            {
                BContains = true;
                if (!string.IsNullOrEmpty(SearchStr))
                    DictionaryURL = DictionaryURL + "&contains=true";
                else
                    DictionaryURL = DictionaryURL + "?contains=true";
            }

            Response.Redirect(DictionaryURL);
        }
    }
}