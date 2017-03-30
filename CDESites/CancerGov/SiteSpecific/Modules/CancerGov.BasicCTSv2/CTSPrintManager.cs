using System;
using System.Web;
using System.Web.UI;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Threading.Tasks;
using System.Configuration;
using NCI.Web.CDE.Application;
using CancerGov.ClinicalTrials.Basic.v2.DataManagers;
using NCI.Text;
using NCI.Web.CDE;
using NCI.Web.CDE.UI;
using NCI.Web.CDE.Modules;
using NCI.Web.CDE.UI.Configuration;
using CancerGov.ClinicalTrialsAPI;
using CancerGov.ClinicalTrials.Basic.v2.SnippetControls;

namespace CancerGov.ClinicalTrials.Basic.v2
{
    public class CTSPrintManager
    {

        public Guid StorePrintContent(List<string> trialIDs, DateTime date, CTSSearchParams searchTerms)
        {
            // Retrieve the collections given the ID's
            BasicCTSManager manager = new BasicCTSManager("https://clinicaltrialsapi.cancer.gov");
            List<ClinicalTrial> results = manager.GetMultipleTrials(trialIDs).ToList();

            // Send results to Velocity template
            var formattedPrintContent = FormatPrintResults(results, date, searchTerms);

            // Save result to cache table
            Guid guid = CTSPrintResultsDataManager.SavePrintResult(formattedPrintContent, searchTerms.ToString(), Settings.IsLive);

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
            // convert description to pretty description
            foreach (var trial in results)
            {
                var desc = trial.DetailedDescription;
                trial.DetailedDescription = new TrialVelocityTools().GetPrettyDescription(trial);
            }
            if (searchTerms.ZipCode != null)
            {
                BasicCTSManager manager = new BasicCTSManager("https://clinicaltrialsapi.cancer.gov");
                searchTerms.GeoCode = manager.GetZipLookupForZip(searchTerms.ZipCode).GeoCode;
            }

            // Bind results to velocity template
            LiteralControl ltl = new LiteralControl(VelocityTemplate.MergeTemplateWithResultsByFilepath(
                @"~/PublishedContent/VelocityTemplates/BasicCTSPrintResultsv2.vm",
                 new
                 {
                     Results = results,
                     SearchDate = searchDate.ToString("MM/d/yyyy"),
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
