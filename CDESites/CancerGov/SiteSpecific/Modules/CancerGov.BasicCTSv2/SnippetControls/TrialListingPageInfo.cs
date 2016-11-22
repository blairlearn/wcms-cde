using System.Xml.Schema;
using System.Xml.Serialization;
using Newtonsoft.Json.Linq;

namespace CancerGov.ClinicalTrials.Basic.v2
{
    /// <summary>
    /// This class represents the data that is set in a Trial Listing Page content item. 
    /// The data is JSON-formatte wrapped in a CDATA block within the content item's Page Instruction snippet info.
    /// The generation of the data may change in a future release, but for now, this requires a properly-formatted 
    /// JSON string entered as in the "Config:" field of an Application Module Page content item. (2016-11-17)
    /// </summary>
    public class TrialListingPageInfo
    {

        /// <summary>
        /// The path to the template to use.
        /// </summary>
        public string ResultsPageTemplatePath { get; set; }

        /// <summary>
        /// The pretty url of the results page.
        /// </summary>
        public string ResultsPagePrettyUrl { get; set; }

        /// <summary>
        /// The path to the template to use.
        /// </summary>
        public string DetailedViewPageTemplatePath { get; set; }

        /// <summary>
        /// The pretty url of the detailed view page
        /// </summary>
        public string DetailedViewPagePrettyUrl { get; set; }

        /// <summary>
        /// The default number of search result items per page.
        /// </summary>
        public int DefaultItemsPerPage { get; set; }

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
