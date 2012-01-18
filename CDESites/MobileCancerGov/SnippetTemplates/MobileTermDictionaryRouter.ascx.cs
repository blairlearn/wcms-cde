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
using CancerGov.Text;
using CancerGov.CDR.TermDictionary;

namespace MobileCancerGov.Web.SnippetTemplates
{
    public partial class MobileTermDictionaryRouter : SnippetControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            String searchString = Strings.Clean(Request.QueryString["search"]);
            String cdrId = Strings.Clean(Request.QueryString["cdrid"]);
            String id = Strings.Clean(Request.QueryString["id"]);
            String expand = Strings.Clean(Request.QueryString["expand"]);
            Control localControl = null;
            
            if (!String.IsNullOrEmpty(searchString))
                localControl = Page.LoadControl("~/SnippetTemplates/MobileTermDictionaryResultsList.ascx");
            else if (!String.IsNullOrEmpty(cdrId) || !String.IsNullOrEmpty(id) )
                localControl = Page.LoadControl("~/SnippetTemplates/MobileTermDictionaryDefinitionView.ascx");
            else if (!String.IsNullOrEmpty(expand))
                localControl = Page.LoadControl("~/SnippetTemplates/MobileTermDictionaryResultsList.ascx");
            else
                localControl = Page.LoadControl("~/SnippetTemplates/MobileTermDictionaryHome.ascx");
        
            if (localControl != null)
                phMobileTermDictionary.Controls.Add(localControl);

        }
    }
}