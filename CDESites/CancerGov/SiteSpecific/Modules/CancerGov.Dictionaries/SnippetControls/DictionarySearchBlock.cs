using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using CancerGov.CDR.TermDictionary;
using NCI.Util;
using NCI.Web.CDE;
using NCI.Web.CDE.Modules;
using NCI.Web.CDE.UI;
using NCI.Web.CDE.WebAnalytics;
using NCI.Web.Dictionary;
using NCI.Web.Dictionary.BusinessObjects;
using Microsoft.Security.Application;
using NCI.Web;

namespace CancerGov.Dictionaries.SnippetControls
{
    public class DictionarySearchBlock : SnippetControl
    {
        protected System.Web.UI.WebControls.PlaceHolder pnlTermSearch;

        protected System.Web.UI.HtmlControls.HtmlForm aspnetForm;

        protected Panel englishHelpText;

        protected Panel espanolHelpText;

        protected RadioButton radioStarts;

        protected Label lblStartsWith;

        protected RadioButton radioContains;

        protected Label lblContains;

        protected TextBox AutoComplete1;

        protected Button btnSearch;

        protected Panel helpButton;

        protected AlphaListBox alphaListBox;

        public NCI.Web.Dictionary.DictionaryType Dictionary { get; set; }

        public string SearchStr { get; set; }

        public string Expand { get; set; }

        public string CdrID { get; set; }

        public string SrcGroup { get; set; }

        public bool BContains { get; set; }

        public String DictionaryLanguage { get; set; }

        public string DictionaryURLSpanish { get; set; }

        public string DictionaryURLEnglish { get; set; }

        public string DictionaryURL { get; set; }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            GetQueryParams();
            ValidateParams();

            //set up everything that is common for all three dictionaries
            SetupCommon();

            //code unique to each dictionary
            switch (Dictionary)
            {
                case NCI.Web.Dictionary.DictionaryType.term:
                    SetUpTermDictionary();
                    break;
                case NCI.Web.Dictionary.DictionaryType.genetic:
                    SetUpGeneticsDictionary();
                    break;
                case NCI.Web.Dictionary.DictionaryType.drug:
                    SetUpDrugDictionary();
                    break;
                default:
                    SetUpTermDictionary();
                    break;
            }
        }

        /// <summary>
        /// Saves the query parameters to support old gets
        /// </summary>
        private void GetQueryParams()
        {
            Expand = "";
            CdrID = "";
            SearchStr = "";
            SrcGroup = "";
            Expand = Strings.Clean(Request.Params["expand"]);
            CdrID = Strings.Clean(Request.Params["cdrid"]);
            SearchStr = Sanitizer.GetSafeHtmlFragment(Request.Params["q"]);
            SearchStr = Strings.Clean(SearchStr);
            SrcGroup = Strings.Clean(Request.Params["contains"]);
        }

        private void ValidateParams()
        {
            CdrID = Strings.Clean(Request.Params["cdrid"]);
            if (!string.IsNullOrEmpty(CdrID))
            {
                try
                {
                    Int32.Parse(CdrID);
                }
                catch (Exception ex)
                {
                    throw new Exception("Invalid CDRID" + CdrID);
                }
            }
        }

        /// <summary>
        /// Setup shared by English and Spanish versions
        /// </summary>
        private void SetupCommon()
        {
            // Set custom data attributes 
            radioStarts.InputAttributes.Add("data-autosuggest", "dict-radio-starts");
            radioContains.InputAttributes.Add("data-autosuggest", "dict-radio-contains");

            //set language to english by default
            //this only applies to Genetics and Drug Dictionaries
            DictionaryLanguage = "en";

            //set the contains radiobutton to false by default
            BContains = false;
            if (!string.IsNullOrEmpty(SrcGroup))
            {
                BContains = Convert.ToBoolean(SrcGroup);

                RadioButton rd = (RadioButton)FindControl("radioContains");

                if (BContains)
                    rd.Checked = BContains;
            }

            if (!string.IsNullOrEmpty(SearchStr))
            {
                SearchStr = Sanitizer.GetSafeHtmlFragment(SearchStr);
                AutoComplete1.Text = SearchStr;
            }

            if (!string.IsNullOrEmpty(Expand))
            {
                if (Expand.Trim() == "#")
                {
                    Expand = "[0-9]";
                }
                else
                {
                    Expand = Expand.Trim().ToUpper();
                }
            }

            alphaListBox.TextOnly = (PageAssemblyContext.Current.DisplayVersion == DisplayVersions.Web) ? true : false;
            alphaListBox.Title = string.Empty;

            DictionaryURL = PageAssemblyContext.Current.requestedUrl.ToString();
            alphaListBox.BaseUrl = DictionaryURL;

            DictionaryURLSpanish = DictionaryURL;
            DictionaryURLEnglish = DictionaryURL;
        }

        #region "Term Dictionary Methods"

