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

        private void RenderNavTree(NavigationItem root, HtmlTextWriter writer)
        {
            //this method generates the main navigation html and sets the classes for the tags accordingly from the html on live site
            //this is similar to the structure of the percussion templates that generate the xml

           
            //checks to make sure the current navon has children
            if (root.ChildItems.Length > 0)
            {
                writer.AddAttribute(HtmlTextWriterAttribute.Class, _navInfo.displayParams.CSSClasses);
                writer.RenderBeginTag(HtmlTextWriterTag.Ul);

                //This generates the homepage tab for the root in the Spanish site.s
                //changing because I shouldn't be checking for "/espanol" but this will have the 
  
                int count = 1;
                
                foreach (NavigationItem item in root.ChildItems)
                {

                    RenderNavItem(item, writer, count, root.ChildItems.Length);
                    count++;
                   
                }
                writer.RenderEndTag();
            }
        }

        private void RenderNavItem(NavigationItem item, HtmlTextWriter writer, int itemNum, int numItems)
        {
            //This method is similar to the RenderNavTree method above but this one generates class attributes based on which item is being passed
            
            //This path is the url path that would be on the current page and is used to figure out if something would be selected or not
            //example: for http://www.cancer.gov/aboutnci/globalhealth the variable would be "/aboutnci/globalhealth"
            String path = PageAssemblyContext.Current.PageAssemblyInstruction.SectionPath;

            String LiClass = "nav-item-"+itemNum;

            //some logic for whether the item is first or last in the children based on parameters passed fromt he previous method
            if (itemNum == 1)
            {
                LiClass = "first " + LiClass;
            }
            else if (itemNum == numItems)
            {
                LiClass = "last " + LiClass;
            }
            
            writer.AddAttribute(HtmlTextWriterAttribute.Class, LiClass);
            writer.RenderBeginTag(HtmlTextWriterTag.Li);
           

            //This block of code checks the URL path against the Item but since the home page has a path of '/'
            //First check to make sure it is the url path is not equal to the item url path

            //The following if statement checks to see if the path is equal to the homepage and the item has a section path of /homepage
            //may be taking this out and changing the logic so I'm not looking for specific names\
            //get rid of this

            String aClass = "";
            
           
            if (itemNum == 1)
            {
                if (path.Equals(item.SectionPath))
                {
                    aClass = "first current";
                }
                else
                {
                    aClass = "first";
                }
            }
            else if (itemNum == numItems)
            {
                if (path.Contains(item.URL))
                {
                    aClass="last current";
                }
                else
                {
                    aClass="last";
                }
            }
            else
            {
                if (path.Contains(item.URL))
                {
                   aClass="current";
                }
            }
           
           
            //these are links and what is displayed that are generated html
            writer.AddAttribute(HtmlTextWriterAttribute.Class, aClass);
            writer.AddAttribute(HtmlTextWriterAttribute.Href, item.URL);
            writer.RenderBeginTag(HtmlTextWriterTag.A);
            writer.RenderBeginTag(HtmlTextWriterTag.Span);
            writer.Write(item.Title);
            writer.RenderEndTag();//end Span
            writer.RenderEndTag();//end A
            
            //this checks to see if there are child items for the current nav itemand if there is it goes on to render the children
            //this logic came from the CGovSectionNav Control but this may be needed in the future for evolution if there are drop down menus
            if (item.ChildItems.Length > 0)
            {
                writer.RenderBeginTag(HtmlTextWriterTag.Ul);
                foreach (NavigationItem subitem in item.ChildItems)
                {
                    
                        RenderNavItem(subitem, writer, itemNum++, numItems);
                   
                }
                writer.RenderEndTag();//End UL
            }

            writer.RenderEndTag();//end Li
            
        }

    }
}
