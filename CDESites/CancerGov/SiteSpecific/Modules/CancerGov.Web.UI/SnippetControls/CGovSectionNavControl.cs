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
           // _navItem = NavigationItem.ParseTree(SnippetInfo.Data);
        }

        public override void RenderControl(HtmlTextWriter writer)
        {
            base.RenderControl(writer);
            //oops!
            //LiteralControl lit = new LiteralControl(NavigationItem.ToString());
            //lit.RenderControl(writer);
        }
    }
}
