using System;
using System.Web.UI;
using NCI.Util;
using NCI.Web.CDE.UI;

namespace CancerGov.Dictionaries.SnippetControls
{
    public class TermDictionaryRouter : BaseDictionaryRouter
    {
        protected BaseDictionaryControl localControl;

        protected override BaseDictionaryControl LoadExpandListControl()
        {
            localControl = (BaseDictionaryControl)Page.LoadControl("~/SnippetTemplates/TermDictionary/Views/TermDictionaryExpandList.ascx");
            return localControl;
        }

        protected override BaseDictionaryControl LoadResultsListControl()
        {
            localControl = (BaseDictionaryControl)Page.LoadControl("~/SnippetTemplates/TermDictionary/Views/TermDictionaryResultsList.ascx");
            return localControl;
        }

        protected override BaseDictionaryControl LoadDefinitionViewControl()
        {
            localControl = (BaseDictionaryControl)Page.LoadControl("~/SnippetTemplates/TermDictionary/Views/TermDictionaryDefinitionView.ascx");
            return localControl;
        }
    }
}