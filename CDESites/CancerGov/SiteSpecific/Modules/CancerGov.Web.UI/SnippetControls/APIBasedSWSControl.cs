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
using NCI.Web.CDE;
using NCI.Web.CDE.UI;
using NCI.Web.CDE.Modules;
using NCI.Search.Client;

using Common.Logging;

namespace CancerGov.Web.UI.SnippetControls
{
    /// <summary>
    /// This Snippet Template is for displaying blobs of HTML that Percussion has generated.
    /// </summary>
    [DefaultProperty("Text")]
    [ToolboxData("<{0}:APIBasedSWSControl runat=server></{0}:CGovMainNavControl>")]
    public class APIBasedSWSControl : SnippetControl
    {
        public class SWSControlConfig
        {
            /// <summary>
            /// Gets and sets the path for the velocity template that should be used to render the results.
            /// </summary>
            public string TemplatePath { get; set; }
        }

        private SWSControlConfig _config;

        static ILog log = LogManager.GetLogger(typeof(APIBasedSWSControl));

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);

            // Read the basic CTS page information JSON
            string configStr = this.SnippetInfo.Data;

            try
            {
                if (string.IsNullOrWhiteSpace(configStr))
                {
                    throw new Exception("SWSControlConfig not present in JSON, associate an Application Module item with this page in Percussion");
                }
                configStr = configStr.Trim();
                    
                // Get our TrialListingPageInfo object this is JSON data that includes template and URL paths and result count parameters.
                // It also includes a nested, JSON-formatted string "RequestFilters", which represents the JSON passed in with the API body request - this is
                // deserialized in TrialListingPageControl.
                // TODO: handle all deserialization in once place, if possible. This will avoid having to go through the process twice
                SWSControlConfig searchControlConfig = ModuleObjectFactory<SWSControlConfig>.GetJsonObject(configStr);

                this._config = searchControlConfig;
            }
            catch (Exception ex)
            {
                log.Error("Could not load the search control configuration; check the config info of the Application Module item in Percussion", ex);
                throw ex;
            }            
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            //Get Results
            SiteWideSearchResults results = GetMockResults();

            LiteralControl ltl = new LiteralControl(VelocityTemplate.MergeTemplateWithResultsByFilepath(
                _config.TemplatePath, new
                {
                    Control = this,
                    Parameters = new {
                        Keyword = "This Is Keyword",
                        StartPos = 1,
                        EndPos = 10,
                        TotalNum = 99999
                    },
                    Results = results,
                    BestBets = GetMockBB(),
                    Pager = "PAGER"
                }));
            Controls.Add(ltl);
        }

        private SiteWideSearchResults GetMockResults()
        {
            List<SiteWideSearchResult> rtnResults = new List<SiteWideSearchResult>();

            for (int i = 0; i < 10; i++)
            {
                rtnResults.Add(new SiteWideSearchResult()
                {
                    Title = "Search Result " + i.ToString(),
                    URL = "http://www.cancer.gov/result" + i.ToString(),
                    Description = "Description " + i.ToString(),
                    ContentType = "nciGeneral"
                });
            }

            return new SiteWideSearchResults(525, rtnResults);
        }

        private BestBetResult[] GetMockBB()
        {
            List<BestBetResult> rtnResults = new List<BestBetResult>();

            for (int i = 0; i < 2; i++)
            {
                rtnResults.Add(new BestBetResult()
                {
                    Name = "Best Bets " + i.ToString(),
                    ID = "12345" + i.ToString(),
                    HTML = "nciGeneral",
                    Weight = 10
                });
            }

            return rtnResults.ToArray();
        }

    }
}
