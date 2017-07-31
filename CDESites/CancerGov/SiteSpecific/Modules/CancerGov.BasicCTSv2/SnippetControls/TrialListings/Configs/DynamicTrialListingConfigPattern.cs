using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CancerGov.ClinicalTrials.Basic.v2.SnippetControls.Configs
{
    public class DynamicTrialListingConfigPattern
    {
        /// <summary>
        /// The page title for the dynamic trial listing pattern.
        /// </summary>
        public string PageTitle { get; set; }

        /// <summary>
        /// The browser title for the dynamic trial listing pattern.
        /// </summary>
        public string BrowserTitle { get; set; }

        /// <summary>
        /// The meta description for the dynamic trial listing pattern.
        /// </summary>
        public string MetaDescription { get; set; }

        /// <summary>
        /// The intro text for the dynamic trial listing pattern.
        /// </summary>
        public string IntroText { get; set; }

        /// <summary>
        /// The HTML for "no trials found" for the dynamic trial listing pattern.
        /// </summary>
        public string NoTrialsHtml { get; set; }
    }
}
