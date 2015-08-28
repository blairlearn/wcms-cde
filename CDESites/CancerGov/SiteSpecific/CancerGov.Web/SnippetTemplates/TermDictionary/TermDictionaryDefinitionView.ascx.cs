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
using NCI.Web.Dictionary;
using NCI.Web.Dictionary.BusinessObjects;
using NCI.Services.Dictionary;
using CancerGov.UI.HTML;

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

        public Language DictionaryLanguage { get; set; }

        public string QueryStringLang { get; set; }

        public string PagePrintUrl { get; set; }

        public int RelatedTermCount { get; set; }

        protected void Page_Load(object sender, EventArgs e)
        {
            dictionarySearchBlock.Dictionary = DictionaryType.Term;
            dictionarySearchBlock.DictionaryURL = PageAssemblyContext.Current.requestedUrl.ToString();
            DictionaryURL = PageAssemblyContext.Current.requestedUrl.ToString();

            GetQueryParams();
            ValidateParams();
            
            DictionaryURLSpanish = DictionaryURL;
            DictionaryURLEnglish = DictionaryURL;
                       
            if (Request.RawUrl.ToLower().Contains("dictionary") && Request.RawUrl.ToLower().Contains("spanish"))
            {
                Response.Redirect("/diccionario" + Request.Url.Query);
            }

            if (PageAssemblyContext.Current.PageAssemblyInstruction.Language == "es")
            {
                DictionaryLanguage = Language.Spanish;
            }
            else
                DictionaryLanguage = Language.English;
                       
            
            if (!Page.IsPostBack)
            {
                DictionaryAppManager _dictionaryAppManager = new DictionaryAppManager();
                
                TermReturn dataItem = _dictionaryAppManager.GetTerm(Convert.ToInt32(CdrID), NCI.Services.Dictionary.DictionaryType.term, DictionaryLanguage, "v1");
                if (dataItem != null && dataItem.Term.Term != null)
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
                else
                {
                    termDictionaryDefinitionView.Visible = false;
                }
            }

            SetupPrintUrl();
                                   
        }

        private void ActivateDefinitionView(TermReturn dataItem)
        {

            var myDataSource = new List<TermReturn> { dataItem };

            termDictionaryDefinitionView.Visible = true;
            termDictionaryDefinitionView.DataSource = myDataSource;
            termDictionaryDefinitionView.DataBind();

            string termName = dataItem.Term.Term;

            CdrID = dataItem.Term.ID.ToString();

            if (DictionaryLanguage == Language.Spanish)
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

            PageAssemblyContext.Current.PageAssemblyInstruction.AddFieldFilter("meta_description", (name, data) =>
            {
                data.Value = "Definition of " + termName;
            });


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
                TermReturn termDetails = (TermReturn)e.Item.DataItem;

                if (termDetails != null)
                {
                    //Get Related Information from the Manager layer
                    //Add check to see if it exists and then display data accordingly
                    Panel pnlRelatedInfo = e.Item.FindControl("pnlRelatedInfo") as Panel;
                    if (pnlRelatedInfo != null)
                    {
                        //display the related information panel
                        //when atleast one of the related item exists
                        if (termDetails.Term.Related.Term.Length > 0 ||
                            termDetails.Term.Related.Summary.Length > 0 ||
                            termDetails.Term.Related.DrugSummary.Length > 0 ||
                            termDetails.Term.Related.External.Length > 0 ||
                            termDetails.Term.Images.Length > 0)
                        {
                            pnlRelatedInfo.Visible = true;
                            Literal litMoreInformation = e.Item.FindControl("litMoreInformation") as Literal;
                            if (litMoreInformation != null)
                            {
                                if (DictionaryLanguage == Language.Spanish)
                                    litMoreInformation.Text = "M&aacute;s informaci&oacute;n";
                                else
                                    litMoreInformation.Text = "More Information";
                            }

                            if (termDetails.Term.Related.External.Length > 0)
                            {
                                Repeater relatedExternalRefs = (Repeater)e.Item.FindControl("relatedExternalRefs");
                                if (relatedExternalRefs != null)
                                {
                                    relatedExternalRefs.Visible = true;
                                    relatedExternalRefs.DataSource = termDetails.Term.Related.External;
                                    relatedExternalRefs.DataBind();
                                }
                            }

                            if (termDetails.Term.Related.Summary.Length > 0)
                            {
                                Repeater relatedSummaryRefs = (Repeater)e.Item.FindControl("relatedSummaryRefs");
                                if (relatedSummaryRefs != null)
                                {
                                    relatedSummaryRefs.Visible = true;
                                    relatedSummaryRefs.DataSource = termDetails.Term.Related.Summary;
                                    relatedSummaryRefs.DataBind();
                                }
                            }

                            if (termDetails.Term.Related.DrugSummary.Length > 0)
                            {
                                Repeater relatedDrugInfoSummaries = (Repeater)e.Item.FindControl("relatedDrugInfoSummaries");
                                if (relatedDrugInfoSummaries != null)
                                {
                                    relatedDrugInfoSummaries.Visible = true;
                                    relatedDrugInfoSummaries.DataSource = termDetails.Term.Related.DrugSummary;
                                    relatedDrugInfoSummaries.DataBind();
                                }
                            }

                            if (termDetails.Term.Related.Term.Length > 0)
                            {
                                RelatedTermCount = termDetails.Term.Related.Term.Length;
                                PlaceHolder phRelatedTerms = (PlaceHolder)e.Item.FindControl("phRelatedTerms");
                                if (phRelatedTerms != null)
                                {                                    
                                    phRelatedTerms.Visible = true;
                                    Label labelDefintion = (Label)e.Item.FindControl("labelDefintion");
                                    if (labelDefintion != null)
                                    {
                                        if (DictionaryLanguage == Language.Spanish)
                                            labelDefintion.Text = "Definici&oacute;n de:";
                                        else
                                            labelDefintion.Text = "Definition of:";
                                    }
                                    Repeater relatedTerms = (Repeater)e.Item.FindControl("relatedTerms");
                                    if (relatedTerms != null)
                                    {
                                        relatedTerms.DataSource = termDetails.Term.Related.Term;
                                        relatedTerms.DataBind();
                                    }
                                }

                            }

                            Repeater relatedImages = (Repeater)e.Item.FindControl("relatedImages");
                            if (relatedImages != null)
                            {
                                if (termDetails.Term.Images.Length > 0)
                                {
                                    relatedImages.Visible = true;
                                    relatedImages.DataSource = termDetails.Term.Images;
                                    relatedImages.DataBind();
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
                            string[] regularTermImage = imageDetails.Filename.Split('.');
                            if (regularTermImage.Length == 2)
                            {
                                //termImage image size is 571
                                //example format CDR526538-571.jpg
                                termImage.Src = ConfigurationSettings.AppSettings["CDRImageLocation"] + regularTermImage[0] + "-" + ConfigurationSettings.AppSettings["CDRImageRegular"] + "." +regularTermImage[1];

                                System.Web.UI.HtmlControls.HtmlAnchor termEnlargeImage = (System.Web.UI.HtmlControls.HtmlAnchor)e.Item.FindControl("termEnlargeImage");
                                if (termEnlargeImage != null)
                                {
                                    //enlarge image size is 750
                                    //example format CDR526538-750.jpg
                                    termEnlargeImage.HRef = ConfigurationSettings.AppSettings["CDRImageLocation"] + regularTermImage[0] + "-" + ConfigurationSettings.AppSettings["CDRImageEnlarge"] + "." + regularTermImage[1];
                                }
                            }
                                                                                    
                        }
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
    }
}