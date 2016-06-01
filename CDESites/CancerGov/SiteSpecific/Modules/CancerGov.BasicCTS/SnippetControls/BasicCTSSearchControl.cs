using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using NCI.Web.CDE.Modules;
using NCI.Web.CDE.UI;

namespace CancerGov.ClinicalTrials.Basic.SnippetControls
{
    public class BasicCTSSearchControl : SnippetControl
    {
        private string _templatePath = "~/VelocityTemplates/BasicCTSSearch.vm";

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            LiteralControl ltl = new LiteralControl(VelocityTemplate.MergeTemplateWithResultsByFilepath(_templatePath, String.Empty));
            Controls.Add(ltl);
        }
    }
}
