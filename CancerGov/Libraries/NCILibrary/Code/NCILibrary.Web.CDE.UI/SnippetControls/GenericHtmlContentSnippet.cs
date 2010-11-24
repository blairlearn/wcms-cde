using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using CancerGov.Common.Extraction;
using System.Text.RegularExpressions;

using NCI.Text;

namespace NCI.Web.CDE.UI.SnippetControls
{
    /// <summary>
    /// This Snippet Template is for displaying blobs of HTML that Percussion has generated.
    /// </summary>
    [DefaultProperty("Text")]
    [ToolboxData("<{0}:GenericHtmlContentSnippet runat=server></{0}:GenericHtmlContentSnippet>")]
    public class GenericHtmlContentSnippet : SnippetControl
    {
        public void Page_Load(object sender, EventArgs e)
        {
            FootnoteExtractor fe = new FootnoteExtractor();
            GlossaryTermExtractor gte = new GlossaryTermExtractor();

            string data = SnippetInfo.Data;
            data = MarkupExtensionProcessor.Instance.Process(data);
            if (PageAssemblyContext.Current.DisplayVersion == DisplayVersions.Print || PageAssemblyContext.Current.DisplayVersion == DisplayVersions.PrintAll)
            {
                if (SnippetInfo.SlotName != "cgvSiteBannerPrint" && SnippetInfo.SlotName != "cgvContentHeader" && SnippetInfo.SlotName != "cgvSiteBanner" && SnippetInfo.SlotName != "cgvLanguageDate" && SnippetInfo.SlotName != "cgvBodyHeader")
                {
                    data = gte.ExtractGlossaryTerms(data);
                    data = fe.Extract(new Regex("<a\\s+?(?:class=\".*?\"\\s+?)*?href=\"(?<extractValue>.*?)\"(?:\\s+?\\w+?=\"(?:.*?)\")*?\\s*?>(?<linkText>.*?)</a>", RegexOptions.IgnoreCase | RegexOptions.Compiled | RegexOptions.Singleline), "extractValue", CancerGov.Common.Extraction.ExtractionTypes.URL, data);
                    if (fe.hashIndex.Count >= 1)
                    {
                        PageAssemblyContext.Current.tableofLinksHash.AddRange(fe.hashIndex);
                    }

                    if (gte.glossaryIds.Count >= 1)
                    {
                        PageAssemblyContext.Current.glossaryIds.AddRange(gte.glossaryIds);

                    }
                    if (gte.glossaryIDHash.Count >= 1)
                        PageAssemblyContext.Current.glossaryIDHash.AddRange(gte.glossaryIDHash);
                    if (gte.glossaryIDHash.Count >= 1)
                        PageAssemblyContext.Current.glossaryTermHash.AddRange(gte.glossaryTermHash);
                    if (gte.terms.Count >= 1)
                    {
                        PageAssemblyContext.Current.terms.AddRange(gte.terms);

                    }
                }

            }
            LiteralControl lit = new LiteralControl(data);
            this.Controls.Add(lit);
        }        
    }
}
