using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Schema;
using System.Xml.Serialization;
using Newtonsoft.Json.Linq;

namespace CancerGov.ClinicalTrials.Basic.v2.SnippetControls.Configs
{
    class TrialListingConfig : BaseTrialListingConfig
    {
        /// <summary>
        /// The path to the template to use.
        /// </summary>
        public string DetailedViewPageTemplatePath { get; set; }

        /// <summary>
        /// Nested JSON-formatted string representing search filters
        /// </summary>
        public JObject RequestFilters { get; set; }

        /// <summary>
        /// Minimum number of results to return.
        /// </summary>
        public int ListingMinResults { get; set; }

        /// <summary>
        /// Maximum number of results to return.
        /// </summary>
        public int ListingMaxResults { get; set; }

        /// <summary>
        /// HTML block for page with no trials
        /// </summary>
        public string NoTrialsHTML { get; set; }
    }
}
