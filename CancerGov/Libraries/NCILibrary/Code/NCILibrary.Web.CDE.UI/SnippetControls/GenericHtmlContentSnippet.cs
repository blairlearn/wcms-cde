using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

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
            string data = SnippetInfo.Data;
            LiteralControl lit = new LiteralControl(data);
            this.Controls.Add(lit);
        }        
    }
}
