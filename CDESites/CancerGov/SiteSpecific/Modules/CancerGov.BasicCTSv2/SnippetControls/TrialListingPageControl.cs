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
using Common.Logging;

using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

using CancerGov.ClinicalTrialsAPI;
using CancerGov.ClinicalTrials.Basic.v2.SnippetControls.Configs;
using NCI.Web;
using NCI.Web.CDE;
using NCI.Web.CDE.Application;
using NCI.Web.CDE.Modules;
using NCI.Web.CDE.UI;
using NCI.Web.CDE.WebAnalytics;

namespace CancerGov.ClinicalTrials.Basic.v2.SnippetControls
{
    /// <summary>
    /// Page control for Trial Listing pages.
    /// This is the code behind for TrialListingPage.ascx snippet template.
    /// </summary>
    public class TrialListingPageControl : BaseTrialListingControl
    {
        /// <summary>
        /// Set logging for this class.
        /// </summary>
        static ILog log = LogManager.GetLogger(typeof(TrialListingPageControl));

        protected override Type GetConfigType()
        {
            return typeof(TrialListingConfig);
        }

        /// <summary>
        /// Implementation of base trial listing page's Trial Query
        /// </summary>
        /// <returns>A JObject of request filters from the TrialListingConfig.</returns>
        protected sealed override JObject GetTrialQuery()
        {
            TrialListingConfig manualConfig = (TrialListingConfig)this.Config;
            return manualConfig.RequestFilters;
        }

        /// <summary>
        /// Implementation of base trial listing page's InternalGetNoTrialsHtml
        /// </summary>
        /// <returns>The NoTrialsHTML string from the TrialListingConfig.</returns>
        protected override String InternalGetNoTrialsHtml()
        {
            TrialListingConfig manualConfig = (TrialListingConfig)this.Config;
            return manualConfig.NoTrialsHTML;
        }

        /// <summary>
        /// Set default pageLoad analytics for this page
        /// </summary>
        protected override void SetAnalytics()
        {
            string val = "clinicaltrials_custom";
            string desc = "Clinical Trials: Custom";

            // Set event
            this.PageInstruction.SetWebAnalytics(WebAnalyticsOptions.Events.event2, wbField =>
            {
                wbField.Value = WebAnalyticsOptions.Events.event2.ToString();
            });

            // Set props
            this.PageInstruction.SetWebAnalytics(WebAnalyticsOptions.Props.prop11, wbField =>
            {
                wbField.Value = val;
            });
            this.PageInstruction.SetWebAnalytics(WebAnalyticsOptions.Props.prop62, wbField =>
            {
                wbField.Value = desc;
            });

            // Set eVars
            this.PageInstruction.SetWebAnalytics(WebAnalyticsOptions.eVars.evar10, wbField =>
            {
                wbField.Value = this.Config.DefaultItemsPerPage.ToString();
            });
            this.PageInstruction.SetWebAnalytics(WebAnalyticsOptions.eVars.evar11, wbField =>
            {
                wbField.Value = val;
            });
            this.PageInstruction.SetWebAnalytics(WebAnalyticsOptions.eVars.evar47, wbField =>
            {
                wbField.Value = val;
            });
            this.PageInstruction.SetWebAnalytics(WebAnalyticsOptions.eVars.evar62, wbField =>
            {
                wbField.Value = desc;
            });

        }
    }
}
