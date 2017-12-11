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
        protected global::System.Web.UI.WebControls.PlaceHolder pnlTermSearch;

        protected global::System.Web.UI.HtmlControls.HtmlForm aspnetForm;

        protected global::System.Web.UI.WebControls.Panel englishHelpText;

        protected global::System.Web.UI.WebControls.Panel espanolHelpText;

        protected global::System.Web.UI.WebControls.RadioButton radioStarts;

        protected global::System.Web.UI.WebControls.Label lblStartsWith;

        protected global::System.Web.UI.WebControls.RadioButton radioContains;

        protected global::System.Web.UI.WebControls.Label lblContains;

        protected global::System.Web.UI.WebControls.TextBox AutoComplete1;

        protected global::System.Web.UI.WebControls.Button btnSearch;

        protected global::System.Web.UI.WebControls.Panel helpButton;

        protected CancerGov.Dictionaries.SnippetControls.AlphaListBox alphaListBox;

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
            SearchStr = Sanitizer.GetSafeHtmlFragment(Request.Params["search"]);
            SearchStr = Strings.Clean(SearchStr);
            //SearchStr = Strings.Clean(Request.Params["search"]);
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

        /**
       * Add a filter for the Canonical URL.
       * The Canonical URL includes query parameters if they exist.
       */
        private void SetupCanonicalUrls(string englishDurl, string spanishDurl)
        {
            PageAssemblyContext.Current.PageAssemblyInstruction.AddUrlFilter(PageAssemblyInstructionUrls.CanonicalUrl, SetupUrlFilter);

            foreach (var lang in PageAssemblyContext.Current.PageAssemblyInstruction.TranslationKeys)
            {
                PageAssemblyContext.Current.PageAssemblyInstruction.AddTranslationFilter(lang, SetupUrlFilter);
            }

        }

        private void SetupUrlFilter(string name, NciUrl url)
        {
            if (!string.IsNullOrEmpty(CdrID))
                url.SetUrl(url.ToString() + "?cdrid=" + CdrID);
            else if (!string.IsNullOrEmpty(Expand))
            {
                if (Expand.Trim() == "#")
                {
                    Expand = "%23";
                }
                url.SetUrl(url.ToString() + "?expand=" + Expand);
            }
            else
                url.SetUrl(url.ToString());
        }

        /// <summary>
        /// Setup shared by English and Spanish versions
        /// </summary>
        private void SetupCommon()
        {
            radioStarts.InputAttributes.Add("onchange", "autoFunc();");
            radioContains.InputAttributes.Add("onchange", "autoFunc();");

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
                    SearchStr = "[0-9]";
                }
                else
                {
                    SearchStr = Expand.Trim().ToUpper();
                }
            }

            alphaListBox.TextOnly = (PageAssemblyContext.Current.DisplayVersion == DisplayVersions.Web) ? true : false;
            alphaListBox.Title = string.Empty;


            DictionaryURL = PageAssemblyContext.Current.requestedUrl.ToString();
            alphaListBox.BaseUrl = DictionaryURL;

            if (PageAssemblyContext.Current.PageAssemblyInstruction.Language == "es")
                DictionaryURLSpanish = DictionaryURL;

            DictionaryURLEnglish = DictionaryURL;

            SetupCanonicalUrls(DictionaryURLEnglish, DictionaryURLSpanish);
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

            if (!string.IsNullOrEmpty(Expand) && Expand.Trim().ToUpper().Equals("All"))
            {
                DictionaryURL = DictionaryURL + "?expand=" + Expand;
            }

            Response.Redirect(DictionaryURL);
        }
    }
}