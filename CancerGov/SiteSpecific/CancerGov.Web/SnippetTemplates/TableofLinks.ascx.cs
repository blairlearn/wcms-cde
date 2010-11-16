using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using NCI.Web.CDE;
using NCI.Web.CDE.UI;
namespace CancerGov.Web.SnippetTemplates
{
    public partial class TableofLinks : SnippetControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            ltTableofLinks.Text = TableofLinksList;

        }

        protected string TableofLinksList
        {
            get
            {
                string language = string.Empty;
                string snippetXmlData = string.Empty;
                if (PageAssemblyContext.Current.PageAssemblyInstruction.Language == "en")
                {
                    language = "English";
                }
                else
                {
                    language = "Spanish";
                }

                string tableofLinks = string.Empty;

                return tableofLinks;
            }
        }
    }
}