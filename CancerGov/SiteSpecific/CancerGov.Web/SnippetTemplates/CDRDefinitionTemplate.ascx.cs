using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using NCI.Web.UI.WebControls;
using NCI.Web.CDE.UI.Configuration;
using NCI.Web.CDE;
using NCI.Web.CDE.UI;
using CancerGov.CDR.TermDictionary;
namespace CancerGov.Web.SnippetTemplates
{
    public partial class CDRDefinitionTemplate : SnippetControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            ltDefinitionText.Text = DefinitionText;
        }

        protected string DefinitionText
        {
            get 
            {
                string language = string.Empty;
                if (PageAssemblyContext.Current.PageAssemblyInstruction.Language == "en")
                {
                    language = "English";
                }
                else
                {
                    language = "Spanish";
                }

                string definitionText = string.Empty;
                TermDictionaryDataItem dataItem = TermDictionaryManager.GetDefinitionByTermID(language, SnippetInfo.CDRId, null, 5);
                definitionText = SnippetInfo.CDRDefinitionName + ":" + dataItem.DefinitionHTML;
                return definitionText;
            }
        }

    }
}