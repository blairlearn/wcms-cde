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
using CancerGov.Common;
using CancerGov.CDR.TermDictionary;
using CancerGov.Web.SnippetTemplates;
using NCI.Web.CDE.UI.SnippetControls;
using NCI.Web.UI.WebControls;
using NCI.Web.CDE;
using NCI.Web;

namespace CancerGov.Web.SnippetTemplates
{
    public partial class GeneticsTermDictionaryRouter : SnippetControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            Control localControl = null;

            //localControl = Page.LoadControl("~/SnippetTemplates/GeneticsTermDictionaryHome.ascx");
            //localControl = Page.LoadControl("~/SnippetTemplates/GeneticsTermDictionaryResultsList.ascx");
            localControl = Page.LoadControl("~/SnippetTemplates/GeneticsTermDictionaryDefinitionView.ascx");
            
            if (localControl != null)
                phGeneticsTermDictionary.Controls.Add(localControl);

        }
    }
}