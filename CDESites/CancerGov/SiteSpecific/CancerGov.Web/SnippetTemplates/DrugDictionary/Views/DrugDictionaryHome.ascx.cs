using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using NCI.Web.CDE.UI;
using NCI.Web.CDE;
using NCI.Web.Dictionary;

namespace CancerGov.Web.SnippetTemplates
{
    public partial class DrugDictionaryHome : SnippetControl
    {
        public int TotalCount = 0;

        protected void Page_Load(object sender, EventArgs e)
        {
            dictionarySearchBlock.Dictionary = DictionaryType.Term;
            dictionarySearchBlock.DictionaryURL = PageAssemblyContext.Current.requestedUrl.ToString();

            DictionaryAppManager _dictionaryAppManager = new DictionaryAppManager();
            DictionarySearchResultCollection resultCollection = _dictionaryAppManager.Search("%", SearchType.Begins, 0, int.MaxValue, NCI.Web.Dictionary.DictionaryType.drug, PageAssemblyContext.Current.PageAssemblyInstruction.Language);

            if (resultCollection != null)
                TotalCount = resultCollection.ResultsCount;

            // Drug dictionary is always in English
            pnlIntroEnglish.Visible = true;
            pnlIntroSpanish.Visible = false;

        }
    }
}