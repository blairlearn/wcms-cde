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
            String searchString = Strings.Clean(Request.QueryString["search"]);
            String term = Strings.Clean(Request.QueryString["term"]);
            String cdrId = Strings.Clean(Request.QueryString["cdrid"]);
            String id = Strings.Clean(Request.QueryString["id"]);
            String expand = Strings.Clean(Request.QueryString["expand"]);
            String language = Strings.Clean(Request.QueryString["language"]);
            Control localControl = null;

            if (!String.IsNullOrEmpty(term))
            {
                searchString = term;
            }

            // Load appropriate control 
            if (!String.IsNullOrEmpty(searchString))
                localControl = Page.LoadControl("~/SnippetTemplates/GeneticsTermDictionary/GeneticsTermDictionaryResultsList.ascx");
            else if (!String.IsNullOrEmpty(cdrId) || !String.IsNullOrEmpty(id))
                localControl = Page.LoadControl("~/SnippetTemplates/GeneticsTermDictionary/GeneticsTermDictionaryDefinitionView.ascx");
            else if (!String.IsNullOrEmpty(expand))
                localControl = Page.LoadControl("~/SnippetTemplates/GeneticsTermDictionary/GeneticsTermDictionaryResultsList.ascx");
            else
                localControl = Page.LoadControl("~/SnippetTemplates/GeneticsTermDictionary/GeneticsTermDictionaryHome.ascx");

            if (localControl != null)
                phGeneticsTermDictionary.Controls.Add(localControl);

        }
    }
}