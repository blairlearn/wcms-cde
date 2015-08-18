using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Xml.Linq;
using NCI.Web.CDE.UI;
using NCI.Web.CDE.WebAnalytics;
using NCI.Web.CDE.UI.SnippetControls;
using NCI.Web.UI.WebControls;
using NCI.Web.CDE;
using NCI.Web;
using CancerGov.Text;
using CancerGov.Common;
using CancerGov.CDR.TermDictionary;
using CancerGov.Web.SnippetTemplates;
using CancerGov.Common.ErrorHandling;
using System.Configuration;
using NCI.Web.CDE.Modules;
using System.Data;

namespace CancerGov.Web.SnippetTemplates
{
    public partial class TermDictionaryDefinitionView : SnippetControl
    {
        public string SearchStr { get; set; }

        public string Expand { get; set; }

        public string CdrID { get; set; }

        public string SrcGroup { get; set; }

        public bool BContains { get; set; }

        public string DictionaryURLSpanish { get; set; }

        public string DictionaryURLEnglish { get; set; }

        public string DictionaryURL { get; set; }

        public DisplayLanguage Language { get; set; }

        public string QueryStringLang { get; set; }

        public string PagePrintUrl { get; set; }

        protected void Page_Load(object sender, EventArgs e)
        {
            dictionarySearchBlock.Dictionary = DictionaryType.Term;
            dictionarySearchBlock.DictionaryURL = PageAssemblyContext.Current.requestedUrl.ToString();
            DictionaryURL = PageAssemblyContext.Current.requestedUrl.ToString();
            //base.OnLoad(e);
            GetQueryParams();
            ValidateParams();
            
            /* Setup URLS -
             * The URLs are being set in the Appmodule page content item, which publishes the following XML:
             * <cde:Module_dictionaryURL ... >
             *  <DictionaryEnglishURL/> //English dictionary path
             *  <DictionarySpanishURL/> //Spanish dictionary path
             * </cde:Module_dictionaryURL>
            */
            //string snippetXmlData = string.Empty;
            //snippetXmlData = SnippetInfo.Data;
            //snippetXmlData = snippetXmlData.Replace("]]ENDCDATA", "]]>");
            //NCI.Web.CDE.Modules.DictionaryURL dUrl = ModuleObjectFactory<NCI.Web.CDE.Modules.DictionaryURL>.GetModuleObject(snippetXmlData);

            DictionaryURLSpanish = DictionaryURL;
            DictionaryURLEnglish = DictionaryURL;

            //DictionaryURL = DictionaryURLEnglish;

            if (Request.RawUrl.ToLower().Contains("dictionary") && Request.RawUrl.ToLower().Contains("spanish"))
            {
                Response.Redirect("/diccionario" + Request.Url.Query);
            }

            if (PageAssemblyContext.Current.PageAssemblyInstruction.Language == "es")
            {
                Language = DisplayLanguage.Spanish;
            }
            else
                Language = DisplayLanguage.English;

            if (!Page.IsPostBack)
            {
                TermDictionaryDataItem dataItem = TermDictionaryManager.GetDefinitionByTermID(Language.ToString(), CdrID, null, 5);
                if (dataItem != null)
                {
                    ActivateDefinitionView(dataItem);
                    // Web Analytics *************************************************
                    if (WebAnalyticsOptions.IsEnabled)
                    {
                        // Add dictionary term view event to analytics
                        this.PageInstruction.SetWebAnalytics(WebAnalyticsOptions.Events.event11, wbField =>
                        {
                            wbField.Value = null;
                        });
                    }
                }
            }

            SetupPrintUrl();
            SetupCanonicalUrls(DictionaryURLEnglish, DictionaryURLSpanish);
                        
        }

