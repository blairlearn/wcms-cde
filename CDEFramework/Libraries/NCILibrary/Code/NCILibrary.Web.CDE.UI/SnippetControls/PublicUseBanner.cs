using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Text.RegularExpressions;

using NCI.Text;

namespace NCI.Web.CDE.UI.SnippetControls
{
    /// <summary>
    /// This Snippet Template is for displaying blobs of HTML that Percussion has generated.
    /// </summary>
    [DefaultProperty("Text")]
    [ToolboxData("<{0}:PublicUseBanner runat=server></{0}:PublicUseBanner>")]
    public class PublicUseBanner : SnippetControl
    {
        string _htmlData = String.Empty;

        public string HtmlData
        {
            get { return _htmlData; }
            set { _htmlData = value; }
        }

        public void Page_Load(object sender, EventArgs e)
        {
            if (displayPublicUse() == "true")
                HtmlData = SnippetInfo.Data;
            else
                HtmlData = String.Empty;
        }

        public override void RenderControl(HtmlTextWriter writer)
        {
            base.RenderControl(writer);
            HtmlData = MarkupExtensionProcessor.Instance.Process(HtmlData);
            LiteralControl lit = new LiteralControl(HtmlData);
            lit.RenderControl(writer);
        }
        /// <summary>
        /// Process the public banner XML field determines if the public archive 
        /// banner should be displayed or not.
        /// </summary>
        /// <param name="snippetXmlData">The xml fragment which contains pageoptions information.</param>
        private string displayPublicUse()
        {

            IPageAssemblyInstruction pgInstruction = PageAssemblyContext.Current.PageAssemblyInstruction;
            // If AlternateContentVersions information is not in the instructions then do not create 
            // the PageOptions box.
            string[] acvKeys = pgInstruction.AlternateContentVersionsKeys;
            string isPubUse = "false";
            if (acvKeys != null)
            {
                if (pgInstruction.AlternateContentVersionsKeys.Contains("publicUse"))
                {
                    isPubUse = "true";
                }
            }
            return isPubUse;
        }
    }
}
