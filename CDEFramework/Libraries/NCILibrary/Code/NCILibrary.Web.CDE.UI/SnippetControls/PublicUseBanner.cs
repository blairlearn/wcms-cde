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
            if (shouldDisplay())
            {
                HtmlData = SnippetInfo.Data;
                Visible = true;
            }
            else
            {
                HtmlData = String.Empty;
                Visible = false;
            }
        }

        public override void RenderControl(HtmlTextWriter writer)
        {
            base.RenderControl(writer);
            HtmlData = MarkupExtensionProcessor.Instance.Process(HtmlData);
            LiteralControl lit = new LiteralControl(HtmlData);
            lit.RenderControl(writer);
        }

        /// <summary>
        /// Checks whether the "Public Use" banner should be displayed.
        /// </summary>
        /// <returns>Returns true if the "publicUse" alternate content version key has been
        /// set, false otherwise.</returns>
        private bool shouldDisplay()
        {
            bool isPubUse = false;

            IPageAssemblyInstruction pgInstruction = PageAssemblyContext.Current.PageAssemblyInstruction;

            // If AlternateContentVersions information is not in the instructions then do not display.
            string[] acvKeys = pgInstruction.AlternateContentVersionsKeys;
            if (acvKeys != null)
            {
                if (acvKeys.Contains("publicUse"))
                {
                    isPubUse = true;
                }
            }
            return isPubUse;
        }
    }
}
