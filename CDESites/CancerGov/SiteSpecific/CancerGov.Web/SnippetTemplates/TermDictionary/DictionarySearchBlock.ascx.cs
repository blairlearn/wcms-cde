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
               
        public String DictionaryLanguage { get; set; }

        public string DictionaryURLSpanish { get; set; }

        public string DictionaryURLEnglish { get; set; }

        public string DictionaryURL { get; set; }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            GetQueryParams();
            ValidateParams();

            SetupCommon();

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

        // SetupCanonicalUrls(DictionaryURLEnglish, DictionaryURLSpanish);

        /**
      * Add a filter for the Canonical URL.
      * The Canonical URL includes query parameters if they exist.
      */
        private void SetupCanonicalUrls(string englishDurl, string spanishDurl)
        {
            PageAssemblyContext.Current.PageAssemblyInstruction.AddUrlFilter(PageAssemblyInstructionUrls.CanonicalUrl, (name, url) =>
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
            });
            
            string canonicalUrl = PageAssemblyContext.Current.PageAssemblyInstruction.GetUrl("CanonicalUrl").ToString();
            PageAssemblyContext.Current.PageAssemblyInstruction.AddTranslationFilter("CanonicalTranslation", (name, url) =>
            {
                if (canonicalUrl.IndexOf(englishDurl) > -1)
                    url.SetUrl(canonicalUrl.Replace(englishDurl, spanishDurl));
                else if (canonicalUrl.IndexOf(spanishDurl) > -1)
                    url.SetUrl(canonicalUrl.Replace(spanishDurl, englishDurl));
                else
                    url.SetUrl("");
            });

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

            //set this false by default
            BContains = false;
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
            AutoComplete1.Attributes.Add("aria-label", "Escriba frase o palabra clave");
            AutoComplete1.Attributes.Add("placeholder", "Escriba frase o palabra clave");

            lblStartsWith.Text = "Empieza con";
            lblContains.Text = "Contiene";
                        
            btnSearch.Text = "Buscar";
            btnSearch.ToolTip = "Buscar";
                        
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
                    wbField.Value = ConfigurationSettings.AppSettings["HostName"] + PageAssemblyContext.Current.requestedUrl.ToString() + suffix;
                });

                Page.Form.Attributes.Add("onsubmit", "NCIAnalytics.GeneticsDictionarySearchNew(this);");
                alphaListBox.WebAnalyticsFunction = "NCIAnalytics.GeneticsDictionarySearchAlphaList"; // Load A-Z list onclick script
            }
           
        }

        private void SetUpDrugDictionary()
        {
            SetupEnglish();
        }

        protected void btnSearch_OnClick(object sender, EventArgs e)
        {
            SearchStr = AutoComplete1.Text;
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