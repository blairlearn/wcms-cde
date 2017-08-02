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
        /// <summary>
        /// Identifies which fields of the form were used.
        /// </summary>
        FormFields _usedFields = FormFields.None;

        TerminologyFieldSearchParam _mainType           = null;
        TerminologyFieldSearchParam[] _subTypes         = { };
        TerminologyFieldSearchParam[] _stages           = { };
        TerminologyFieldSearchParam[] _findings         = { };
        TerminologyFieldSearchParam[] _drugs            = { };
        TerminologyFieldSearchParam[] _otherTreatments  = { };

        LabelledSearchParam[] _trialTypes   = { };
        LabelledSearchParam[] _trialPhases  = { };

        //int _pageNum                = 1;
        //int _itemsPerPage           = 10;
        ResultsLinkType _resultsLinkFlag = ResultsLinkType.Unknown;
        int _age                    = 0;
        string _gender              = string.Empty;
        string _phrase              = string.Empty;
        string _investigator        = string.Empty;
        string _leadOrg             = string.Empty;
        string[] _trialIDs          = { };
        LocationType _locationType  = LocationType.None;

        LocationSearchParams _locationParams = null;

        /// <summary>
        /// Identified is a field within this location parameter is set.
        /// </summary>
        /// <param name="field">The field to check</param>
        /// <returns>true if set, false if not.</returns>
        public bool IsFieldSet(FormFields field)
        {
            return (_usedFields & field) == field;
        }

        /// <summary>
        /// Determines if the parameters had parse errors
        /// </summary>
        /// <returns></returns>
        public bool HasInvalidParams()
        {
            return this.ParseErrors.Count > 0;
        }

        /// <summary>
        /// Gets or sets the main cancer type that was selected.
        /// </summary>
        public TerminologyFieldSearchParam MainType
        {
            get { return _mainType; }
            set { _mainType = value; _usedFields |= FormFields.MainType; }
        }

        /// <summary>
        /// Gets or sets an array of the subtypes for this search definition
        /// </summary>
        public TerminologyFieldSearchParam[] SubTypes {
            get { return _subTypes; }
            set { _subTypes = value; _usedFields |= FormFields.SubTypes; }
        }

        /// <summary>
        /// Gets or sets an array of the stages for this search definition
        /// </summary>
        public TerminologyFieldSearchParam[] Stages {
            get { return _stages; }
            set { _stages = value; _usedFields |= FormFields.Stages; }
        }

        /// <summary>
        /// Gets or sets an array of the findings for this search definition
        /// </summary>
        public TerminologyFieldSearchParam[] Findings {
            get { return _findings; }
            set { _findings = value; _usedFields |= FormFields.Findings; }
        }

        /// <summary>
        /// Gets or sets the age for this search definition
        /// </summary>
        public int Age {
            get { return _age; }
            set { _age = value; _usedFields |= FormFields.Age; }
        }

        /// <summary>
        /// Gets or sets the gender for this search definition
        /// </summary>
        public String Gender {
            get { return _gender;  }
            set { _gender = value; _usedFields |= FormFields.Gender; }
        }

        /// <summary>
        /// Gets or sets the Phrase/Keyword used in the search
        /// </summary>
        public String Phrase {
            get { return _phrase; }
            set { _phrase = value; _usedFields |= FormFields.Phrase; } 
        }

        /// <summary>
        /// Gets or sets the location type value
        /// </summary>
        public LocationType Location {
            get { return _locationType; }
            set { _locationType = value; _usedFields |= FormFields.Location; } 
        }

        /// <summary>
        /// Gets or sets the parameters for the selected location type
        /// </summary>
        public LocationSearchParams LocationParams
        {
            get { return _locationParams; }
            set { _locationParams = value; }
        }

        /// <summary>
        /// Gets or sets an array of the trial types in the search
        /// </summary>
        public LabelledSearchParam[] TrialTypes {
            get { return _trialTypes; }
            set { _trialTypes = value; _usedFields |= FormFields.TrialTypes; } 
        }

        /// <summary>
        /// Gets or sets an array of the drugs for this search definition
        /// </summary>
        public TerminologyFieldSearchParam[] Drugs {
            get { return _drugs; }
            set { _drugs = value; _usedFields |= FormFields.Drugs; } 
        }

        /// <summary>
        /// Gets or sets an array of the other treatments for this search definition
        /// </summary>
        public TerminologyFieldSearchParam[] OtherTreatments {
            get { return _otherTreatments; }
            set { _otherTreatments = value; _usedFields |= FormFields.OtherTreatments; } 
        }

        /// <summary>
        /// Gets or sets an array of the trial phases in the search
        /// </summary>
        public LabelledSearchParam[] TrialPhases {
            get { return _trialPhases; }
            set { _trialPhases = value; _usedFields |= FormFields.TrialPhases; }
        }

        /// <summary>
        /// Gets or sets an array of the trial IDs in the search
        /// </summary>
        public string[] TrialIDs {
            get { return _trialIDs; }
            set { _trialIDs = value; _usedFields |= FormFields.TrialIDs; }
        }

        /// <summary>
        /// Gets or sets the Investigator used in the search
        /// </summary>
        public String Investigator {
            get { return _investigator; }
            set { _investigator = value; _usedFields |= FormFields.Investigator; }
        }

        /// <summary>
        /// Gets or sets the lead org used in the search
        /// </summary>
        public String LeadOrg {
            get { return _leadOrg; }
            set { _leadOrg = value; _usedFields |= FormFields.LeadOrg; }
        }
        /*
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
        }*/

        /// <summary>
        /// Gets or sets the results link flag for the search
        /// </summary>
        public ResultsLinkType ResultsLinkFlag
        {
            get { return _resultsLinkFlag; }
            set { _resultsLinkFlag = value; }
        }

        /// <summary>
        /// Gets or sets an array of CTS Search Param Errors to identify when a parse error occurred.
        /// </summary>
        public List<CTSSearchParamError> ParseErrors { get; set; }

    }
}
