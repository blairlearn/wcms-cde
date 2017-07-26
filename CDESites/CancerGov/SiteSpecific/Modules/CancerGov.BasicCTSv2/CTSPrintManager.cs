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

        /// <summary>
        /// Gets the URL for the ClinicalTrials API from BasicClinicalTrialSearchAPISection:GetAPIUrl()
        /// </summary>
        protected string ApiUrl
        {
            get { return BasicClinicalTrialSearchAPISection.GetAPIUrl(); }
        }

        public Guid StorePrintContent(List<String> trialIDs, DateTime date, CTSPrintSearchParams searchTerms)
        {
            // Retrieve the collections given the ID's
            BasicCTSManager manager = new BasicCTSManager(ApiUrl);
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

        private string FormatPrintResults(IEnumerable<ClinicalTrial> results, DateTime searchDate, CTSPrintSearchParams searchTerms)
        {
            // convert description to pretty description
            foreach (var trial in results)
            {
                var desc = trial.DetailedDescription;
                if (!string.IsNullOrWhiteSpace(desc))
                {
                    trial.DetailedDescription = new TrialVelocityTools().GetPrettyDescription(trial);
                }
            }
            if (searchTerms.ZipCode != null)
            {
                BasicCTSManager manager = new BasicCTSManager(ApiUrl);
                searchTerms.GeoCode = manager.GetZipLookupForZip(searchTerms.ZipCode).GeoCode;
            }

            // Bind results to velocity template
            LiteralControl ltl = new LiteralControl(VelocityTemplate.MergeTemplateWithResultsByFilepath(
                @"~/PublishedContent/VelocityTemplates/BasicCTSPrintResultsv2.vm",
                 new
                 {
                     Results = results,
                     SearchDate = searchDate.ToString("M/d/yyyy"),
                     SearchTerms = searchTerms,
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
