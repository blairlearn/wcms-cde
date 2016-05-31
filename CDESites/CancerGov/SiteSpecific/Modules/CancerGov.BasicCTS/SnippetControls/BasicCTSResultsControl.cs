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
    public class BasicCTSResultsControl : SnippetControl
    {
        private string _index = "clinicaltrials";
        private string _indexType = "trial";
        private string _clusterName = "cts";
        private string _templatePath = "~/VelocityTemplates/BasicCTSResults.vm";

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            //Get Phrase
            string phrase = "Cancer";

            BasicCTSManager basicCTSManager = new BasicCTSManager(_index, _indexType, _clusterName);

            // Create Search Params
            PhraseSearchParam searchParams = new PhraseSearchParam()
            {
                From = 0,
                Size = 10,
                Phrase = phrase
            };

            //Do the search
            var results = basicCTSManager.Search(searchParams);

            // Show Results

            LiteralControl ltl = new LiteralControl(VelocityTemplate.MergeTemplateWithResultsByFilepath(_templatePath, results));
            Controls.Add(ltl);
        }
    }
}
