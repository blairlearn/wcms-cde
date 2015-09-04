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
using NCI.Web.Dictionary.BusinessObjects;
using NCI.Web.Dictionary;
using NCI.Services.Dictionary;

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

        public Language DictionaryLanguage { get; set; }

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
            DictionaryLanguage = Language.English;

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
            DictionaryAppManager _dictionaryAppManager = new DictionaryAppManager();
            SearchReturn resultList = _dictionaryAppManager.Search("%", SearchType.Begins, 0, int.MaxValue, NCI.Services.Dictionary.DictionaryType.term, DictionaryLanguage);

            if (resultList != null && resultList.Meta != null)
                TotalCount = resultList.Meta.ResultCount;
                        
            //dictionarySearchForm 
            bool _isSpanish = false;
                                   
            if (PageAssemblyContext.Current.PageAssemblyInstruction.Language == "es")
            {
                DictionaryLanguage = Language.Spanish;
                SetupSpanish();
                _isSpanish = true;
            }
            else
            {
                DictionaryLanguage = Language.English;
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

            pnlIntroEnglish.Visible = false;
            pnlIntroSpanish.Visible = true;

            btnSearch.Text = "Buscar";
            btnSearch.ToolTip = "Buscar";

            litTotalCount2.Text = TotalCount.ToString();
           
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

            pnlIntroEnglish.Visible = true;
            pnlIntroSpanish.Visible = false;

            litTotalCount1.Text = TotalCount.ToString();
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