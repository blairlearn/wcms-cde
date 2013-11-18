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
            //loads the xml structure from the snippet infos
           _navItem = NavigationItem.ParseTree(SnippetInfo.Data);
        }

        public override void RenderControl(HtmlTextWriter writer)
        {
            //renders the control for the html output
            base.RenderControl(writer);
            if (_navItem == null)
            {
                throw new Exception("Item is null");
            }

            //generates the start of the html by creating a div with the class to shade the area
            writer.AddAttribute(HtmlTextWriterAttribute.Class, "leftnav-shaded-box");
            writer.RenderBeginTag(HtmlTextWriterTag.Div);

            writer.RenderBeginTag(HtmlTextWriterTag.H1);

            writer.AddAttribute(HtmlTextWriterAttribute.Href, _navItem.URL);
            writer.RenderBeginTag(HtmlTextWriterTag.A);
            writer.Write(_navItem.Title);
            writer.RenderEndTag();
            writer.RenderEndTag();

            //then calls the renderNavtree method with the nav item
            RenderNavTree(_navItem, writer);
         
            writer.RenderEndTag();


            
        }

        private void RenderNavTree(NavigationItem root, HtmlTextWriter writer)
        {
            //this method starts the rendering on the root navitem html from xml

           
            

            //calls RenderNavItem assuming there are child nodes for the root and calls it on each child
            if (root.ChildItems.Length > 0)
            {
                
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
            //gets the current webpage path to use for comparisons
            String  path = Request.RawUrl;
            
           
            
            //this checks the path against the url of the item and determines if it needs to be selected or open
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
            
            //this checks if there are children for the node and if there is checks to see if the path contains the url and then renders it if it does
            if (item.ChildItems.Length > 0)
            {
                if (path.Contains(item.URL))
                {
                    writer.RenderBeginTag(HtmlTextWriterTag.Ul);
                    foreach (NavigationItem subitem in item.ChildItems)
                    {
                        RenderNavItem(subitem, writer);
                    }
                    writer.RenderEndTag();
                }
            }

            
            
            writer.RenderEndTag();
            
        }

    }
}
