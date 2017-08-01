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

namespace CancerGov.ClinicalTrials.Basic.v2.SnippetControls.Search
{
    /// <summary>
    /// Represents the new CTAPI Driven Results page for Basic & Advanced.
    /// </summary>
    public class APICTSResultsControl : BaseMgrAPICTSControl
    {
        /// <summary>
        /// Gets the path to the template
        /// </summary>
        /// <returns></returns>
        protected override string GetTemplatePath()
        {
            return this.Config.ResultsPageTemplatePath;
        }

        /// <summary>
        /// Goes and fetches the data from the API & Returns the results to base class to be bound to the template.
        /// </summary>
        /// <returns></returns>
        protected override object GetDataForTemplate()
        {

            //TODO: Get the page number & items per page

            ClinicalTrialsCollection results = CTSManager.Search(SearchParams);

            //TODO: Setup field filters

            //TODO: Add Analytics Filters?

            //Return the object for binding.
            return new
            {
                Results = results,
                Control = this,
                TrialTools = new TrialVelocityTools()
            };
        }
    }
}
