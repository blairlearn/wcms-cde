using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Xml;
using NCI.Web.CDE;
using NCI.Web.CDE.UI;
using NCI.Web.CDE.Modules;

using NCI.Web.CancerGov.Apps;
using NCI.Logging;

namespace CancerGov.Web.SnippetTemplates
{
    public partial class DocTitleBlock : AppsBaseUserControl
    {
        NCI.Web.CDE.Modules.DockTitleBlock moduleData = null;
        string title = string.Empty;

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {

                title = PageAssemblyContext.Current.PageAssemblyInstruction.GetField("long_title");

                String audience = string.Empty;
                try
                {
                    audience = PageAssemblyContext.Current.PageAssemblyInstruction.GetField("PDQAudience");
                }
                catch { }

                // Use titleDisplay to decide what content should be displayed.
                if (!string.IsNullOrEmpty(ModuleData.TitleDisplay))
                {
                    switch (ModuleData.TitleDisplay)
                    {
                        case "DocTitleBlockTitle":
                            if (ModuleData.Title != null)
                            {
                                title = ModuleData.Title;
                            }
                            break;
                    }
                }

                if (PageAssemblyContext.CurrentDisplayVersion == DisplayVersions.Print ||
                    PageAssemblyContext.CurrentDisplayVersion == DisplayVersions.PrintAll)
                {
                    #region Print Version
                    if (!string.IsNullOrEmpty(audience))
                    {
                        phPrint.Visible = true;
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
                        string posted;
                        string reviewed;
                        string updated;

                        if (PageDisplayInformation.Language == NCI.Web.CDE.DisplayLanguage.Spanish)
                        {
                            posted = "<img width=\"12\" height=\"15\" border=\"0\" alt=\"\" src=\"/Images/spacer.gif\" /><strong>" +
                                postedTxt + "</strong>" + String.Format("{0:MM/dd/yyyy}", CovertToSpanishFormat(contenDates.FirstPublished));
                            reviewed = "<img width=\"12\" height=\"15\" border=\"0\" alt=\"\" src=\"/Images/spacer.gif\" /><strong>" +
                                reviewedTxt + "</strong>" + String.Format("{0:MM/dd/yyyy}", CovertToSpanishFormat(contenDates.LastReviewed));
                            updated = "<img width=\"12\" height=\"15\" border=\"0\" alt=\"\" src=\"/Images/spacer.gif\" /><strong>" +
                                 updatedTxt + "</strong>" + String.Format("{0:MM/dd/yyyy}", CovertToSpanishFormat(contenDates.LastModified));
                        }

                        else
                        {
                            posted = "<img width=\"12\" height=\"15\" border=\"0\" alt=\"\" src=\"/Images/spacer.gif\" /><strong>" +
                                postedTxt + "</strong>" + String.Format("{0:MM/dd/yyyy}", contenDates.FirstPublished);
                            reviewed = "<img width=\"12\" height=\"15\" border=\"0\" alt=\"\" src=\"/Images/spacer.gif\" /><strong>" +
                                reviewedTxt + "</strong>" + String.Format("{0:MM/dd/yyyy}", contenDates.LastReviewed);
                            updated = "<img width=\"12\" height=\"15\" border=\"0\" alt=\"\" src=\"/Images/spacer.gif\" /><strong>" +
                                 updatedTxt + "</strong>" + String.Format("{0:MM/dd/yyyy}", contenDates.LastModified);

                        }



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
                    }
                    #endregion
                }
                else
                {
                    #region Web Version
                    phWeb.Visible = true;
                    // Image displayed only for web version.
                    imgImage.ImageUrl = ModuleData.ImageUrl;
                    #endregion
                }

            }
            catch (Exception ex)
            {
                Logger.LogError("Docktitle Snippet Control", NCIErrorLevel.Error, ex);
            }
        }

        protected string TblColor
        {
            get{ return string.IsNullOrEmpty(ModuleData.TableColor) == true ? "#d4d9d9" : ModuleData.TableColor; }
        }

        protected string Title
        {
            get { return title; }
        }
        protected NCI.Web.CDE.Modules.DockTitleBlock ModuleData
        {
            get
            {
                if (moduleData == null)
                {
                    string snippetXmlData = this.SnippetInfo.Data;
                    snippetXmlData = snippetXmlData.Replace("]]ENDCDATA", "]]>");
                    //Parse Data To Get Information
                    moduleData = ModuleObjectFactory<NCI.Web.CDE.Modules.DockTitleBlock>.GetModuleObject(snippetXmlData);
                }
                return moduleData;
            }
        }

        #region Private Members
        private string CovertToSpanishFormat(DateTime Date)
        {
            string spanishDate = string.Empty;

            spanishDate = Date.Day + " de " + GetSpanishMonth(Date.Month) + " de " + Date.Year;

            return spanishDate;
        }
        private string GetSpanishMonth(int month)
        {
            string spanishMonth = string.Empty;
            switch (month)
            {
                case 1:
                    spanishMonth = "enero";
                    break;
                case 2:
                    spanishMonth = "febrero";
                    break;
                case 3:
                    spanishMonth = "marzo";
                    break;
                case 4:
                    spanishMonth = "abril";
                    break;
                case 5:
                    spanishMonth = "mayo";
                    break;
                case 6:
                    spanishMonth = "junio";
                    break;
                case 7:
                    spanishMonth = "julio";
                    break;
                case 8:
                    spanishMonth = "agosto";
                    break;
                case 9:
                    spanishMonth = "septiembre";
                    break;
                case 10:
                    spanishMonth = "octubre";
                    break;
                case 11:
                    spanishMonth = "noviembre";
                    break;
                case 12:
                    spanishMonth = "diciembre";
                    break;
                default:
                    spanishMonth = "";
                    break;
            }

            return spanishMonth;
        }
        #endregion
    } 
   
}