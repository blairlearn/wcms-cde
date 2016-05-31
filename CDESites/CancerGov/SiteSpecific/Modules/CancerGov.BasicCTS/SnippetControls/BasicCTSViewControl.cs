using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using NCI.Web.CDE.UI;

namespace CancerGov.ClinicalTrials.Basic.SnippetControls
{
    public class BasicCTSViewControl : SnippetControl
    {
        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            // Get ID
            string nctid = Request.Params["id"];
            if (String.IsNullOrWhiteSpace(nctid))
            {
                this.Controls.Add(new LiteralControl("NeedID"));
                return;
            }

            if (!Regex.IsMatch(nctid, "^NCT[0-9]+$"))
            {
                this.Controls.Add(new LiteralControl("Invalid ID"));
                return;
            }


            // Get Trial by ID
            // Show Trial



        }
    }
}
