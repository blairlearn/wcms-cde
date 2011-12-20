using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;
using NCI.Web.CDE.UI;
using NCI.Web.CDE.WebAnalytics;

namespace MobileCancerGov.Web.SnippetTemplates
{
    public partial class MobileDictionaryHome : SnippetControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            azLink.HRef = Page.Request.Url.LocalPath;
        }

        protected void ImageButton1_Click(object sender, ImageClickEventArgs e)
        {
            Page.Response.Redirect(Page.Request.Url.LocalPath + "?SearchString=" + searchString.Value.Trim().ToString());
        }
    }
}