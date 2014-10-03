using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Text.RegularExpressions;

using NCI.Text;

namespace NCI.Web.CDE.UI.SnippetControls
{
    class BreadCrumbSnippet : SnippetControl
    {
        protected string RootPath { get; set; }

        protected string CurrUrl { get { return PageAssemblyContext.Current.PageAssemblyInstruction.GetUrl("PrettyUrl").ToString(); } }

        protected void RenderContents(HtmlTextWriter writer)
        {
            /*
            string sectionPath = PageAssemblyContext.Current.PageAssemblyInstruction.SectionPath;
            SectionDetail details = SectionDetailFactory.GetSectionDetail(sectionPath);

            if (details == null)
            {
                // log error message
                return;
            }

            //if the current page is the landing page of the root navon for the breadcrumbs, I.E. you are viewing the page that would be the root, then DO NOT DRAW ANYTHING.
            if (RootPath == details.ParentPath && details.LandingPageURL == CurrUrl)
            {
                return;
            }
            */

            //Opening UL tag
            writer.AddAttribute(HtmlTextWriterAttribute.Class, "breadcrumbs");
            writer.RenderBeginTag(HtmlTextWriterTag.Ul);
            writer.Write("test breadcrumb text");

            /*
            //Draw parents
            RenderBreadcrumbSections(details, writer);

            //Draw this item if not the landing page of the folder
            if (details.LandingPageURL != CurrUrl)
            {
                writer.RenderBeginTag(HtmlTextWriterTag.Li);
                writer.Write(PageAssemblyContext.Current.PageAssemblyInstruction.GetField("short_title"));
                writer.RenderEndTag();
            }
            */
            writer.RenderEndTag();
        }

        private void RenderBreadcrumbSections(SectionDetail section, HtmlTextWriter writer)
        {
            /*    
                   //Need to add FullPath to section details... this should combine ParentPath with SectionName (including a path separator between them)
                   //Basically we need to check if we should draw this section in the bread crumbs, and the reason we would is because this
                   //section is within the section that is the root of the navon.  Luckily we can do this with string comparisons...
                   if (We the current "section" is with the folder structure of the RootPath of the breadcrumb) {
            */
            //If the section has a parent, attempt to draw it first.        
            if (section.ParentPath != null)
            {
                RenderBreadcrumbSections(section, writer);
                // RenderBreadcrumbLink(section.parent, writer);
            }



            //If the LandingPageURL is not set, DO NOT DRAW A BAD LINK!!!
            if (section.LandingPageURL != null && section.NavTitle != null)
            {
                //Draw this item
                if (section.LandingPageURL == CurrUrl)
                {
                    writer.RenderBeginTag(HtmlTextWriterTag.Li);
                    writer.Write(section.NavTitle);
                    writer.RenderEndTag();
                }
                else
                {
                    writer.RenderBeginTag(HtmlTextWriterTag.Li);
                    writer.AddAttribute(HtmlTextWriterAttribute.Href, section.LandingPageURL);
                    writer.RenderBeginTag(HtmlTextWriterTag.A);
                    writer.Write(section.NavTitle);
                    writer.RenderEndTag();
                    writer.RenderEndTag();
                }
            }

        } // RenderBreadCrumbSections()
    } // BreadCrumbSnippet class
}
