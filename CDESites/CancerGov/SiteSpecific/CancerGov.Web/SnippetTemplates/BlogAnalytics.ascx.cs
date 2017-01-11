using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using NCI.Web.CDE;
using NCI.Web.CDE.UI;

namespace CancerGov.Web.SnippetTemplates
{
    public partial class BlogAnalytics : SnippetControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            BasePageAssemblyInstruction basePage = PageAssemblyContext.Current.PageAssemblyInstruction as BasePageAssemblyInstruction;
            string blogName = "";

            if (basePage.ContentItemInfo.ContentItemType == "rx:cgvBlogPost")
            {
                blogName = basePage.GetWebAnalytics("prop44");
                if (!String.IsNullOrWhiteSpace(blogName))
                {
                    basePage.SetWebAnalytics("evar48", ffD =>
                    {
                        ffD.Value = blogName + " Viewer";
                    });
                    basePage.SetWebAnalytics("event53", ffD =>
                    {
                        // only fires off event
                    });
                }
            }

            this.Visible = false;
        }
    }
}