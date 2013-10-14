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
        NavigationItem _navItem = null;

        //Property for My Nav
        public NavigationItem NavigationItem
        {
            get { return _navItem; }
            set { _navItem = value; }
        }

        public void Page_Load(object sender, EventArgs e)
        {
            //Fix ParseTree so it checks to make sure a variable is not null before calling a property or method
            //so, Foo myvar = DoSomething(); myvar.whatever();  without checking if DoSomething did not return
            //null is bad.  The only time it is ok is if you absolutely know DoSomething will never return 
            //null.  Either because of the MSDN documentation or you specifically made sure it would not be.
            _navItem = NavigationItem.ParseTree(SnippetInfo.Data);
            
        }

        public override void RenderControl(HtmlTextWriter writer)
        {
            base.RenderControl(writer);
            //Let's render out a basic div tag with hello world in it.

            //Ok let's add a class attribute.  Now, the way the HTMLWriter works is that it is a big stack.
            //So a stack is first in last out, and in ordr to add a class to the div, we need to do it
            //before we render the tag.

            //We can do this by adding an attribute.  Note this takes a HtmlTextWriter*Attribute*
            //not a HtmlTextWriter*Tag*.
            writer.AddAttribute(HtmlTextWriterAttribute.Class, "my-nav");

            //This starts the <div>
            writer.RenderBeginTag(HtmlTextWriterTag.Div);
            if (_navItem != null)
            {
                writer.Write("its not null");
            }


            writer.Write("Hello World");
            RenderNavTree(_navItem, writer);
            //This ends the </div>.  The number of RenderEndTags must match the number of RenderBeginTags
            writer.RenderEndTag();


            
        }

        private void RenderNavTree(NavigationItem root, HtmlTextWriter writer)
        {
            //This will be the "anchor" html.  Probably the opening div, and the main ul tag.
            //remember that for the MainNav we don't draw the root information...
            base.RenderControl(writer);
            //also remember that the RenderEndTag must be called when you want to "output" the closing tag.
            //I will put a placeholder div in for right now to help illustrate what I am getting at...
            int size = root.ChildItems.Length;

            writer.RenderBeginTag(HtmlTextWriterTag.Div);
            if (root.ChildItems.Length > 0)
            {
                writer.RenderBeginTag(HtmlTextWriterTag.Ul);
                foreach (NavigationItem item in root.ChildItems)
                {
                    RenderNavItem(item, writer, 1, size);
                }
                writer.RenderEndTag();
            }
            writer.RenderEndTag();
        }

        private void RenderNavItem(NavigationItem item, HtmlTextWriter writer, int itemNum, int numItems)
        {
            base.RenderControl(writer);
            //Add in appropriate classes
            //detect if we the element is open/on/etc.
            writer.RenderBeginTag(HtmlTextWriterTag.Li);
            if (itemNum == 1)
            {
                writer.AddAttribute(HtmlTextWriterAttribute.Class, "first nav-item-"+numItems);
            }
            else if (itemNum == numItems)
            {
                writer.AddAttribute(HtmlTextWriterAttribute.Class, "last nav-item-" + numItems);
            }
            else
            {
                writer.AddAttribute(HtmlTextWriterAttribute.Class, "nav-item-" + itemNum);
            }

            //render the element information, matching the current HTML - so you may need more than writer.write() statements for that
            //actually, you will because you need the link!!
            
            //the following needs to be before the if statement in case there is another list of items
            writer.RenderBeginTag(HtmlTextWriterTag.A);
            writer.AddAttribute(HtmlTextWriterAttribute.Href, item.SectionPath);
            writer.RenderBeginTag(HtmlTextWriterTag.Span);
            writer.Write(item.Title);
            writer.RenderEndTag();
            writer.RenderEndTag();
            
            
            if (item.ChildItems.Length > 0)
            {
                writer.RenderBeginTag(HtmlTextWriterTag.Ul);
                foreach (NavigationItem subitem in item.ChildItems)
                {
                    RenderNavItem(subitem, writer, itemNum++, numItems);
                }
                writer.RenderEndTag();
            }

            
            
            writer.RenderEndTag();
            writer.Write("getting here?");
        }

    }
}
