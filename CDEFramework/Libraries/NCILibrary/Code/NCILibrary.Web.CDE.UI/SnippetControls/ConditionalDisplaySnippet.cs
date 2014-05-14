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
    /// This Snippet Template is for displaying conditional message based 
    /// on the state of booleanCondition atColo in web.config nci/web/conditional 
    /// </summary>
    [DefaultProperty("Text")]
    [ToolboxData("<{0}:ConditionalDisplaySnippet runat=server></{0}:ConditionalDisplaySnippet>")]
    public class ConditionalDisplaySnippet : GenericHtmlContentSnippet
    {
        public override bool Visible
        {
            get
            {
                return ConditionalConfig.AtColo;
            }
            set { }
        }
    }
}
