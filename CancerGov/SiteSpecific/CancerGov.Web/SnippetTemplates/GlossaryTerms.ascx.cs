using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using NCI.Web.CDE;
using NCI.Web.CDE.UI;
using CancerGov.Common.Extraction;
using System.Collections;

namespace CancerGov.Web.SnippetTemplates
{
    public partial class GlossaryTerms : System.Web.UI.UserControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            ltGlossaryTerms.Text = GlossaryTermsList;

        }

        protected string GlossaryTermsList
        {
            get
            {
                string language = string.Empty;
                string snippetXmlData = string.Empty;
                string linksTableTitle;
                if (PageAssemblyContext.Current.PageAssemblyInstruction.Language == "en")
                {
                    language = "English";
                    linksTableTitle = "Glossary Terms";
                }
                else
                {
                    language = "Spanish";
                    linksTableTitle = "Glosario";
                }

                string tableofLinks = string.Empty;
                tableofLinks = BuildGlossaryTable(linksTableTitle);
                return tableofLinks;
            }


        }


        public string BuildGlossaryTable(string title)
        {
            GlossaryTermExtractor gte = new GlossaryTermExtractor();
            string glossaryTerms = "";
            glossaryTerms = gte.BuildGlossaryTable(title, PageAssemblyContext.Current.glossaryIds, PageAssemblyContext.Current.glossaryIDHash, PageAssemblyContext.Current.glossaryTermHash);
            return glossaryTerms;
        }
    }
}