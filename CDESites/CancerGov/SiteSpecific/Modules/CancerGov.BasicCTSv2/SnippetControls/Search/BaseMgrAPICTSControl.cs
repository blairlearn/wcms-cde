using System;
using System.Configuration;
using System.Text.RegularExpressions;
using System.Web;
using CancerGov.ClinicalTrials.Basic.v2.Configuration;
using Common.Logging;
using NCI.Web;
using NCI.Web.CDE.Modules;
using NCI.Web.CDE.UI;
using CancerGov.ClinicalTrialsAPI;

namespace CancerGov.ClinicalTrials.Basic.v2.SnippetControls
{
    /// <summary>
    /// This is the base control for anything that needs to interact with the CTAPI (e.g. Results and Details view)
    /// </summary>
    public abstract class BaseMgrAPICTSControl : BaseAPICTSControl
    {
        static ILog log = LogManager.GetLogger(typeof(BaseMgrAPICTSControl));

        //An instance of the BasicCTSManager for interacting with the CTSAPI
        protected BasicCTSManager CTSManager { get; private set; }
        protected CTSSearchParams SearchParams { get; private set; }

        protected override void Init()
        {
            base.Init();

            //////////////////////////////
            // Create an instance of a BasicCTSManager.
            string apiURL = BasicClinicalTrialSearchAPISection.GetAPIUrl();
            if (string.IsNullOrEmpty(apiURL))
            {
                string err = String.Format("Could not load APIURL for {0}", this.GetType().ToString());
                log.Error(err);
                throw new Exception(err);
            }
            CTSManager = new BasicCTSManager(new ClinicalTrialsAPIClient(apiURL));

            /////////////////////////////
            // Parse the Query to get the search params.
            try
            {
                CTSSearchParamFactory factory = new CTSSearchParamFactory(DynamicTrialListingMapping.Instance);
                SearchParams = factory.Create(this.Request.Url.Query);
            }
            catch (Exception ex)
            {
                log.Error("could not parse the CTS search parameters", ex);
                throw ex;
            }
        }

    }
}
