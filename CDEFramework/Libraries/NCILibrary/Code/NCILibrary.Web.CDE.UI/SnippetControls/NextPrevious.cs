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
using NCI.Web.CDE.UI;
using NCI.Web.CDE;


namespace NCI.Web.CDE.UI.SnippetControls
{
    /// <summary>
    /// This Snippet Template is for creating buttons for previous/next posts for Blog Post Content Type.
    /// </summary>
    [DefaultProperty("Text")]
    [ToolboxData("<{0}:NextPrevious runat=server></{0}:NextPrevious>")]
    public class NextPrevious : SnippetControl
    {


    }
}
