using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using NCI.Web.CDE.UI;


namespace CancerGov.Web.SnippetTemplates.BasicCTS
{
    public partial class BasicViewTrial : SnippetControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            // Get ID
            string nctid = Request.Params["id"];
            if (String.IsNullOrWhiteSpace(nctid)) {
                this.Controls.Add(new LiteralControl("NeedID"));
                return;
            }
                       
            if (!Regex.IsMatch(nctid, "^NCT[0-9]+$")) {
                this.Controls.Add(new LiteralControl("Invalid ID"));
                return;
            }


            // Get Trial by ID
            // Show Trial
        }

    }
}