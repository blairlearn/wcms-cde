using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CancerGov.ClinicalTrials.Basic.v2.SnippetControls.Configs
{
    public abstract class BaseTrialListingConfig
    {
        /// <summary>
        /// The path to the template to use.
        /// </summary>
        public string ResultsPageTemplatePath { get; set; }

        // <summary>
        /// The default number of search result items per page.
        /// </summary>
        public int DefaultItemsPerPage { get; set; }

        /// <summary>
        /// The detailed view page pretty url formatter.
        /// </summary>
        public string DetailedViewPagePrettyUrlFormatter { get; set; }
    }
}
