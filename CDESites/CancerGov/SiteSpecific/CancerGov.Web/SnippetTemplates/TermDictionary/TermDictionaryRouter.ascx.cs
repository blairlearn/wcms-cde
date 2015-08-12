using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using NCI.Web.CDE.UI;
using CancerGov.Text;
using CancerGov.Common;

namespace CancerGov.Web.SnippetTemplates
{
    public partial class TermDictionaryRouter : SnippetControl
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
                localControl = Page.LoadControl("~/SnippetTemplates/TermDictionary/TermDictionaryResultsList.ascx");
            else if (!String.IsNullOrEmpty(cdrId) || !String.IsNullOrEmpty(id))
                localControl = Page.LoadControl("~/SnippetTemplates/TermDictionary/TermDictionaryDefinitionView.ascx.ascx");
            else if (!String.IsNullOrEmpty(expand))
                localControl = Page.LoadControl("~/SnippetTemplates/TermDictionary/TermDictionaryResultsList.ascx");
            else
                localControl = Page.LoadControl("~/SnippetTemplates/TermDictionary/TermDictionaryHome.ascx");
                       
            if (localControl != null)
                phTermDictionary.Controls.Add(localControl);
        }
    }
}