        private void ActivateDefinitionView(TermDictionaryDataItem dataItem)
        {
            var myDataSource = new List<TermDictionaryDataItem> { dataItem };

            termDictionaryDefinitionView.DataSource = myDataSource;
            termDictionaryDefinitionView.DataBind();

            string termName = string.Empty;
            string termPronun = string.Empty;
            string defHtml = string.Empty;
            string imageHtml = string.Empty;
            string audioMediaHtml = string.Empty;
            string relatedLinkInfo = String.Empty;

            CdrID = dataItem.GlossaryTermID.ToString();
            termName = dataItem.TermName;
            termPronun = dataItem.TermPronunciation;
            defHtml = dataItem.DefinitionHTML;
            imageHtml = (dataItem.MediaHTML == null) ? string.Empty : dataItem.MediaHTML;
            imageHtml = imageHtml.Replace("[__imagelocation]", ConfigurationSettings.AppSettings["CDRImageLocation"]);

            audioMediaHtml = (dataItem.AudioMediaHTML == null) ? string.Empty : dataItem.AudioMediaHTML;
            audioMediaHtml = audioMediaHtml.Replace("[_audioMediaLocation]", ConfigurationSettings.AppSettings["CDRAudioMediaLocation"]);
            audioMediaHtml = audioMediaHtml.Replace("[_flashMediaLocation]", ConfigurationSettings.AppSettings["FlashMediaLocation"]);

            relatedLinkInfo = (dataItem.RelatedInfoHTML == null) ? string.Empty : dataItem.RelatedInfoHTML;

            if (Language == DisplayLanguage.Spanish)
            {
                PageAssemblyContext.Current.PageAssemblyInstruction.AddFieldFilter("short_title", (name, data) =>
                {
                    data.Value = "Definici&oacute;n de " + termName + " - Diccionario de c&aacute;ncer";
                });

                this.Page.Header.Title = PageAssemblyContext.Current.PageAssemblyInstruction.GetField("short_title");
            }
            else
            {
                PageAssemblyContext.Current.PageAssemblyInstruction.AddFieldFilter("short_title", (name, data) =>
                {
                    data.Value = "Definition of " + termName + " - NCI Dictionary of Cancer Terms";
                });

                this.Page.Header.Title = PageAssemblyContext.Current.PageAssemblyInstruction.GetField("short_title");
                //lblTermPronun.Text = termPronun;
            }

            PageAssemblyContext.Current.PageAssemblyInstruction.AddFieldFilter("meta_description", (name, data) =>
            {
                data.Value = "Definition of " + termName;
            });


            PageAssemblyContext.Current.PageAssemblyInstruction.AddFieldFilter("meta_keywords", (name, data) =>
            {
                data.Value = termName + ", definition";
            });

            if (imageHtml != string.Empty)
                imageHtml = "<p>" + imageHtml;

            //lblTermName.Text = termName;
            //litDefHtml.Text = defHtml;
            //litImageHtml.Text = imageHtml;
            //litAudioMediaHtml.Text = audioMediaHtml;

            //if (!string.IsNullOrEmpty(relatedLinkInfo))
            //{
            //    pnlRelatedInfo.Visible = true;
            //    litRelatedLinkInfo.Text = relatedLinkInfo;
            //}
            //else
            //    pnlRelatedInfo.Visible = false;

           // RenderLangButtons();
            // 
        }


        /**
         * Add URL filter for old print page implementation
         * @deprecated
         */
        private void SetupPrintUrl()
        {
            PagePrintUrl = "?print=1";

            //add expand
            if (!string.IsNullOrEmpty(Expand))
            {
                if (Expand.Trim() == "#")
                {
                    PagePrintUrl += "&expand=%23";
                }
                else
                {
                    PagePrintUrl += "&expand=" + Expand.Trim().ToUpper();
                }
            }

            //Language stuff
            PagePrintUrl += QueryStringLang;

            //add cdrid or searchstr
            if (!string.IsNullOrEmpty(CdrID))
            {
                PagePrintUrl += "&cdrid=" + CdrID;
            }
            else if (!string.IsNullOrEmpty(SearchStr))
            {
                PagePrintUrl += "&search=" + SearchStr;
                if (BContains)
                    PagePrintUrl += "&contains=true";
            }

            PageAssemblyContext.Current.PageAssemblyInstruction.AddUrlFilter("Print", (name, url) =>
            {
                url.SetUrl(PageAssemblyContext.Current.PageAssemblyInstruction.GetUrl("CurrentURL").ToString() + "/" + PagePrintUrl);
            });
        }

