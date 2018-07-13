using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using NCI.Web.CDE;
using NCI.Web.CDE.UI;
using NCI.Web.CDE.WebAnalytics;

namespace CancerGov.Web.SnippetTemplates
{
    public partial class BlogAnalytics : SnippetControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            IPageAssemblyInstruction pageInstruction = PageAssemblyContext.Current.PageAssemblyInstruction;
            BasePageAssemblyInstruction basePage = pageInstruction as BasePageAssemblyInstruction;
            string blogName = "";
            string blogContentType = "rx:cgvBlogPost";

            if (basePage.ContentItemInfo.ContentItemType == blogContentType)
            {
                blogName = SectionDetailFactory.GetSectionDetail(pageInstruction.SectionPath).GetWAContentGroups();
                if (!String.IsNullOrWhiteSpace(blogName))
                {
                    basePage.SetWebAnalytics(WebAnalyticsOptions.eVars.evar48.ToString(), ffD =>
                    {
                        ffD.Value = blogName + " Viewer";
                    });
                    basePage.SetWebAnalytics(WebAnalyticsOptions.Events.event53.ToString(), ffD =>
                    {
                        ffD.Value = String.Empty; // only fires off event number; no value needed
                    });
                }
            }

            this.Visible = false;
        }
    }
}