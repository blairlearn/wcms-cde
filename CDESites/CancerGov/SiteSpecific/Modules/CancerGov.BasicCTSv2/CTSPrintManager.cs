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

namespace CancerGov.ClinicalTrials.Basic.v2
{
    public class CTSPrintManager
    {

        public Guid StorePrintContent(List<String> trialIDs)
        {
            // Retrieve the collections given the ID's
            BasicCTSManager manager = new BasicCTSManager("https://clinicaltrialsapi.cancer.gov");
            List<ClinicalTrial> results = manager.GetMultipleTrials(trialIDs).ToList();

            // Send results to Velocity template
            var formattedResults = FormatPrintResults(results);

            // Save result to cache table
            Guid test = CTSPrintResultsDataManager.SavePrintResult(formattedResults, results.ToString(), Settings.IsLive);

            return test;
        }

        public string FormatPrintResults(IEnumerable<ClinicalTrial> results)
        {
            // Format Print Page HTML by binding results to velocity template for CTS print.
            LiteralControl ltl = new LiteralControl(VelocityTemplate.MergeTemplateWithResultsByFilepath(
                @"~/PublishedContent/VelocityTemplates/BasicCTSPrintResultsv2.vm",
                 new
                 {
                     Results = results
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
