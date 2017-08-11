using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.UI;

using NCI.Text;
using NCI.Web.CDE;
using NCI.Web.CDE.Application;
using NCI.Web.CDE.Modules;
using NCI.Web.CDE.UI;
using NCI.Web.CDE.UI.Configuration;
using CancerGov.ClinicalTrials.Basic.v2.Configuration;
using CancerGov.ClinicalTrials.Basic.v2.DataManagers;
using CancerGov.ClinicalTrials.Basic.v2.SnippetControls;
using CancerGov.ClinicalTrialsAPI;

namespace CancerGov.ClinicalTrials.Basic.v2
{
    public class CTSPrintManager
    {
        private static BasicCTSPageInfo _config = null;

        /// <summary>
        /// Creates a new instance of a CTSPrintManager
        /// </summary>
        /// <param name="config">CTS Config</param>
        public CTSPrintManager(BasicCTSPageInfo config)
        {
            _config = config;
        }

        /// <summary>
        /// Gets the URL for the ClinicalTrials API from BasicClinicalTrialSearchAPISection:GetAPIUrl()
        /// </summary>
        protected string ApiUrl
        {
            get { return BasicClinicalTrialSearchAPISection.GetAPIUrl(); }
        }

        public Guid StorePrintContent(List<String> trialIDs, DateTime date, CTSSearchParams searchTerms)
        {
            // Retrieve the collections given the ID's
            BasicCTSManager manager = new BasicCTSManager(new ClinicalTrialsAPIClient(ApiUrl));
            List<ClinicalTrial> results = manager.GetMultipleTrials(trialIDs).ToList();

            // Send results to Velocity template
            var formattedPrintContent = FormatPrintResults(results, date, searchTerms);

            // Save result to cache table
            Guid guid = CTSPrintResultsDataManager.SavePrintResult(formattedPrintContent, trialIDs, searchTerms, Settings.IsLive);

            if (guid == Guid.Empty)
            {
                // Something went wrong with the save/return from the DB
                ErrorPageDisplayer.RaisePageByCode(this.GetType().ToString(), 500);
                throw new DbConnectionException("Unable to connect to the database. ");
            }

            return guid;
        }

        private string FormatPrintResults(IEnumerable<ClinicalTrial> results, DateTime searchDate, CTSSearchParams searchTerms)
        {
            // Bind results to velocity template
            LiteralControl ltl = new LiteralControl(VelocityTemplate.MergeTemplateWithResultsByFilepath(
                _config.PrintPageTemplatePath,
                 new
                 {
                     Results = results,
                     SearchDate = searchDate.ToString("M/d/yyyy"),
                     Parameters = searchTerms,
                     TrialTools = new TrialVelocityTools()
                 }
            ));

            return (ltl.Text);
        }

        public string GetPrintContent(Guid printID)
        {
            // Call the data manager to retrieve the cached print results based on guid printID
            string printContent = CTSPrintResultsDataManager.RetrieveResult(printID, Settings.IsLive);

            return printContent;
        }


    }
}
