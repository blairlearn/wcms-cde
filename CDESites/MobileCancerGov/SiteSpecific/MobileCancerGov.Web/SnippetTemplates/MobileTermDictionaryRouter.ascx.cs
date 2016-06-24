using System;
using System.Web.UI;
using NCI.Util;
using NCI.Web.CDE;
using NCI.Web.CDE.UI;
using NCI.Web.CDE.UI.SnippetControls;


namespace MobileCancerGov.Web.SnippetTemplates
{
    public partial class MobileTermDictionaryRouter : SnippetControl
    {
        protected void Page_Load(object sender, EventArgs e)
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

            // Redirect if language parameter is not the same as language of the Page Assembly 
            if (!String.IsNullOrEmpty(language))
                language = language.ToLower();

            if (PageAssemblyContext.Current.PageAssemblyInstruction.Language == "en" &&
                language == MobileTermDictionary.SPANISH)
            {
                //redirect to Spanish site
                string reDirectUrl = PageAssemblyContext.Current.PageAssemblyInstruction.GetUrl(PageAssemblyInstructionUrls.AltLanguage) + "?" +  Page.Request.QueryString.ToString();
                Page.Response.Redirect(reDirectUrl);
            }
            if (PageAssemblyContext.Current.PageAssemblyInstruction.Language == "es" &&
                language == MobileTermDictionary.ENGLISH)
            {
                //redirect to English site
                string reDirectUrl = PageAssemblyContext.Current.PageAssemblyInstruction.GetUrl(PageAssemblyInstructionUrls.AltLanguage) + "?" + Page.Request.QueryString.ToString();
                Page.Response.Redirect(reDirectUrl);
            }


            // Load appropriate control 
            if (!String.IsNullOrEmpty(searchString))
                localControl = Page.LoadControl("~/SnippetTemplates/MobileTermDictionaryResultsList.ascx");
            else if (!String.IsNullOrEmpty(cdrId) || !String.IsNullOrEmpty(id))
                localControl = Page.LoadControl("~/SnippetTemplates/MobileTermDictionaryDefinitionView.ascx");
            else if (!String.IsNullOrEmpty(expand))
                localControl = Page.LoadControl("~/SnippetTemplates/MobileTermDictionaryResultsList.ascx");
            else if (!String.IsNullOrEmpty(expand))
                localControl = Page.LoadControl("~/SnippetTemplates/MobileTermDictionaryResultsList.ascx");
            else
                localControl = Page.LoadControl("~/SnippetTemplates/MobileTermDictionaryHome.ascx");
        
            if (localControl != null)
                phMobileTermDictionary.Controls.Add(localControl);

        }
    }
}