using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Linq;
using NCI.Util;
using NCI.Web.CDE.UI;

namespace CancerGov.Dictionaries.SnippetControls
{
    public class TermDictionaryRouter : BaseDictionaryRouter
    {
        protected System.Web.UI.WebControls.PlaceHolder phTermDictionary;

        protected Control localControl;

        /*protected void Page_Load(object sender, EventArgs e)
        {
            String searchString = Strings.Clean(Request.QueryString["search"]);
            String term = Strings.Clean(Request.QueryString["term"]);
            String cdrId = Strings.Clean(Request.QueryString["cdrid"]);
            String id = Strings.Clean(Request.QueryString["id"]);
            // default results to 'A' if no term chosen
            String expand = Strings.Clean(Request.QueryString["expand"], "A");
            String language = Strings.Clean(Request.QueryString["language"]);
            Control localControl = null;

            if (!String.IsNullOrEmpty(term))
            {
                searchString = term;
            }

            // Load appropriate control 
            if (!String.IsNullOrEmpty(searchString))
                localControl = Page.LoadControl("~/SnippetTemplates/TermDictionary/Views/TermDictionaryResultsList.ascx");
            else if (!String.IsNullOrEmpty(cdrId) || !String.IsNullOrEmpty(id))
                localControl = Page.LoadControl("~/SnippetTemplates/TermDictionary/Views/TermDictionaryDefinitionView.ascx");
            // 301 redirect to /def/
            // check for friendly name
            else if (!String.IsNullOrEmpty(expand))
                localControl = Page.LoadControl("~/SnippetTemplates/TermDictionary/Views/TermDictionaryResultsList.ascx");
            else
                localControl = Page.LoadControl("~/SnippetTemplates/TermDictionary/Views/TermDictionaryHome.ascx");

            if (localControl != null)
                phTermDictionary.Controls.Add(localControl);
        }*/

        protected void LoadDictionaryControl(string controlType)
        {
            // Load appropriate control based on controlType
            if (controlType.Equals("home"))
            {
                localControl = Page.LoadControl("~/SnippetTemplates/TermDictionary/Views/TermDictionaryHome.ascx");
            }
            else if (controlType.Equals("search") || controlType.Equals("expand"))
            {
                localControl = Page.LoadControl("~/SnippetTemplates/TermDictionary/Views/TermDictionaryResultsList.ascx");
            }
            else if (controlType.Equals("view"))
            {
                localControl = Page.LoadControl("~/SnippetTemplates/TermDictionary/Views/TermDictionaryDefinitionView.ascx");
            }

            // Add control to page
            if (localControl != null)
            {
                phTermDictionary.Controls.Add(localControl);
            }
        }
    }
}