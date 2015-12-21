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
        protected void Page_Load(object sender, EventArgs e)
        {
            dictionarySearchBlock.Dictionary = DictionaryType.drug;
            dictionarySearchBlock.DictionaryURL = PageAssemblyContext.Current.requestedUrl.ToString();

        }
        
    }
}