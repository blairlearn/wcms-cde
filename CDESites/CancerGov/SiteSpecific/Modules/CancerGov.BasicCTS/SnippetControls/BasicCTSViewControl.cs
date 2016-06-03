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
    public partial class BasicCTSViewControl : BasicCTSBaseControl
    {
        //private string _index = "clinicaltrials";
        //private string _indexType = "trial";
        //private string _clusterName = "SearchCluster";
        //private string _templatePath = "~/VelocityTemplates/BasicCTSView.vm";

        public ZipLookup ZipLookup { get; set; }
        public int ZipRadius {
            get { return BasicCTSPageInfo.DefaultZipProximity;  }
        }

        private BasicCTSManager _basicCTSManager = null;

        /// <summary>
        /// Determines if the current search has a Zip or not.
        /// </summary>
        /// <returns></returns>
        public bool HasZip()
        {
            return ZipLookup != null;
        }

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);

            _basicCTSManager = new BasicCTSManager(
                BasicCTSPageInfo.SearchIndex,
                BasicCTSPageInfo.TrialIndexType,
                BasicCTSPageInfo.MenuTermIndexType,
                BasicCTSPageInfo.GeoLocIndexType,
                BasicCTSPageInfo.SearchClusterName
            );

        }

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

            string zip = this.ParmAsStr(ZIP_PARAM, string.Empty);
            int zipProximity = this.ParmAsInt(ZIPPROX_PARAM, BasicCTSPageInfo.DefaultZipProximity); //In miles

            if (!string.IsNullOrWhiteSpace(zip))
            {
                ZipLookup = _basicCTSManager.GetZipLookupForZip(zip);
            }
    
            


            BasicCTSManager basicCTSManager = new BasicCTSManager(
                BasicCTSPageInfo.SearchIndex,
                BasicCTSPageInfo.TrialIndexType,
                BasicCTSPageInfo.MenuTermIndexType,
                BasicCTSPageInfo.GeoLocIndexType,
                BasicCTSPageInfo.SearchClusterName
            );

            // Get Trial by ID
            var trial = _basicCTSManager.Get(nctid);

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

            LiteralControl ltl = new LiteralControl(VelocityTemplate.MergeTemplateWithResultsByFilepath(
                    BasicCTSPageInfo.DetailedViewPageTemplatePath, 
                    new
                    {
                        Trial = trial,
                        Control = this
                    }
                )
            );
            Controls.Add(ltl);
        }
    }
}
