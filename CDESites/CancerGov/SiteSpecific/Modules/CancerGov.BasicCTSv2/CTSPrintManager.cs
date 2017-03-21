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

        public Guid StorePrintContent(string formattedPrintContent, SearchTerms searchTerms)
        {
            // Retrieve the collections given the ID's
            BasicCTSManager manager = new BasicCTSManager("https://clinicaltrialsapi.cancer.gov");

            // Save result to cache table
            Guid guid = CTSPrintResultsDataManager.SavePrintResult(formattedPrintContent, searchTerms.ToString(), Settings.IsLive);

            return guid;
        }

        public string FormatPrintResults(IEnumerable<ClinicalTrial> results, DateTime searchDate, SearchTerms searchTerms)
        {
            // convert description to pretty description
            foreach (var trial in results)
            {
                var desc = trial.DetailedDescription;
                trial.DetailedDescription = new TrialVelocityTools().GetPrettyDescription(trial);
            }
            // Show Results
            LiteralControl ltl = new LiteralControl(VelocityTemplate.MergeTemplateWithResultsByFilepath(
                @"~/PublishedContent/VelocityTemplates/BasicCTSPrintResultsv2.vm",
                 new
                 {
                     Results = results,
                     SearchDate = searchDate.ToString("d/MM/yyyy"),
                     SearchTerms = searchTerms
                 }
            ));

            File.WriteAllText(@"C:\Development\misc\output.html", ltl.Text);

            return (ltl.Text);
        }

        public string GetPrintContent(Guid printID)
        {
            string printContent = CTSPrintResultsDataManager.RetrieveResult(printID, Settings.IsLive);

            return printContent;
        }

    }
}
