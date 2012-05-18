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
           //Control localControl = null;

           // //localControl = Page.LoadControl("~/SnippetTemplates/GeneticsTermDictionaryHome.ascx");
           // localControl = Page.LoadControl("~/SnippetTemplates/GeneticsTermDictionaryResultsList.ascx");
           // //localControl = Page.LoadControl("~/SnippetTemplates/GeneticsTermDictionaryDefinitionView.ascx");
            
           // if (localControl != null)
           //     phGeneticsTermDictionary.Controls.Add(localControl);

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

            // Redirect if language parameter is not the same as language of the Page Assembly 
            if (!String.IsNullOrEmpty(language))
                language = language.ToLower();

            if (PageAssemblyContext.Current.PageAssemblyInstruction.Language == "en" &&
                language == GeneticsTermDictionaryHelper.SPANISH)
            {
                //redirect to Spanish site
                string reDirectUrl = PageAssemblyContext.Current.PageAssemblyInstruction.GetUrl(PageAssemblyInstructionUrls.AltLanguage) + "?" + Page.Request.QueryString.ToString();
                Page.Response.Redirect(reDirectUrl);
            }
            if (PageAssemblyContext.Current.PageAssemblyInstruction.Language == "es" &&
                language == GeneticsTermDictionaryHelper.ENGLISH)
            {
                //redirect to English site
                string reDirectUrl = PageAssemblyContext.Current.PageAssemblyInstruction.GetUrl(PageAssemblyInstructionUrls.AltLanguage) + "?" + Page.Request.QueryString.ToString();
                Page.Response.Redirect(reDirectUrl);
            }


            // Load appropriate control 
            if (!String.IsNullOrEmpty(searchString))
                localControl = Page.LoadControl("~/SnippetTemplates/GeneticsTermDictionaryResultsList.ascx");
            else if (!String.IsNullOrEmpty(cdrId) || !String.IsNullOrEmpty(id))
                localControl = Page.LoadControl("~/SnippetTemplates/GeneticsTermDictionaryDefinitionView.ascx");
            else if (!String.IsNullOrEmpty(expand))
                localControl = Page.LoadControl("~/SnippetTemplates/GeneticsTermDictionaryResultsList.ascx");
            else
                localControl = Page.LoadControl("~/SnippetTemplates/GeneticsTermDictionaryHome.ascx");

            if (localControl != null)
                phGeneticsTermDictionary.Controls.Add(localControl);

        }
    }
}