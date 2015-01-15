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
using NCI.Web.CDE.UI;
using NCI.Web.CDE;

namespace CancerGov.Web.UI.SnippetControls
{
    /// <summary>
    /// This Snippet Template is for displaying blobs of HTML that Percussion has generated.
    /// </summary>
    [DefaultProperty("Text")]
    [ToolboxData("<{0}:CGovMainNavControl runat=server></{0}:CGovMainNavControl>")]
    public class CGovMainNavControl : SnippetControl
    { 
        //My Nav Here
        NavigationDisplayInfo _navInfo = null;

        public void Page_Load(object sender, EventArgs e)
        {
            //this calls the ParseTree method in NavigationItem.cs which loads up the xml from the templates in Rhythmyx
            _navInfo = NavigationDisplayInfo.ParseTree(SnippetInfo.Data);

        }

        public override void RenderControl(HtmlTextWriter writer)
        {

            //This method renders the control that starts out the generation of the html for the Navigation of the Main Navigation
            base.RenderControl(writer);

            //calls the RenderNavTree Method on the navigation item passed in and uses the html writer that was 
            RenderNavTree(_navInfo.rootNavItem, writer);

        }

        /// <summary>
        /// Generates the main navigation html and sets the classes for the tags accordingly from the html on live site.
        /// This is similar to the structure of the percussion templates that generate the xml.
        /// </summary>
        /// <param name="root"></param>
        /// <param name="writer"></param>
        private void RenderNavTree(NavigationItem root, HtmlTextWriter writer)
        {
            //checks to make sure we have something to render and that has children
            if (root != null && root.ChildItems.Length > 0)
            {
                writer.AddAttribute(HtmlTextWriterAttribute.Class, "navigation mobile-menu-bar");
                writer.RenderBeginTag(HtmlTextWriterTag.Div);
                writer.AddAttribute(HtmlTextWriterAttribute.Id, "mega-nav");
                writer.AddAttribute("role", "navigation");
                writer.RenderBeginTag("nav");
                writer.AddAttribute(HtmlTextWriterAttribute.Class, "menu nav-menu");
                writer.RenderBeginTag(HtmlTextWriterTag.Ul);

                //This generates the homepage tab for the root in the Spanish site.s
                //changing because I shouldn't be checking for "/espanol" but this will have the 
                int count = 1;
                foreach (NavigationItem item in root.ChildItems)
                {
                    RenderNavItem(item, writer, count, root.ChildItems.Length);
                    count++;
                }

                writer.RenderEndTag();//end ul
                writer.RenderEndTag();//end nav
                writer.RenderEndTag();//endDiv
            }
        }

        private void RenderNavItem(NavigationItem item, HtmlTextWriter writer, int itemNum, int numItems)
        {
            //This method is similar to the RenderNavTree method above but this one generates class attributes based on which item is being passed

            //This path is the url path that would be on the current page and is used to figure out if something would be selected or not
            //example: for http://www.cancer.gov/aboutnci/globalhealth the variable would be "/aboutnci/globalhealth"
            String path = PageAssemblyContext.Current.PageAssemblyInstruction.SectionPath;



            writer.AddAttribute(HtmlTextWriterAttribute.Class, "nav-item");
            writer.RenderBeginTag(HtmlTextWriterTag.Li);


            //This block of code checks the URL path against the Item but since the home page has a path of '/'
            //First check to make sure it is the url path is not equal to the item url path

            //The following if statement checks to see if the path is equal to the homepage and the item has a section path of /homepage
            //may be taking this out and changing the logic so I'm not looking for specific names\
            //get rid of this



           



            //these are links and what is displayed that are generated html
            writer.AddAttribute(HtmlTextWriterAttribute.Class, "nav-item-title");
            writer.RenderBeginTag(HtmlTextWriterTag.Div);
            if (path.Contains(item.SectionPath))
            {
                writer.AddAttribute(HtmlTextWriterAttribute.Class, "active");
            }
            writer.AddAttribute(HtmlTextWriterAttribute.Href, item.URL);
            //writer.AddAttribute(HtmlTextWriterAttribute.Id, item.PathName);
            writer.RenderBeginTag(HtmlTextWriterTag.A);
            writer.Write(item.Title);
            writer.RenderEndTag();//end A\

            
            
          

            //this checks to see if there are child items for the current nav itemand if there is it goes on to render the children
            //this logic came from the CGovSectionNav Control but this may be needed in the future for evolution if there are drop down menus
            if (item.ChildItems.Length > 0)
            {
                writer.AddAttribute("aria-expanded", "false");
                writer.AddAttribute(HtmlTextWriterAttribute.Class, "toggle");
                writer.AddAttribute(HtmlTextWriterAttribute.Type, "button");
                writer.RenderBeginTag(HtmlTextWriterTag.Button);
                writer.AddAttribute(HtmlTextWriterAttribute.Class, "hidden");
                writer.RenderBeginTag(HtmlTextWriterTag.Span);
                writer.Write("Open child elements");
                writer.RenderEndTag();//p tag
                writer.RenderEndTag();//button
                writer.RenderEndTag();//div if child items

                writer.AddAttribute(HtmlTextWriterAttribute.Class, "mobile-item");
                writer.RenderBeginTag(HtmlTextWriterTag.Ul);

                foreach (NavigationItem subitem in item.ChildItems)
                {
                    if (subitem.ChildItems.Length > 0)
                    {
                        writer.AddAttribute(HtmlTextWriterAttribute.Class, "has-children");
                    }
                    writer.RenderBeginTag(HtmlTextWriterTag.Li);
                    RenderSubItem(subitem, writer, itemNum++, numItems);
                    writer.RenderEndTag();
                }
                writer.RenderEndTag();//End UL
            }
            else
            {
                writer.RenderEndTag();//div if no child items
            }

            writer.AddAttribute("aria-expanded", "false");
            writer.AddAttribute("aria-haspopup", "true");
            writer.AddAttribute(HtmlTextWriterAttribute.Class, "sub-nav-mega");
            writer.RenderBeginTag(HtmlTextWriterTag.Div);
            writer.Write(item.MegaMenuInfo);
            writer.RenderEndTag();//end Div




            writer.RenderEndTag();//end Li

        }

