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
    [ToolboxData("<{0}:CGovSectionNavControl runat=server></{0}:CGovSectionNavControl>")]
    public class CGovSectionNavControl : SnippetControl
    {
        //My Nav Here
        NavigationItem _navItem = null;

        //Property for My Nav
        public NavigationItem NavigationItem
        {
            get { return _navItem; }
            set { _navItem = value; }
        }

        public void Page_Load(object sender, EventArgs e)
        {
           _navItem = NavigationItem.ParseTree(SnippetInfo.Data);
        }

        public override void RenderControl(HtmlTextWriter writer)
        {
            base.RenderControl(writer);
            if (_navItem == null)
            {
                throw new Exception("Item is null");
            }


            //The section navigation has a beginning div tag which is why it is added here.
       
            writer.AddAttribute(HtmlTextWriterAttribute.Class, "leftnav-shaded-box");
            writer.RenderBeginTag(HtmlTextWriterTag.Div);

            writer.RenderBeginTag(HtmlTextWriterTag.H1);

            writer.AddAttribute(HtmlTextWriterAttribute.Href, _navItem.URL);
            writer.RenderBeginTag(HtmlTextWriterTag.A);
            writer.Write(_navItem.Title);
            writer.RenderEndTag();
            writer.RenderEndTag();

            RenderNavTree(_navItem, writer);
            //This ends the </div>.  The number of RenderEndTags must match the number of RenderBeginTags
            writer.RenderEndTag();


            
        }

        private void RenderNavTree(NavigationItem root, HtmlTextWriter writer)
        {
            //This will be the "anchor" html.  Probably the opening div, and the main ul tag.
            //remember that for the MainNav we don't draw the root information...
            // base.RenderControl(writer);
            //also remember that the RenderEndTag must be called when you want to "output" the closing tag.
            //I will put a placeholder div in for right now to help illustrate what I am getting at...
           
            //writer.Write("Test"+size); //testing to see if it gets here

            // writer.RenderBeginTag(HtmlTextWriterTag.Div);
            if (root.ChildItems.Length > 0)
            {
                //writer.Write("test if childitems");//tests to see if it gets here
                writer.RenderBeginTag(HtmlTextWriterTag.Ul);
                foreach (NavigationItem item in root.ChildItems)
                {
                    RenderNavItem(item, writer);
            
                }
                writer.RenderEndTag();
            }
           // writer.RenderEndTag();
        }

        private void RenderNavItem(NavigationItem item, HtmlTextWriter writer)
        {
            //base.RenderControl(writer);
            //Add in appropriate classes
            //detect if we the element is open/on/etc.
            String  path = Request.RawUrl;
            
           
            
            //render the element information, matching the current HTML - so you may need more than writer.write() statements for that
            //actually, you will because you need the link!!
            
            //the following needs to be before the if statement in case there is another list of items\

            //This block of code checks the URL path against the Item but since the home page has a path of '/'
            //First check to make sure it is the url path is not equal to the item url path
            if (path.Equals(item.URL))
            {
                writer.AddAttribute(HtmlTextWriterAttribute.Class, "leftnav-on");
            }
            else if (path.Contains(item.URL))
            {
                writer.AddAttribute(HtmlTextWriterAttribute.Class, "leftnav-open");
            }

            
            writer.RenderBeginTag(HtmlTextWriterTag.Li);
            
            
            writer.AddAttribute(HtmlTextWriterAttribute.Href, item.URL);
            
            writer.RenderBeginTag(HtmlTextWriterTag.A);

          
            
            writer.Write(item.Title);
            
         
            
            writer.RenderEndTag();
            
            
            if (item.ChildItems.Length > 0)
            {
                writer.RenderBeginTag(HtmlTextWriterTag.Ul);
                foreach (NavigationItem subitem in item.ChildItems)
                {
                    RenderNavItem(subitem, writer);
                }
                writer.RenderEndTag();
            }

            
            
            writer.RenderEndTag();
            //writer.Write("getting here?");
        }

    }
}