        /**
        * Add a filter for the Canonical URL.
        * The Canonical URL includes query parameters if they exist.
        */
        private void SetupCanonicalUrls(string englishDurl, string spanishDurl)
        {
            PageAssemblyContext.Current.PageAssemblyInstruction.AddUrlFilter(PageAssemblyInstructionUrls.CanonicalUrl, (name, url) =>
            {
                if (CdrID != "")
                    url.SetUrl(url.ToString() + "?cdrid=" + CdrID);
                else if (Expand != "")
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

        protected void termDictionaryDefinitionView_OnItemDataBound(object sender, RepeaterItemEventArgs e)
        { 
            
            //Get Related Information from the Manager layer
            //Add check to see if it exists and then display data accordingly
           
            Literal litMoreInformation = e.Item.FindControl("litMoreInformation") as Literal;
            if (litMoreInformation != null)
            {
                if (Language == DisplayLanguage.Spanish)
                    litMoreInformation.Text = "M&aacute;s informaci&oacute;n";
                else
                    litMoreInformation.Text = "More Information";
            }
            //Based on the old code in GateKeeper the display order for related information is:
            //External Refs
            //Summary Refs
            //Drug info summary refs
            //Related terms
            //if (lang == Language.English)
            //    url = String.Format("/dictionary?CdrID={0}", documentId);
            //else if (lang == Language.Spanish && relatedTerm.HasSpanishTerm)
            //    url = String.Format("/diccionario?CdrID={0}", documentId);

            //String relatedDictionaryLinks = string.Empty;
            //String dictionaryFmt = @"<a href=""{0}"">{1}</a>";  // First list entry.
            //String dictionaryFmtNext = @", <a href=""{0}"">{1}</a>"; // Additiional entry w/ leading comma.

            //// List of related pages.
            //String relatedPageLinks = string.Empty;
            //foreach (RelatedInformationLink ri in def.RelatedInformationList)
            //{
            //    // Glossary terms go in a list for separate rendering, everything gets rendered into an <li>.
            //    if (ri.LinkType == RelatedInformationLink.RelatedLinkType.GlossaryTerm)
            //    {
            //        relatedDictionaryLinks += string.Format(dictionaryFmt, ri.Url, ri.Name);
            //        dictionaryFmt = dictionaryFmtNext;  // Sleight of hand for formatting.
            //    }
            //    else
            //        relatedPageLinks += string.Format("<li><a href=\"{0}\">{1}</a></li>", ri.Url, ri.Name);
            //}

            //// Do the actual rendering part.  Again, not enough to really justify a StringBuilder.
            //if (def.RelatedInformationList.Count > 0)
            //{
            //    relatedInformationHtml = "<div class=\"related-resources\"><h6>" + langMoreinfo + "</h6>";

            //    // General list of related items.
            //    if (!string.IsNullOrEmpty(relatedPageLinks))
            //        relatedInformationHtml += String.Format("<ul class=\"no-bullets\">{0}</ul>", relatedPageLinks);
            //    // Related glossary terms.
            //    if (!string.IsNullOrEmpty(relatedDictionaryLinks))
            //        relatedInformationHtml += String.Format("<p><span class=\"related-definition-label\">{1}</span> {0}.</p>", relatedDictionaryLinks, langDefinitionLabel);
            //    relatedInformationHtml += "</div>";
            //}

            //use this for Genetics dictionary 
            //  // Health Professional is always English, always /geneticsdictionary.
            //String url = String.Format("/geneticsdictionary?cdrid={0}", documentId);
            //relInfoLink = new RelatedInformationLink(termName, url, Language.English, RelatedInformationLink.RelatedLinkType.GlossaryTerm);
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
    }
}