        private void SetUpTermDictionary()
        {
            bool _isSpanish = false;

            if (PageAssemblyContext.Current.PageAssemblyInstruction.Language == "es")
            {
                DictionaryLanguage = PageAssemblyContext.Current.PageAssemblyInstruction.Language;
                SetupSpanish();
                _isSpanish = true;
            }
            else
            {
                DictionaryLanguage = PageAssemblyContext.Current.PageAssemblyInstruction.Language;
                SetupEnglish();
            }

            if (WebAnalyticsOptions.IsEnabled)
            {
                // Add page name to analytics
                this.PageInstruction.SetWebAnalytics(WebAnalyticsOptions.eVars.evar1, wbField =>
                {
                    string suffix = "";
                    if (!string.IsNullOrEmpty(Expand))
                        suffix = " - AlphaNumericBrowse";
                    else if (!string.IsNullOrEmpty(CdrID))
                        suffix = " - Definition";
                    wbField.Value = ConfigurationManager.AppSettings["HostName"] + PageAssemblyContext.Current.requestedUrl.ToString() + suffix;
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
            AutoComplete1.Attributes.Add("aria-label", "Escriba frase o palabra clave");
            AutoComplete1.Attributes.Add("placeholder", "Escriba frase o palabra clave");
            AutoComplete1.Attributes.Add("data-autosuggest", "dict-autocomplete");

            lblStartsWith.Text = "Empieza con";
            lblContains.Text = "Contiene";

            btnSearch.Text = "Buscar";
            btnSearch.ToolTip = "Buscar";

            // hide english help text and show espanol
            englishHelpText.Visible = false;
            espanolHelpText.Visible = true;
        }

        /// <summary>
        /// Set up English Properties
        /// </summary>
        private void SetupEnglish()
        {
            //Controls            
            AutoComplete1.Attributes.Add("aria-label", "Enter keywords or phrases");
            AutoComplete1.Attributes.Add("placeholder", "Enter keywords or phrases");
            AutoComplete1.Attributes.Add("data-autosuggest", "dict-autocomplete");

            btnSearch.Text = "Search";

            // show english help text and hide espanol
            englishHelpText.Visible = true;
            espanolHelpText.Visible = false;
        }
        #endregion

        private void SetUpGeneticsDictionary()
        {
            SetupEnglish();
            alphaListBox.ShowAll = true;

            if (WebAnalyticsOptions.IsEnabled)
            {
                // Add page name to analytics
                this.PageInstruction.SetWebAnalytics(WebAnalyticsOptions.eVars.evar1, wbField =>
                {
                    string suffix = "";
                    if (!string.IsNullOrEmpty(Expand))
                        suffix = " - AlphaNumericBrowse";
                    else if (!string.IsNullOrEmpty(CdrID))
                        suffix = " - Definition";
                    wbField.Value = ConfigurationManager.AppSettings["HostName"] + PageAssemblyContext.Current.requestedUrl.ToString() + suffix;
                });

                Page.Form.Attributes.Add("onsubmit", "NCIAnalytics.GeneticsDictionarySearchNew(this);");
                alphaListBox.WebAnalyticsFunction = "NCIAnalytics.GeneticsDictionarySearchAlphaList"; // Load A-Z list onclick script
            }
        }

        private void SetUpDrugDictionary()
        {
            SetupEnglish();
            alphaListBox.ShowAll = true;
            helpButton.Visible = true;

            if (WebAnalyticsOptions.IsEnabled)
            {
                // Add page name to analytics
                this.PageInstruction.SetWebAnalytics(WebAnalyticsOptions.eVars.evar1, wbField =>
                {
                    string suffix = "";
                    if (!string.IsNullOrEmpty(Expand))
                        suffix = " - AlphaNumericBrowse";
                    else if (!string.IsNullOrEmpty(CdrID))
                        suffix = " - Definition";
                    wbField.Value = ConfigurationManager.AppSettings["HostName"] + PageAssemblyContext.Current.requestedUrl.ToString() + suffix;
                });

                Page.Form.Attributes.Add("onsubmit", "NCIAnalytics.DrugDictionarySearch(this);"); // Load from onsubmit script
                alphaListBox.WebAnalyticsFunction = "NCIAnalytics.DrugDictionarySearchAlphaList"; // Load A-Z list onclick script
            }
        }

        protected void btnSearch_OnClick(object sender, EventArgs e)
        {
            SearchStr = Sanitizer.GetSafeHtmlFragment(AutoComplete1.Text);
            SearchStr = SearchStr.Replace("[", "[[]");
            CdrID = string.Empty;

            if (!string.IsNullOrEmpty(SearchStr))
            {
                if (PageAssemblyContext.Current.PageAssemblyInstruction.Language == "es")
                {
                    DictionaryURL = DictionaryURL + "/buscar?q=" + SearchStr;
                }
                else
                {
                    DictionaryURL = DictionaryURL + "/search?q=" + SearchStr;
                }
            }

            RadioButton rd = (RadioButton)FindControl("radioContains");

            if (rd.Checked == true)
            {
                BContains = true;
                if (!string.IsNullOrEmpty(SearchStr))
                    DictionaryURL = DictionaryURL + "&contains=true";
                else
                    DictionaryURL = DictionaryURL + "?contains=true";
            }

            if (!string.IsNullOrEmpty(Expand) && Expand.Trim().ToUpper().Equals("All"))
            {
                DictionaryURL = DictionaryURL + "?expand=" + Expand;
            }

            Response.Redirect(DictionaryURL);
        }
    }
}