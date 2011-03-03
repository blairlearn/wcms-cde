using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Xml;
using NCI.Web.CDE;
using NCI.Web.CDE.UI;
using NCI.Web.CancerGov.Apps;
using NCI.Logging;

namespace CancerGov.Web.SnippetTemplates
{
    public partial class DocTitleBlock : AppsBaseUserControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                //Parse Data To Get Information
                XmlDocument doc = new XmlDocument();
                doc.LoadXml(this.SnippetInfo.Data.Trim());

                XmlNode xnTitle = doc.SelectSingleNode("//Title");
                XmlNode titleDisplay = doc.SelectSingleNode("//TitleDisplay");
                XmlNode imageUrl = doc.SelectSingleNode("//ImageUrl");
                //XmlNode displayedDate = doc.SelectSingleNode("//Date");

                String title = PageAssemblyContext.Current.PageAssemblyInstruction.GetField("long_title");
                String audience = string.Empty;
                try
                {
                    audience = PageAssemblyContext.Current.PageAssemblyInstruction.GetField("PDQAudience");
                }
                catch { }
                //String date = displayedDate.InnerText;

                if (titleDisplay != null)
                {
                    switch (titleDisplay.InnerText)
                    {
                        case "DocTitleBlockTitle":
                            {
                                if (xnTitle != null)
                                {
                                    title = xnTitle.InnerText;
                                }
                            }
                            break;
                    }
                }

                if (imageUrl != null)
                {
                    imgImage.ImageUrl = imageUrl.InnerText;
                }

                if (PageAssemblyContext.CurrentDisplayVersion == DisplayVersions.Print ||
                    PageAssemblyContext.CurrentDisplayVersion == DisplayVersions.PrintAll)
                {
                    if (!string.IsNullOrEmpty(audience))
                    {
                        phPrint.Visible = true;
                        litPrintTitle.Text = title;
                        litAudienceTitle.Text = audience;

                        ContentDates contenDates = ((BasePageAssemblyInstruction)PageAssemblyContext.Current.PageAssemblyInstruction).ContentDates;
                        string postedTxt = string.Empty;
                        string updatedTxt = string.Empty;
                        string reviewedTxt = string.Empty;
                        

                        //make code better
                        if (PageDisplayInformation.Language == NCI.Web.CDE.DisplayLanguage.English)
                        {
                            postedTxt = "Posted: ";
                            updatedTxt = "Last Modified: ";
                            reviewedTxt = "Reviewed: ";
                        }
                        else if (PageDisplayInformation.Language == NCI.Web.CDE.DisplayLanguage.Spanish)
                        {
                            postedTxt = "Publicaci&oacute;n: ";
                            updatedTxt = "Actualizado: ";
                            reviewedTxt = "Revisi&oacute;n: ";
                        }

                        string posted = "<img width=\"12\" height=\"15\" border=\"0\" alt=\"\" src=\"/Images/spacer.gif\" /><strong>" +
                            postedTxt + "</strong>" + String.Format("{0:MM/dd/yyyy}", contenDates.FirstPublished);
                        string reviewed =  "<img width=\"12\" height=\"15\" border=\"0\" alt=\"\" src=\"/Images/spacer.gif\" /><strong>" +
                            reviewedTxt + "</strong>"+String.Format("{0:MM/dd/yyyy}", contenDates.LastReviewed);
                        string updated =  "<img width=\"12\" height=\"15\" border=\"0\" alt=\"\" src=\"/Images/spacer.gif\" /><strong>" +
                             updatedTxt + "</strong>" + String.Format("{0:MM/dd/yyyy}", contenDates.LastModified);



                        if (contenDates.DateDisplayMode == DateDisplayModes.All)
                        {
                            litPrintDate.Text = posted + updated + reviewed;
                        }
                        else if (contenDates.DateDisplayMode == DateDisplayModes.UpdatedReviewed)
                        {
                            litPrintDate.Text = updated + reviewed;
                        }
                        else if (contenDates.DateDisplayMode == DateDisplayModes.PostedReviewed)
                        {
                            litPrintDate.Text = posted + reviewed;
                        }
                        else if (contenDates.DateDisplayMode == DateDisplayModes.Reviewed)
                        {
                            litPrintDate.Text = reviewed;
                        }
                        else if (contenDates.DateDisplayMode == DateDisplayModes.PostedUpdated)
                        {
                            litPrintDate.Text = posted + updated;
                        }
                        else if (contenDates.DateDisplayMode == DateDisplayModes.Updated)
                        {
                            litPrintDate.Text = updated;
                        }
                        else if (contenDates.DateDisplayMode == DateDisplayModes.Posted)
                        {
                            litPrintDate.Text = posted;
                        }
                        else
                        {
                            litPrintDate.Text = updated;
                        }
 
                    }
                    else
                    {
                        phPrintNoAudience.Visible = true;
                        litNoAudiencePrintTitle.Text = title;
                    }
                }
                else
                {
                    phWeb.Visible = true;
                    litTitle.Text = title;
                }

            }
            catch (Exception ex)
            {
                Logger.LogError("Docktitle Snippet Control", NCIErrorLevel.Error, ex);               
            }
        }
    }
}