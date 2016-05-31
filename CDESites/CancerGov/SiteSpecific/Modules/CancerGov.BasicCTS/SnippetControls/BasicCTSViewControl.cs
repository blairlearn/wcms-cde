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
using NCI.Web.CDE.Modules;



namespace CancerGov.ClinicalTrials.Basic.SnippetControls
{
    public class BasicCTSViewControl : SnippetControl
    {
        private string _index = "clinicaltrials";
        private string _indexType = "trial";
        private string _clusterName = "cts";
        private string _templatePath = "~/VelocityTemplates/BasicCTSView.vm";

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

            nctid = nctid.Trim();

            if (!Regex.IsMatch(nctid, "^NCT[0-9]+$"))
            {
                this.Controls.Add(new LiteralControl("Invalid ID"));
                return;
            }


            BasicCTSManager basicCTSManager = new BasicCTSManager(_index, _indexType, _clusterName);

            // Get Trial by ID
            var trial = basicCTSManager.Get(nctid);

            if (trial == null)
                throw new HttpException(404, "Trial cannot be found.");

            // Show Trial

            // Copying the Title & Short Title logic from Advanced Form
            //set the page title as the protocol title
            PageInstruction.AddFieldFilter("long_title", (fieldName, data) =>
            {
                data.Value = trial.BriefTitle;
            });

            PageInstruction.AddFieldFilter("short_title", (fieldName, data) =>
            {
                //Eh, When would this happen???
                if (!string.IsNullOrWhiteSpace(trial.NCTID))
                    data.Value = trial.NCTID + " Clinical Trial";

            });

            LiteralControl ltl = new LiteralControl(VelocityTemplate.MergeTemplateWithResultsByFilepath(_templatePath, trial));
            Controls.Add(ltl);
        }
    }
}
