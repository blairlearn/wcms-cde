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
                string linksTableTitle;
                if (PageAssemblyContext.Current.PageAssemblyInstruction.Language == "en")
                {
                    language = "English";
                    linksTableTitle = "Table of Links";
                }
                else
                {
                    language = "Spanish";
                    linksTableTitle = "Lista de Enlaces";
                }

                string tableofLinks = string.Empty;
                tableofLinks = GetFootnotes(linksTableTitle);
                return tableofLinks;
            }


        }


        public string GetFootnotes(string title)
        {
            string footnotes = "";
            int LinkNum = 1;
            ArrayList tableofLinks = new ArrayList(PageAssemblyContext.Current.tableofLinksHash);
            

            if (PageAssemblyContext.Current.tableofLinksHash.Count > 0)
            {
                //footnotes = "<BR><BR><a name=\"" + title + "\"></a><h2>" + title + "</h2><P>";
                footnotes += "<table border=\"0\" cellspacing=\"2\" cellpadding=\"0\">\n";
                footnotes += "<tr><td align=\"left\" colspan=\"2\"><a name=\"" + title + "\"></a><h2>" + title + "</h2></td></tr>";


                for (int i = 0; i < tableofLinks.Count; i++)
                {
                    
                    footnotes += "<tr><td valign=\"top\"><a name=\"footnote" + (i+1) + "\"></a><b><sup>" + (i+1) + "</sup></b></td><td valign=\"top\">" + tableofLinks[i] + "</td></tr>\n";
                    LinkNum = LinkNum + 1;
                }
                footnotes += "</table>\n";
            }

            return footnotes;
        }
    }
}