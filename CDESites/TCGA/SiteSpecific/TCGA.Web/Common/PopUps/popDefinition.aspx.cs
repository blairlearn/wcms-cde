using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using TCGA.Web;
using System.Web.UI.HtmlControls;
using CancerGov.UI;
using NCI.Web.CDE;
using NCI.Web.Dictionary.BusinessObjects;
using NCI.Web.CDE.WebAnalytics;
using NCI.Web.Dictionary;
using System.Text.RegularExpressions;
using CancerGov.Text;
using System.Configuration;
using System.Diagnostics;

namespace TCGA.Web.Common.PopUps
{
    ///<summary>
    ///Defines frame sources for definition popup<br/>
    ///<br/>
    ///<b>Author</b>:  Greg Andres<br/>
    ///<b>Date</b>:  10-15-2001<br/>
    ///<br/>
    ///<b>Revision History</b>:<br/>
    ///<br/>
    ///</summary>
    public partial class PopDefinition : System.Web.UI.Page
    {
        private string urlArgs = "";
        public string CdrID { get; set; }
        //set the language to English by default
        string dictionaryLanguage = "en";

        protected void Page_Load(object sender, EventArgs e)
        {
            string input_term;
            PDQVersion version;

            if (PageAssemblyContext.Current.DisplayVersion == DisplayVersions.Web)
            {               
                urlArgs = Request.Url.Query.Substring(1);
                input_term = Strings.Clean(Request.Params["term"]);
                CdrID = Strings.IfNull(Strings.Clean(Request.Params["id"]), Strings.Clean(Request.Params["cdrid"]));
                version = PDQVersionResolver.GetPDQVersion(Strings.Clean(Request.Params["version"]));
                                

                //determine the term audience from the PDQVersion
                AudienceType audience;
                switch (version)
                {
                    case PDQVersion.HealthProfessional:
                        audience = AudienceType.HealthProfessional;
                        break;
                    case PDQVersion.Patient:
                        audience = AudienceType.Patient;
                        break;
                    default:
                        audience = AudienceType.Patient;
                        break;
                }                                           

                //load the definition
                DictionaryAppManager _dictionaryAppManager = new DictionaryAppManager();

                DictionaryTerm dataItem = null;

                if (!string.IsNullOrEmpty(CdrID))
                {
                    CdrID = Regex.Replace(CdrID, "^CDR0+", "", RegexOptions.Compiled);
                    //the language is set to English = en by default
                    dataItem = _dictionaryAppManager.GetTermForAudience(Convert.ToInt32(CdrID), dictionaryLanguage, "v1", audience);

                }

                if (dataItem != null && dataItem.Term != null)
                {
                    ActivateDefinitionView(dataItem);
                }

                // Web Analytics *************************************************
                WebAnalyticsPageLoad webAnalyticsPageLoad = new WebAnalyticsPageLoad();
                                
                webAnalyticsPageLoad.SetChannel("Dictionary of Cancer Terms");
                webAnalyticsPageLoad.SetLanguage("en");
                
                webAnalyticsPageLoad.AddEvent(WebAnalyticsOptions.Events.event11); // Dictionary Term view (event11)
                litOmniturePageLoad.Text = webAnalyticsPageLoad.Tag();  // Load page load script 
                // End Web Analytics *********************************************

            }


        }

        private void ActivateDefinitionView(DictionaryTerm dataItem)
        {
            var myDataSource = new List<DictionaryTerm> { dataItem };

            termDictionaryDefinitionView.Visible = true;
            termDictionaryDefinitionView.DataSource = myDataSource;
            termDictionaryDefinitionView.DataBind();


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
                        
                        Label pronunciationKey = (Label)e.Item.FindControl("pronunciationKey");
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
                        if (termDetails.Images.Length > 0)
                        {
                            pnlRelatedInfo.Visible = true;

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
                        }
                        else
                        {
                            pnlRelatedInfo.Visible = false;
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
                            if (string.IsNullOrEmpty(ConfigurationSettings.AppSettings["CDRImageRegular"]) || string.IsNullOrEmpty(ConfigurationSettings.AppSettings["CDRImageEnlarge"]))
                            {
                                termImage.Src = imageDetails.Filename;

                                if (termEnlargeImage != null)
                                {
                                    termEnlargeImage.HRef = imageDetails.Filename;
                                    termEnlargeImage.InnerText = "Enlarge";
                                }

                                //log a warning
                                NCI.Logging.Logger.LogError("TermDictionaryDefinitionView.ascx", "Web.Config file does not specify image sizes for term id: " + CdrID + ". Display full image.", NCI.Logging.NCIErrorLevel.Warning);
                            }
                            else
                            {
                                string[] regularTermImage = imageDetails.Filename.Split('.');
                                if (regularTermImage.Length == 2)
                                {
                                    //termImage image size is 571
                                    //example format CDR526538-571.jpg
                                    termImage.Src = regularTermImage[0] + "-" + ConfigurationSettings.AppSettings["CDRImageRegular"] + "." + regularTermImage[1];

                                    //enlarge image size is 750
                                    //example format CDR526538-750.jpg
                                    if (termEnlargeImage != null)
                                        termEnlargeImage.HRef = regularTermImage[0] + "-" + ConfigurationSettings.AppSettings["CDRImageEnlarge"] + "." + regularTermImage[1];

                                }
                            }

                        }
                    }

                }
            }
        }
        protected void Page_Init(object sender, EventArgs e)
        {
            //
            // CODEGEN: This call is required by the ASP.NET Web Form Designer.
            //
            InitializeComponent();
        }

        #region Web Form Designer generated code
        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {

        }
        #endregion
    }
}
