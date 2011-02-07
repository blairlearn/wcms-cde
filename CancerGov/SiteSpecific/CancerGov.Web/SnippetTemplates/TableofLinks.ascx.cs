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
using NCI.Web.Extensions;
using NCI.Web.CDE.UI.SnippetControls;
using CancerGov.Common;

namespace CancerGov.Web.SnippetTemplates
{
    public partial class TableofLinks : SnippetControl
    {
        private ArrayList tableofLinksHash = new ArrayList();
        protected void Page_Load(object sender, EventArgs e)
        {

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

        protected override void OnPreRender(EventArgs e)
        {
            base.OnPreRender(e);
            string data = string.Empty;
            FootnoteExtractor fe = new FootnoteExtractor();

            foreach (GenericHtmlContentSnippet slot in Page.FindControlByType<GenericHtmlContentSnippet>())
            {

                if (slot.SnippetInfo.SlotName != "cgvSectionNav" && slot.SnippetInfo.SlotName != "cgvSiteBannerPrint" && slot.SnippetInfo.SlotName != "cgvContentHeader" && slot.SnippetInfo.SlotName != "cgvSiteBanner" && slot.SnippetInfo.SlotName != "cgvLanguageDate" && slot.SnippetInfo.SlotName != "cgvBodyHeader")
                {
                    data = slot.SnippetInfo.Data;
                    data = data.Replace("contenteditable=\"false\"", "");
                    data = fe.Extract(new Regex("<a\\s+?(?:class=\".*?\"\\s+?)*?href=\"(?<extractValue>.*?)\"(?:\\s+?\\w+?=\"(?:.*?)\")*?\\s*?>(?<linkText>.*?)</a>", RegexOptions.IgnoreCase | RegexOptions.Compiled | RegexOptions.Singleline), "extractValue", CancerGov.Common.Extraction.ExtractionTypes.URL, data);
                    if (fe.hashIndex.Count >= 1)
                    {
                        for (int i = 0; i < fe.hashIndex.Count; i++)
                        {
                            if (!tableofLinksHash.Contains(fe.hashIndex[i].ToString()))
                            {

                                {
                                    tableofLinksHash.Add(fe.hashIndex[i].ToString());
                                    //tableofLinksHash.AddRange(fe.hashIndex);
                                }
                            }
                        }
                    }
                    slot.HtmlData = data;

                }
            }

            ltTableofLinks.Text = TableofLinksList;

        }
        public string GetFootnotes(string title)
        {
            string footnotes = "";
            int LinkNum = 1;

            if (tableofLinksHash.Count > 0)
            {
                //footnotes = "<BR><BR><a name=\"" + title + "\"></a><h2>" + title + "</h2><P>";
                footnotes += "<table border=\"0\" cellspacing=\"2\" cellpadding=\"0\">\n";
                footnotes += "<tr><td align=\"left\" colspan=\"2\"><a name=\"" + title + "\"></a><h2>" + title + "</h2></td></tr>";


                for (int i = 0; i < tableofLinksHash.Count; i++)
                {

                    footnotes += "<tr><td valign=\"top\"><a name=\"footnote" + (i + 1) + "\"></a><b><sup>" + (i + 1) + "</sup></b></td><td valign=\"top\">" + Functions.TruncLine(tableofLinksHash[i].ToString(), 80, true, true) + "</td></tr>\n";
                    LinkNum = LinkNum + 1;
                }
                footnotes += "</table>\n";
            }

            return footnotes;
        }
    }
}