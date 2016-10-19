using System;
using System.Web.UI;
using System.Xml;
using Common.Logging;

namespace NCI.Web.CDE.UI.SnippetControls
{
    public class BreadcrumbSnippet : SnippetControl
    {
        static ILog log = LogManager.GetLogger(typeof(BreadcrumbSnippet));

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
                        log.Error("Unable to load XML document", ex);
                    }

                    XmlNode rootPathNode = doc.DocumentElement;
                    BreadcrumbData = rootPathNode.InnerText.ToString();
                }
                return BreadcrumbData;
            }
        }

        protected string CurrUrl { get { return PageAssemblyContext.Current.PageAssemblyInstruction.GetUrl("PrettyUrl").ToString(); } }

        protected int totalCount = 0; // Content depth relative to root
        protected int recurseCount = 0; // Number of visible breadcrumb elements

        // Total depth (to site root) of the navon for the content item's breadcrumb. This will be subtracted from the total depth of the content
        // item to determine the relative level of its navon.
        protected int NavTreeDepth
        {
            get
            {
                int count = 0;
                if (!String.IsNullOrEmpty(BreadcrumbData))
                {
                    count = BreadcrumbData.Split('/').Length - 2;
                }
                return count;
            }
        }


        public override void RenderControl(HtmlTextWriter writer)
        {

            string sectionPath = PageAssemblyContext.Current.PageAssemblyInstruction.SectionPath;
            SectionDetail details = SectionDetailFactory.GetSectionDetail(sectionPath);

            if (details == null)
            {
                log.Error("Section detail cannot be null.");
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
            string fullPath = section.ParentPath + "/" + section.SectionName + "/";
            if (RootPath != null && fullPath.IndexOf(RootPath) != -1)
            {

                //If the section has a parent, attempt to draw it first.        
                if (section.ParentPath != null)
                {
                    RenderBreadcrumbSections(section.Parent, writer);
                }

                //If the LandingPageURL is not set, DO NOT DRAW A BAD LINK!!!
                if (!String.IsNullOrEmpty(section.LandingPageURL) && !String.IsNullOrEmpty(section.NavTitle) && section.LandingPageURL != CurrUrl && totalCount > 1)
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

