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
using NCI.Web.CDE.UI.SnippetControls;
using CancerGov.CDR.TermDictionary;


namespace MobileCancerGov.Web.SnippetTemplates
{
    public partial class MobileDictionaryHome : SnippetControl
    {

        protected void Page_Load(object sender, EventArgs e)
        {
            litPageUrl.Text = MobileTermDictionary.RawUrlClean(Page.Request.RawUrl);
            litSearchBlock.Text = MobileTermDictionary.SearchBlock("","");
            litAZList.Text = MobileTermDictionary.AZBlock(MobileTermDictionary.RawUrlClean(Page.Request.RawUrl));
        }
    }
}