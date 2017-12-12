using System;
using System.Collections.Generic;
using System.Configuration;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

using CancerGov.Text;
using Common.Logging;

using NCI.Web.CDE;
using NCI.Web.CDE.UI;
using NCI.Web.CDE.WebAnalytics;
using NCI.Web.Dictionary;
using NCI.Web.Dictionary.BusinessObjects;
using CancerGov.Dictionaries.SnippetControls.Helpers;

namespace CancerGov.Dictionaries.SnippetControls.GeneticsTermDictionary
{
    public class GeneticsTermDictionaryDefintionView : SnippetControl
    {
        protected GeneticsTermDictionaryHome dictionarySearchBlock;

        protected Repeater termDictionaryDefinitionView;

        static ILog log = LogManager.GetLogger(typeof(GeneticsTermDictionaryDefintionView));

        public string SearchStr { get; set; }

        public string Expand { get; set; }

        public string CdrID { get; set; }

        public string SrcGroup { get; set; }

        public bool BContains { get; set; }

        public string DictionaryURLSpanish { get; set; }

        public string DictionaryURLEnglish { get; set; }

        public string DictionaryURL { get; set; }

        public String DictionaryLanguage { get; set; }

        public string QueryStringLang { get; set; }

        public string PagePrintUrl { get; set; }

        public int RelatedTermCount { get; set; }

        protected void Page_Load(object sender, EventArgs e)
        {
            DictionaryURL = PageAssemblyContext.Current.requestedUrl.ToString();

            GetQueryParams();
            ValidateParams();

            //For Genetics dictionary language is always English
            DictionaryLanguage = "en";

            if (!Page.IsPostBack)
            {
                DictionaryAppManager _dictionaryAppManager = new DictionaryAppManager();

                DictionaryTerm dataItem = _dictionaryAppManager.GetTerm(Convert.ToInt32(CdrID), NCI.Web.Dictionary.DictionaryType.genetic, DictionaryLanguage, "v1");
                if (dataItem != null && dataItem.Term != null)
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

                    PageInstruction.AddFieldFilter("browser_title", (name, data) =>
                    {
                        data.Value = "Definition of " + dataItem.Term + " - NCI Dictionary of Genetics Terms";
                    });

                }
                else
                {
                    termDictionaryDefinitionView.Visible = false;
                }
            }

            SetupPrintUrl();
        }

        private void ActivateDefinitionView(DictionaryTerm dataItem)
        {
            var myDataSource = new List<DictionaryTerm> { dataItem };

            termDictionaryDefinitionView.Visible = true;
            termDictionaryDefinitionView.DataSource = myDataSource;
            termDictionaryDefinitionView.DataBind();

            string termName = dataItem.Term;

            CdrID = dataItem.ID.ToString();

            if (DictionaryLanguage == "es")
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
            }

            DictionaryDefinitionHelper.SetMetaTagDescription(dataItem, DictionaryLanguage, PageInstruction);

            PageAssemblyContext.Current.PageAssemblyInstruction.AddFieldFilter("meta_keywords", (name, data) =>
            {
                data.Value = termName + ", definition";
            });
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

        protected void termDictionaryDefinitionView_OnItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                //get the TermReturn object that is bound to the current row.
                DictionaryTerm termDetails = (DictionaryTerm)e.Item.DataItem;

