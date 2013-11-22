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

        //Property for My Nav
        public NavigationDisplayInfo NavigationDisplayInfo
        {
            get { return _navInfo; }
            set { _navInfo = value; }
        }

        public void Page_Load(object sender, EventArgs e)
        {
            //this calls the ParseTree method in NavigationItem.cs which loads up the xml from the templates in Rhythmyx
            _navInfo = NavigationDisplayInfo.ParseTree(SnippetInfo.Data);
            
        }

        public override void RenderControl(HtmlTextWriter writer)
        {
            
            //This method renders the control that starts out the generation of the html for the Navigation of the Main Navigation
            base.RenderControl(writer);
            //This path is the url path that would be on the current page and is used to figure out if something would be selected or not
            //example: for http://www.cancer.gov/aboutnci/globalhealth the variable would be "/aboutnci/globalhealth"
            String path = Request.RawUrl;
          
            //This code will be taken out or generated a differently way once I figure out how, leaving for now though for testing purposes
           
            
            writer.AddAttribute(HtmlTextWriterAttribute.Class, _navInfo.displayParams.CSSClasses);
            

            //calls the RenderNavTree Method on the navigation item passed in and uses the html writer that was 
            RenderNavTree(_navInfo.rootNavItem, writer);
          


            
        }

        private void RenderNavTree(NavigationItem root, HtmlTextWriter writer)
        {
            //this method generates the main navigation html and sets the classes for the tags accordingly from the html on live site
            //this is similar to the structure of the percussion templates that generate the xml

            //gets the number of children
            int size = root.ChildItems.Length;
            int count = 1;
            
            //once again getting the path from the url page currently open
            String path = Request.RawUrl;
           
            //checks to make sure the current navon has children
            if (root.ChildItems.Length > 0)
            {
                
                writer.RenderBeginTag(HtmlTextWriterTag.Ul);

                //This generates the homepage tab for the root in the Spanish site.s
                //changing because I shouldn't be checking for "/espanol" but this will have the 
  
                    
                
                foreach (NavigationItem item in root.ChildItems)
                {
                    
                   
                        RenderNavItem(item, writer, count, size);
                        count++;
                   
                }
                writer.RenderEndTag();
            }
        }

        private void RenderNavItem(NavigationItem item, HtmlTextWriter writer, int itemNum, int numItems)
        {
            //This method is similar to the RenderNavTree method above but this one generates class attributes based on which item is being passed
            

            //the path is gotten again for comparison to the section paths of items to have items be selected or not on the webpage

            String  path = Request.RawUrl;


            //some logic for whether the item is first or last in the children based on parameters passed fromt he previous method
            if (itemNum == 1)
            {
                writer.AddAttribute(HtmlTextWriterAttribute.Class, "first nav-item-"+itemNum);
            }
            else if (itemNum == numItems)
            {
                writer.AddAttribute(HtmlTextWriterAttribute.Class, "last nav-item-" + numItems);
            }
            else
            {
                writer.AddAttribute(HtmlTextWriterAttribute.Class, "nav-item-" + itemNum);
            }
            writer.RenderBeginTag(HtmlTextWriterTag.Li);
           

            //This block of code checks the URL path against the Item but since the home page has a path of '/'
            //First check to make sure it is the url path is not equal to the item url path

            //The following if statement checks to see if the path is equal to the homepage and the item has a section path of /homepage
            //may be taking this out and changing the logic so I'm not looking for specific names
            if (path=="/" && item.SectionPath.Equals("/homepage"))
            {
                writer.AddAttribute(HtmlTextWriterAttribute.Class, "first current");
            }

            else if (!path.Equals(item.URL))
            {
                if (itemNum == 1)
                {
                    if (path.Contains(item.SectionPath))
                    {
                        writer.AddAttribute(HtmlTextWriterAttribute.Class, "first current");
                    }
                    writer.AddAttribute(HtmlTextWriterAttribute.Class, "first");
                }
                else if (itemNum == numItems)
                {
                    if (path.Contains(item.URL))
                    {
                        writer.AddAttribute(HtmlTextWriterAttribute.Class, "last current");
                    }
                    else
                    {
                        writer.AddAttribute(HtmlTextWriterAttribute.Class, "last");
                    }
                }
                else
                {
                    if (path.Contains(item.URL))
                    {
                        writer.AddAttribute(HtmlTextWriterAttribute.Class, "current");
                    }
                }
            }

            else
            {
                if (itemNum == 1)
                {
                    if (path.Contains(item.URL))
                    {
                        writer.AddAttribute(HtmlTextWriterAttribute.Class, "first current");
                    }
                    writer.AddAttribute(HtmlTextWriterAttribute.Class, "first");
                }
                else if (itemNum == numItems)
                {
                    if (path.Contains(item.URL))
                    {
                        writer.AddAttribute(HtmlTextWriterAttribute.Class, "last current");
                    }
                    else
                    {
                        writer.AddAttribute(HtmlTextWriterAttribute.Class, "last");
                    }
                }
                else
                {
                    if (path.Contains(item.URL))
                    {
                        writer.AddAttribute(HtmlTextWriterAttribute.Class, "current");
                       

                    }
                }
            }
            //these are links and what is displayed that are generated html
            
            writer.AddAttribute(HtmlTextWriterAttribute.Href, item.URL);
            
            writer.RenderBeginTag(HtmlTextWriterTag.A);

            writer.RenderBeginTag(HtmlTextWriterTag.Span);
            
            writer.Write(item.Title);
            
            writer.RenderEndTag();
            
            writer.RenderEndTag();
            
            //this checks to see if there are child items for the current nav itemand if there is it goes on to render the children
            //this logic came from the CGovSectionNav Control but this may be needed in the future for evolution if there are drop down menus
            if (item.ChildItems.Length > 0)
            {
                writer.RenderBeginTag(HtmlTextWriterTag.Ul);
                foreach (NavigationItem subitem in item.ChildItems)
                {
                    //might have to change this as well to make sure it doesn't use strings
                    
                    
                        RenderNavItem(subitem, writer, itemNum++, numItems);
                   
                }
                writer.RenderEndTag();
            }

            
            
            writer.RenderEndTag();
            
        }

    }
}
