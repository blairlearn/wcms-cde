using System.Xml.Schema;
using System.Xml.Serialization;

namespace CancerGov.ClinicalTrials.Basic.v2
{
    /// <summary>
    /// This class defines the properties of search result. Like the prettyUrl of the 
    /// search results page. This information should be made avaliable in the instruction 
    /// that defines the search page.
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
        public string RequestFilters { get; set; }

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
