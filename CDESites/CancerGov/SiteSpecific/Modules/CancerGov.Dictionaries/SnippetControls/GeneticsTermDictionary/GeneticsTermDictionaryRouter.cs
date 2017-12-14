using System;
using System.Web.UI;
using NCI.Util;
using NCI.Web.CDE.UI;

namespace CancerGov.Dictionaries.SnippetControls
{
    public class GeneticsTermDictionaryRouter : BaseDictionaryRouter
    {
        protected Control localControl;

        protected override Control LoadHomeControl()
        {
            localControl = Page.LoadControl("~/SnippetTemplates/GeneticsTermDictionary/Views/GeneticsTermDictionaryHome.ascx");
            return localControl;
        }

        protected override Control LoadResultsListControl()
        {
            localControl = Page.LoadControl("~/SnippetTemplates/GeneticsTermDictionary/Views/GeneticsTermDictionaryResultsList.ascx");
            return localControl;
        }

        protected override Control LoadDefinitionViewControl()
        {
            localControl = Page.LoadControl("~/SnippetTemplates/GeneticsTermDictionary/Views/GeneticsTermDictionaryDefinitionView.ascx");
            return localControl;
        }
    }
}