                if (termDetails != null)
                {
                    PlaceHolder phPronunciation = (PlaceHolder)e.Item.FindControl("phPronunciation");
                    if (termDetails.HasPronunciation && phPronunciation != null)
                    {
                        phPronunciation.Visible = true;
                        System.Web.UI.HtmlControls.HtmlAnchor pronunciationLink = (System.Web.UI.HtmlControls.HtmlAnchor)e.Item.FindControl("pronunciationLink");
                        if (pronunciationLink != null && termDetails.Pronunciation.HasAudio)
                        {
                            pronunciationLink.Visible = true;
                            pronunciationLink.HRef = termDetails.Pronunciation.Audio;
                        }
                        else
                            pronunciationLink.Visible = false;

                        Literal pronunciationKey = (Literal)e.Item.FindControl("pronunciationKey");
                        if (pronunciationKey != null && termDetails.Pronunciation.HasKey)
                            pronunciationKey.Text = " " + termDetails.Pronunciation.Key;
                    }
                    else
                        phPronunciation.Visible = false;

                    //Get Related Information from the Manager layer
                    //Add check to see if it exists and then display data accordingly
                    Panel pnlRelatedInfo = e.Item.FindControl("pnlRelatedInfo") as Panel;
                    if (pnlRelatedInfo != null)
                    {
                        //display the related information panel
                        //when atleast one of the related item exists
                        if (termDetails.Related.Term.Length > 0 ||
                            termDetails.Related.Summary.Length > 0 ||
                            termDetails.Related.DrugSummary.Length > 0 ||
                            termDetails.Related.External.Length > 0 ||
                            termDetails.Images.Length > 0)
                        {
                            pnlRelatedInfo.Visible = true;
                            Literal litMoreInformation = e.Item.FindControl("litMoreInformation") as Literal;
                            if (litMoreInformation != null)
                            {
                                //don't display more information text when only images are being displayed
                                if (termDetails.Related.Term.Length > 0 ||
                                termDetails.Related.Summary.Length > 0 ||
                                termDetails.Related.DrugSummary.Length > 0 ||
                                termDetails.Related.External.Length > 0)
                                {
                                    litMoreInformation.Visible = true;
                                    if (DictionaryLanguage == "es")
                                        litMoreInformation.Text = "M&aacute;s informaci&oacute;n";
                                    else
                                        litMoreInformation.Text = "More Information";
                                }
                                else
                                {
                                    litMoreInformation.Visible = false;
                                }
                            }

                            if (termDetails.Related.External.Length > 0)
                            {
                                Repeater relatedExternalRefs = (Repeater)e.Item.FindControl("relatedExternalRefs");
                                if (relatedExternalRefs != null)
                                {
                                    relatedExternalRefs.Visible = true;
                                    relatedExternalRefs.DataSource = termDetails.Related.External;
                                    relatedExternalRefs.DataBind();
                                }
                            }

                            if (termDetails.Related.Summary.Length > 0)
                            {
                                Repeater relatedSummaryRefs = (Repeater)e.Item.FindControl("relatedSummaryRefs");
                                if (relatedSummaryRefs != null)
                                {
                                    relatedSummaryRefs.Visible = true;
                                    relatedSummaryRefs.DataSource = termDetails.Related.Summary;
                                    relatedSummaryRefs.DataBind();
                                }
                            }

                            if (termDetails.Related.DrugSummary.Length > 0)
                            {
                                Repeater relatedDrugInfoSummaries = (Repeater)e.Item.FindControl("relatedDrugInfoSummaries");
                                if (relatedDrugInfoSummaries != null)
                                {
                                    relatedDrugInfoSummaries.Visible = true;
                                    relatedDrugInfoSummaries.DataSource = termDetails.Related.DrugSummary;
                                    relatedDrugInfoSummaries.DataBind();
                                }
                            }

                            if (termDetails.Related.Term.Length > 0)
                            {
                                RelatedTermCount = termDetails.Related.Term.Length;
                                PlaceHolder phRelatedTerms = (PlaceHolder)e.Item.FindControl("phRelatedTerms");
                                if (phRelatedTerms != null)
                                {
                                    phRelatedTerms.Visible = true;
                                    Label labelDefintion = (Label)e.Item.FindControl("labelDefintion");
                                    if (labelDefintion != null)
                                    {
                                        if (DictionaryLanguage == "es")
                                            labelDefintion.Text = "Definici&oacute;n de:";
                                        else
                                            labelDefintion.Text = "Definition of:";
                                    }
                                    Repeater relatedTerms = (Repeater)e.Item.FindControl("relatedTerms");
                                    if (relatedTerms != null)
                                    {
                                        relatedTerms.DataSource = termDetails.Related.Term;
                                        relatedTerms.DataBind();
                                    }
                                }
                            }

                            Repeater relatedImages = (Repeater)e.Item.FindControl("relatedImages");
                            if (relatedImages != null)
                            {
                                if (termDetails.Images.Length > 0)
                                {
                                    relatedImages.Visible = true;
                                    relatedImages.DataSource = termDetails.Images;
                                    relatedImages.DataBind();
                                }

                            }

                            Repeater relatedVideos = (Repeater)e.Item.FindControl("relatedVideos");
                            if (relatedVideos != null)
                            {
                                if (termDetails.HasVideos && termDetails.Videos.Length > 0)
                                {
                                    relatedVideos.Visible = true;
                                    relatedVideos.DataSource = termDetails.Videos;
                                    relatedVideos.DataBind();
                                }
                            }
                        }
                        else
                        {
                            pnlRelatedInfo.Visible = false;
                        }
                    }
                }
            }
        }

        protected void relatedTerms_OnItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                //get the RelatedTerm object that is bound to the current row.
                RelatedTerm relatedTerm = (RelatedTerm)e.Item.DataItem;
                if (relatedTerm != null)
                {
                    HyperLink relatedTermLink = (HyperLink)e.Item.FindControl("relatedTermLink");
                    if (relatedTermLink != null)
                    {
                        relatedTermLink.NavigateUrl = DictionaryURL + "?cdrid=" + relatedTerm.Termid;
                        relatedTermLink.Text = relatedTerm.Text;

                        //make sure the comma is only displayed when there is more than one related term
                        Literal relatedTermSeparator = (Literal)e.Item.FindControl("relatedTermSeparator");
                        if (relatedTermSeparator != null)
                        {
                            if (e.Item.ItemIndex >= 0 && e.Item.ItemIndex < RelatedTermCount - 1)
                                relatedTermSeparator.Visible = true;
                        }
                    }
                }
            }
        }

        protected void relatedImages_OnItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                //get the ImageReference object that is bound to the current row.
                ImageReference imageDetails = (ImageReference)e.Item.DataItem;

                if (imageDetails != null)
                {
                    System.Web.UI.HtmlControls.HtmlImage termImage = (System.Web.UI.HtmlControls.HtmlImage)e.Item.FindControl("termImage");
                    if (termImage != null)
                    {
                        termImage.Alt = imageDetails.AltText;

                        if (!string.IsNullOrEmpty(imageDetails.Filename))
                        {
                            System.Web.UI.HtmlControls.HtmlAnchor termEnlargeImage = (System.Web.UI.HtmlControls.HtmlAnchor)e.Item.FindControl("termEnlargeImage");

                            //if either the regular image size or the enlarge image size is not in the config file
                            //default to the full image in the database
                            if (string.IsNullOrEmpty(ConfigurationManager.AppSettings["CDRImageRegular"]) || string.IsNullOrEmpty(ConfigurationManager.AppSettings["CDRImageEnlarge"]))
                            {
                                termImage.Src = imageDetails.Filename;

                                if (termEnlargeImage != null)
                                    termEnlargeImage.HRef = imageDetails.Filename;

                                //log a warning
                                log.WarnFormat("Web.Config file does not specify image sizes for term id: {0}. Display full image.", CdrID);
                            }
                            else
                            {
                                string[] regularTermImage = imageDetails.Filename.Split('.');
                                if (regularTermImage.Length == 2)
                                {
                                    //termImage image size is 571
                                    //example format CDR526538-571.jpg
                                    termImage.Src = regularTermImage[0] + "-" + ConfigurationManager.AppSettings["CDRImageRegular"] + "." + regularTermImage[1];

                                    //enlarge image size is 750
                                    //example format CDR526538-750.jpg
                                    if (termEnlargeImage != null)
                                        termEnlargeImage.HRef = regularTermImage[0] + "-" + ConfigurationManager.AppSettings["CDRImageEnlarge"] + "." + regularTermImage[1];
                                }
                            }
                        }
                    }
                }
            }
        }

        protected void relatedVideos_OnItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                //get the ImageReference object that is bound to the current row.
                VideoReference videoDetails = (VideoReference)e.Item.DataItem;

                if (videoDetails != null)
                {
                    // Set the CSS class for the video's containing figure element.
                    HtmlGenericControl container = (HtmlGenericControl)e.Item.FindControl("videoContainer");
                    if (container != null)
                    {
                        // These are the templates allowed by the CDR's DTD for GlossaryTerm Embedded videos.
                        // Others do exist in Percussion, but are deprecated per OCECDR-3558.
                        switch (videoDetails.Template.ToLowerInvariant())
                        {
                            case "video100notitle":
                            case "video100title":
                                container.Attributes.Add("class", "video center size100");
                                break;

                            case "video50notitle":
                                container.Attributes.Add("class", "video center size50");
                                break;

                            case "video50notitleright":
                            case "video50titleright":
                                container.Attributes.Add("class", "video right size50");
                                break;

                            case "video75notitle":
                            case "video75title":
                                container.Attributes.Add("class", "video center size75");
                                break;
                            default:
                                break;
                        }
                    }

                    // Set up the title display.
                    // Requires a title be present, and not using one of the "NoTitle" templates.
                    if (!String.IsNullOrWhiteSpace(videoDetails.Title) && !videoDetails.Template.ToLowerInvariant().Contains("notitle"))
                    {
                        HtmlGenericControl title = (HtmlGenericControl)e.Item.FindControl("videoTitle");
                        if (title != null)
                        {
                            title.Visible = true;
                            title.InnerText = videoDetails.Title;
                        }
                    }

                    // Display the caption
                    HtmlGenericControl caption = (HtmlGenericControl)e.Item.FindControl("captionContainer");
                    if (caption != null && !String.IsNullOrWhiteSpace(videoDetails.Caption))
                    {
                        caption.Visible = true;
                        caption.InnerHtml = videoDetails.Caption;
                    }
                }
            }
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
                catch (Exception)
                {
                    throw new Exception("Invalid CDRID" + CdrID);
                }
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