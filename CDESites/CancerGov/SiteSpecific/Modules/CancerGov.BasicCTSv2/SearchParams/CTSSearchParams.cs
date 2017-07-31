using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CancerGov.ClinicalTrials.Basic.v2
{
    /// <summary>
    /// Represents the Search Parameters of a new API-based Clinical Trials Search.
    /// This includes the parameters used for a search, the labels to be displayed,
    /// as well as helpers to determine if a field was used.
    /// </summary>
    public class CTSSearchParams
    {
        int _pageNum = 1;
        int _itemsPerPage = 10;
        int _zipRadius = 100;

        /// <summary>
        /// Gets or sets the main cancer type that was selected.
        /// </summary>
        public TerminologyFieldSearchParam MainType { get; set; }

        /// <summary>
        /// Gets or sets an array of the subtypes for this search definition
        /// </summary>
        public TerminologyFieldSearchParam[] SubTypes { get; set; }

        /// <summary>
        /// Gets or sets an array of the stages for this search definition
        /// </summary>
        public TerminologyFieldSearchParam[] Stages { get; set; }

        /// <summary>
        /// Gets or sets an array of the findings for this search definition
        /// </summary>
        public TerminologyFieldSearchParam[] Findings { get; set; }

        /// <summary>
        /// Gets or sets the age for this search definition
        /// </summary>
        public int Age { get; set; }

        /// <summary>
        /// Gets or sets the gender for this search definition
        /// </summary>
        public String Gender { get; set; }

        /// <summary>
        /// Gets or sets the Phrase/Keyword used in the search
        /// </summary>
        public String Phrase { get; set; }

        /// <summary>
        /// Gets or sets the location type value
        /// </summary>
        public String Location { get; set; }

        /// <summary>
        /// Gets or sets the zip code value
        /// TODO: verify how this should work with updated API
        /// </summary>
        public String ZipCode { get; set; }

        /// <summary>
        /// Gets or sets the zip code search radius
        /// TODO: verify how this should work with updated API
        /// </summary>
        public int ZipRadius { get; set; }

        /// <summary>
        /// Gets or sets the country used in the search
        /// </summary>
        public String Country { get; set; }

        /// <summary>
        /// Gets or sets the state used in the search
        /// </summary>
        public LabelledSearchParam State { get; set; }

        /// <summary>
        /// Gets or sets the city used in the search
        /// </summary>
        public String City { get; set; }

        /// <summary>
        /// Gets or sets the Hospital used in the search
        /// </summary>
        public String Hospital { get; set; }

        /// <summary>
        /// Gets or sets whether to filter only trials At NIH in the search
        /// </summary>
        public bool AtNIH { get; set; }

        /// <summary>
        /// Gets or sets an array of the trial types in the search
        /// </summary>
        public LabelledSearchParam[] TrialTypes { get; set; }

        /// <summary>
        /// Gets or sets an array of the drugs for this search definition
        /// </summary>
        public TerminologyFieldSearchParam[] Drugs { get; set; }

        /// <summary>
        /// Gets or sets an array of the other treatments for this search definition
        /// </summary>
        public TerminologyFieldSearchParam[] OtherTreatments { get; set; }

        /// <summary>
        /// Gets or sets an array of the trial phases in the search
        /// </summary>
        public LabelledSearchParam[] TrialPhases { get; set; }

        /// <summary>
        /// Gets or sets an array of the trial IDs in the search
        /// </summary>
        public string[] TrialIDs { get; set; }

        /// <summary>
        /// Gets or sets the Investigator used in the search
        /// </summary>
        public String Investigator { get; set; }

        /// <summary>
        /// Gets or sets the lead org used in the search
        /// </summary>
        public String LeadOrg { get; set; }

        /// <summary>
        /// Gets or sets the page number for the search
        /// </summary>
        public int Page
        {
            get { return _pageNum; }
            set { _pageNum = value; }

        }

        /// <summary>
        /// Gets or sets the items per page for the search
        /// </summary>
        public int ItemsPerPage
        {
            get { return _itemsPerPage; }
            set { _itemsPerPage = value; }
        }

        /// <summary>
        /// Gets or sets the results link flag for the search
        /// </summary>
        public int ResultsLinkFlag { get; set; }

        /// <summary>
        /// Gets or sets an array of CTS Search Param Errors to identify when a parse error occurred.
        /// </summary>
        public List<CTSSearchParamError> ParseErrors { get; set; }

    }
}