        private void RenderSubItem(NavigationItem item, HtmlTextWriter writer, int itemNum, int numItems)
        {
            //This method is similar to the RenderNavItem method above but this one doesn't generate megamenu info

            //This path is the url path that would be on the current page and is used to figure out if something would be selected or not
            //example: for http://www.cancer.gov/aboutnci/globalhealth the variable would be "/aboutnci/globalhealth"
            String path = PageAssemblyContext.Current.PageAssemblyInstruction.SectionPath;



            


            //This block of code checks the URL path against the Item but since the home page has a path of '/'
            //First check to make sure it is the url path is not equal to the item url path

            //The following if statement checks to see if the path is equal to the homepage and the item has a section path of /homepage
            //may be taking this out and changing the logic so I'm not looking for specific names\
            //get rid of this



            if (path.Contains(item.SectionPath))
            {
                writer.AddAttribute(HtmlTextWriterAttribute.Class, "active");
            }



            //these are links and what is displayed that are generated html
            writer.AddAttribute(HtmlTextWriterAttribute.Class, "nav-item-title");
            writer.RenderBeginTag(HtmlTextWriterTag.Div);
            if (path.Contains(item.SectionPath))
            {
                writer.AddAttribute(HtmlTextWriterAttribute.Class, "active");
            }
            writer.AddAttribute(HtmlTextWriterAttribute.Href, item.URL);
            //writer.AddAttribute(HtmlTextWriterAttribute.Id, item.PathName);
            writer.RenderBeginTag(HtmlTextWriterTag.A);
            writer.Write(item.Title);
            writer.RenderEndTag();//end A\





            //this checks to see if there are child items for the current nav itemand if there is it goes on to render the children
            //this logic came from the CGovSectionNav Control but this may be needed in the future for evolution if there are drop down menus
            if (item.ChildItems.Length > 0)
            {
                writer.AddAttribute("aria-expanded", "false");
                writer.AddAttribute(HtmlTextWriterAttribute.Class, "toggle");
                writer.AddAttribute(HtmlTextWriterAttribute.Type, "button");
                writer.RenderBeginTag(HtmlTextWriterTag.Button);
                writer.AddAttribute(HtmlTextWriterAttribute.Class, "hidden");
                writer.RenderBeginTag(HtmlTextWriterTag.Span);
                writer.Write("Open child elements");
                writer.RenderEndTag();//p tag
                writer.RenderEndTag();//button
                writer.RenderEndTag();//div if child items

                writer.AddAttribute(HtmlTextWriterAttribute.Class, "mobile-item");
                writer.RenderBeginTag(HtmlTextWriterTag.Ul);

                foreach (NavigationItem subitem in item.ChildItems)
                {
                    if (subitem.ChildItems.Length > 0)
                    {
                        writer.AddAttribute(HtmlTextWriterAttribute.Class, "has-children");
                    }
                    writer.RenderBeginTag(HtmlTextWriterTag.Li);
                    RenderSubItem(subitem, writer, itemNum++, numItems);
                    writer.RenderEndTag();
                }
                writer.RenderEndTag();//End UL
            }
            else
            {
                writer.RenderEndTag();//div if no child items
            }







        }

    }
}
