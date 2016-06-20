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
using NCI.Web;



namespace CancerGov.ClinicalTrials.Basic.SnippetControls
{
    public partial class BasicCTSViewControl : BasicCTSBaseControl
    {
        //private string _index = "clinicaltrials";
        //private string _indexType = "trial";
        //private string _clusterName = "SearchCluster";
        //private string _templatePath = "~/VelocityTemplates/BasicCTSView.vm";
        private const string _phaseI = "Phase I";
        private const string _phaseII = "Phase II";
        private const string _phaseIII = "Phase III";
        private const string _phaseIV = "Phase IV";

        private const string _phaseI_II = "Phase I/II";
        private const string _phaseII_III = "Phase II/III";

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

        public int GetShowAll()
        {
            return ParmAsInt("all", -1);
        }

        protected string GetGlossifiedTrialPhase(string[] phases)
        {
            int phaseBits = 0x00;
            List<string> glossPhases = new List<string>();

            foreach (string phase in phases)
            {
                switch (phase)
                {
                    case _phaseI:
                        phaseBits |= 0x01;
                        break;
                    case _phaseII:
                        phaseBits |= 0x02;
                        break;
                    case _phaseIII:
                        phaseBits |= 0x04;
                        break;
                    case _phaseIV:
                        phaseBits |= 0x08;
                        break;
                    default:
                        glossPhases.Add(phase);
                        break;
                }
            }


            SortedDictionary<int, string> termIds = new SortedDictionary<int, string>();

            switch (phaseBits)
            {
                case 0x00: // no phases recognized, just use glossPhases
                    break;
                case 0x01: //"phase I":
                    termIds.Add(45830, _phaseI);
                    break;
                case 0x02: //"phase II":
                    termIds.Add(45831, _phaseII);
                    break;
                case 0x03: //"phase I/II":
                    termIds.Add(45832, _phaseI_II);
                    break;
                case 0x04: //"phase III":
                    termIds.Add(45833, _phaseIII);
                    break;
                case 0x06: //"phase II/III":
                    termIds.Add(45834, _phaseII_III);
                    break;
                case 0x08: //"phase IV":
                    termIds.Add(45835, _phaseIV);
                    break;
                default: // unknown, combine all phases
                    glossPhases.Add("unknown phase pairing: " + string.Join(", ", phases) + " (bits ="  + phaseBits + ")");
                    return string.Join(", ", glossPhases);
            }

            foreach (KeyValuePair<int, string> pair in termIds)
            {
                glossPhases.Add("<a onclick=\"javascript:popWindow('defbyid','CDR00000" + pair.Key.ToString() + "&amp;version=Patient&amp;language=English'); return false;\" " +
                "href=\"/Common/PopUps/popDefinition.aspx?id=CDR00000" + pair.Key.ToString() + "&amp;version=Patient&amp;language=English\" " +
                "class=\"definition\">" + pair.Value + "</a>");
            }

            return string.Join(", ", glossPhases);
        }

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);

            _basicCTSManager = new BasicCTSManager();

        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            // Get ID
            string nctid = Request.Params["id"];
            if (String.IsNullOrWhiteSpace(nctid))
            {
                throw new HttpException(404, "Missing trial ID.");
            }


            nctid = nctid.Trim();

            if (!Regex.IsMatch(nctid, "^NCT[0-9]+$"))
            {
                throw new HttpException(404, "Invalid trial ID.");
            }

            string zip = this.ParmAsStr(ZIP_PARAM, string.Empty);
            int zipProximity = this.ParmAsInt(ZIPPROX_PARAM, BasicCTSPageInfo.DefaultZipProximity); //In miles

            if (!string.IsNullOrWhiteSpace(zip))
            {
                ZipLookup = _basicCTSManager.GetZipLookupForZip(zip);
            }
    
            


            BasicCTSManager basicCTSManager = new BasicCTSManager();

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
                    data.Value = "View Clinical Trial " + trial.NCTID;
                else
                    data.Value = "View Clinical Trial";

            });

            PageInstruction.AddUrlFilter("CurrentUrl", (name, url) =>
            {
                url.QueryParameters.Add("id", nctid);
                url.QueryParameters.Add("z", zip);
                if (GetShowAll() > -1)
                {
                    url.QueryParameters.Add("all", GetShowAll().ToString());
                }
            });

            PageInstruction.AddUrlFilter("ShowNearbyUrl", (name, url) =>
            {
                url.SetUrl(PageInstruction.GetUrl("CurrentUrl").ToString());
                if (url.QueryParameters.ContainsKey("all"))
                {
                    url.QueryParameters["all"] = "0";
                }
                else
                {
                    url.QueryParameters.Add("all", "0");
                }
            });

            PageInstruction.AddUrlFilter("ShowAllUrl", (name, url) =>
            {
                url.SetUrl(PageInstruction.GetUrl("CurrentUrl").ToString());
                if (url.QueryParameters.ContainsKey("all"))
                {
                    url.QueryParameters["all"] = "1";
                }
                else
                {
                    url.QueryParameters.Add("all", "1");
                }
            });

            LiteralControl ltl = new LiteralControl(VelocityTemplate.MergeTemplateWithResultsByFilepath(
                    BasicCTSPageInfo.DetailedViewPageTemplatePath, 
                    new
                    {
                        Trial = trial,
                        Control = this,
                        GlossifiedPhase = GetGlossifiedTrialPhase(trial.ProtocolPhases)
                    }
                )
            );
            Controls.Add(ltl);
        }
    }
}
