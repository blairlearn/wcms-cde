using System;
using System.Web.UI;
using NCI.Util;
using NCI.Web.CDE.UI;

namespace CancerGov.Dictionaries.SnippetControls
{
    public class DrugDictionaryRouter : BaseDictionaryRouter
    {
        protected BaseDictionaryControl localControl;

        protected override BaseDictionaryControl LoadHomeControl()
        {
            localControl = (BaseDictionaryControl)Page.LoadControl("~/SnippetTemplates/DrugDictionary/Views/DrugDictionaryHome.ascx");
            return localControl;
        }

        protected override BaseDictionaryControl LoadResultsListControl()
        {
            localControl = (BaseDictionaryControl)Page.LoadControl("~/SnippetTemplates/DrugDictionary/Views/DrugDictionaryResultsList.ascx");
            return localControl;
        }

        protected override BaseDictionaryControl LoadDefinitionViewControl()
        {
            localControl = (BaseDictionaryControl)Page.LoadControl("~/SnippetTemplates/DrugDictionary/Views/DrugDictionaryDefinitionView.ascx"); 
            return localControl;
        }
    }
}