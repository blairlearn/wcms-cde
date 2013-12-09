using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Text.RegularExpressions;

using NCI.Logging;
using NCI.Text;
using NCI.Web.CDE.UI;
using NCI.Web.CDE;

namespace CancerGov.Web.UI.SnippetControls
{
    /// <summary>
    /// This Snippet Template is for displaying blobs of HTML that Percussion has generated.
    /// </summary>
    [DefaultProperty("Text")]
    [ToolboxData("<{0}:CGovSectionNavControl runat=server></{0}:CGovSectionNavControl>")]
    public class CGovSectionNavControl : SnippetControl
    {
        //My Nav Here
        NavigationDisplayInfo _navItem = null;
        

        public void Page_Load(object sender, EventArgs e)
        {
            //loads the xml structure from the snippet infos
           _navItem = NavigationDisplayInfo.ParseTree(SnippetInfo.Data);
        }

        /// <summary>
        /// Renders the navigation control's HTML.
        /// </summary>
        /// <param name="writer"></param>
        public override void RenderControl(HtmlTextWriter writer)
        {
            base.RenderControl(writer);

            // If the navigation item exists, go ahead and render it.  Otherwise, log an error, but
            // allow the page to render.
            if (_navItem != null)
            {
                // Tricky piece -- it's legitimate for _navItem to exist, but with a NULL _navItem.rootNavItem.
                // The case in point for this is where an empty Main Nav is being used (e.g. Cancer Bulletin.)
                // Since the rootNavItem is what's actually being rendered, we also need to check whether it
                // exists, however no error is logged in that case.
                if (_navItem.rootNavItem != null)
                {
                    //generates the start of the html by creating a div with the class to shade the area
                    writer.AddAttribute(HtmlTextWriterAttribute.Class, "leftnav-shaded-box");
                    writer.RenderBeginTag(HtmlTextWriterTag.Div);
                    writer.RenderBeginTag(HtmlTextWriterTag.H1);
                    writer.AddAttribute(HtmlTextWriterAttribute.Href, _navItem.rootNavItem.URL);
                    writer.RenderBeginTag(HtmlTextWriterTag.A);
                    writer.Write(_navItem.rootNavItem.Title);
                    writer.RenderEndTag();  //end A
                    writer.RenderEndTag();  //end H1

                    //then calls the renderNavtree method with the nav item
                    RenderNavTree(_navItem.rootNavItem, writer);

                    writer.RenderEndTag();  //End Div
                }
            }
            else
            {
                Logger.LogError("CancerGov.Web.UI.SnippetControls.CGovSectionNavControl", "Navigation item is unexpectedly null.", NCIErrorLevel.Error);
            }
        }

        private void RenderNavTree(NavigationItem root, HtmlTextWriter writer)
        {
           
            //calls RenderNavItem assuming there are child nodes for the root and calls it on each child
            if (root.ChildItems.Length > 0)
            {
                
                writer.RenderBeginTag(HtmlTextWriterTag.Ul);
                foreach (NavigationItem item in root.ChildItems)
                {
                    RenderNavItem(item, writer);
            
                }
                writer.RenderEndTag();//end Ul
            }
          
        }

        private void RenderNavItem(NavigationItem item, HtmlTextWriter writer)
        {
            //gets the current webpage Section path to use for comparisons
            String path = PageAssemblyContext.Current.PageAssemblyInstruction.SectionPath;
            String liClass = "";
            
            // Checks the section path against the page url and determines if it needs to be selected or open
            if(path.Equals(item.URL))
            {
                liClass="leftnav-on";
            }
            else if (path.Contains(item.URL))
            {
                liClass="leftnav-open";
            }

            writer.AddAttribute(HtmlTextWriterAttribute.Class, liClass);
            writer.RenderBeginTag(HtmlTextWriterTag.Li);
            writer.AddAttribute(HtmlTextWriterAttribute.Href, item.URL);
            writer.RenderBeginTag(HtmlTextWriterTag.A);
            writer.Write(item.Title);
            writer.RenderEndTag();//end A tag
            
            // Checks if there are children for the node and
            // then whether the section path contains the page url and then renders it if it does
            if (item.ChildItems.Length > 0)
            {
                if (path.Contains(item.URL))
                {
                    writer.RenderBeginTag(HtmlTextWriterTag.Ul);
                    foreach (NavigationItem subitem in item.ChildItems)
                    {
                        RenderNavItem(subitem, writer);
                    }
                    writer.RenderEndTag();//end ul
                }
            }
            writer.RenderEndTag();//end li
            
        }

    }
}
