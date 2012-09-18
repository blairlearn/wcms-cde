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
using NCI.Web.CDE.Conditional;

namespace NCI.Web.CDE.UI.SnippetControls
{
    /// <summary>
    /// This Snippet Template is for displaying conditional message based on state of a web.config 
    /// </summary>
    [DefaultProperty("Text")]
    [ToolboxData("<{0}:ConditionalDisplaySnippet runat=server></{0}:ConditionalDisplaySnippet>")]
    public class ConditionalDisplaySnippet : GenericHtmlContentSnippet
    {
        public override void RenderControl(HtmlTextWriter writer)
        {
            if (ConditionalConfig.AtColo)
            {
                base.RenderControl(writer);
            }
            else
            {
                base.Visible = false;
            }
        }

    }
}
