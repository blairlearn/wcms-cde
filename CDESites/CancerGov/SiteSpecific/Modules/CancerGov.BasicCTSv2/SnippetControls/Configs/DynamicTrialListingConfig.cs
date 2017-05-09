using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CancerGov.ClinicalTrials.Basic.v2.SnippetControls.Configs
{
    public class DynamicTrialListingConfig : BaseTrialListingConfig
    {
        /// <summary>
        /// A dictionary for dynamic trial listing pattern names and patterns.
        /// </summary>
        public Dictionary<string, DynamicTrialListingConfigPattern> DynamicListingPatterns;
    }
}
