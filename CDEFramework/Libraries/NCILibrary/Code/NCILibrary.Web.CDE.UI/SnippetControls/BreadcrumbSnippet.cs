using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Xml;
using System.Xml.Linq;

using NCI.Logging;
using NCI.Text;


namespace NCI.Web.CDE.UI.SnippetControls
{
    public class BreadcrumbSnippet : SnippetControl
    {
        public int totalCount = 0;
        public int recurseCount = 0;

        // Get root path from SectionDetails.xml
        public string _breadcrumbData = String.Empty;
        public string BreadcrumbData
        {
            get { return _breadcrumbData; }
            set { _breadcrumbData = value; }
        }

        protected string RootPath
        {
            get
            {
                if (!String.IsNullOrEmpty(SnippetInfo.Data))
                {
                    XmlDocument doc = new XmlDocument();
                    string xml = SnippetInfo.Data;

                    try
                    {
                        doc.LoadXml(xml);
                    }
                    catch (Exception ex)
                    {
                        Logger.LogError("NCI.Web.CDE.SectionDetail", "Unable to load XML document", NCIErrorLevel.Error, ex);
                    }

                    XmlNode rootPathNode = doc.DocumentElement;
                    BreadcrumbData = rootPathNode.InnerText.ToString();
                }
                return BreadcrumbData;
            }
        }

        protected string CurrUrl { get { return PageAssemblyContext.Current.PageAssemblyInstruction.GetUrl("PrettyUrl").ToString(); } }

        public override void RenderControl(HtmlTextWriter writer)
        {

            string sectionPath = PageAssemblyContext.Current.PageAssemblyInstruction.SectionPath;
            SectionDetail details = SectionDetailFactory.GetSectionDetail(sectionPath);

            if (details == null)
            {
                NCI.Logging.Logger.LogError("BreadcrumbSnippet", "Section detail cannot be null.", NCIErrorLevel.Error);
                return;
            }

            //if the current page is the landing page of the root navon for the breadcrumbs, I.E. you are viewing the page that would be the root, then DO NOT DRAW ANYTHING.
            if (RootPath == details.ParentPath && details.LandingPageURL == CurrUrl)
            {
                return;
            }

            //Opening UL tag 
            writer.AddAttribute(HtmlTextWriterAttribute.Class, "breadcrumbs");
            writer.RenderBeginTag(HtmlTextWriterTag.Ul);

            //Draw parents
            RenderBreadcrumbSections(details, writer);

            //Close UL tag
            writer.RenderEndTag();
        }

        protected void RenderBreadcrumbSections(SectionDetail section, HtmlTextWriter writer)
        {
            // Get the total number of items in the breadcrumb; this will be used to determine the li for the final breadcrumb item
            if (!String.IsNullOrEmpty(section.LandingPageURL) && !String.IsNullOrEmpty(section.NavTitle) && section.LandingPageURL != CurrUrl)
            {
                totalCount++;
            }

            /* 
             * Add "FullPath" that combines ParentPath with SectionName (including a path separator between them)
             * Basically we need to check if we should draw this section in the bread crumbs, and the reason we would is because this
             * section is within the section that is the root of the navon. 
             */
            string fullPath = section.ParentPath + "/" + section.SectionName;
            if (RootPath != null && fullPath.IndexOf(RootPath) != -1)
            {

                //If the section has a parent, attempt to draw it first.        
                if (section.ParentPath != null)
                {
                    RenderBreadcrumbSections(section.Parent, writer);
                }

                //If the LandingPageURL is not set, DO NOT DRAW A BAD LINK!!!
                if (!String.IsNullOrEmpty(section.LandingPageURL) && !String.IsNullOrEmpty(section.NavTitle) && section.LandingPageURL != CurrUrl)
                {
                    // Increment the count with each pass through the method and update the <li> tag on the last item
                    recurseCount++;
                    if (recurseCount == totalCount)
                    {
                        writer.AddAttribute(HtmlTextWriterAttribute.Class, "last-breadcrumb");
                    }
                    writer.RenderBeginTag(HtmlTextWriterTag.Li);
                    writer.AddAttribute(HtmlTextWriterAttribute.Href, section.LandingPageURL);
                    writer.RenderBeginTag(HtmlTextWriterTag.A);
                    writer.Write(section.NavTitle);
                    writer.RenderEndTag();
                    writer.RenderEndTag();
                }
                else
                {
                    return;
                }
            }
            else
            {
                return;
            } // drawing html
			
        } // RenderBreadbrumbSections()
    } // BreadcrumbSnippet class
}

