using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.Xml.Serialization;

using NCI.Util;

namespace CancerGov.CDR.ClinicalTrials.Search
{
    public class CTSearchDefinition
    {
        #region Fields

        SearchInvocationType _searchInvocationType;

        KeyValuePair<string,int> _cancerType;    // Name, ID

        // Cancer subtype is two separate fields because only the ID list
        // gets persisted to the database.  The name list just rides along
        // in order to get passed into the constructor for the criteria display.
        List<int> _cancerSubtypeIDList = null;
        List<string> _cancerSubtypeNameList = null;

        List<string> _trialTypeList = null;
        
        List<KeyValuePair<string, int>> _interventionList = null;
        List<KeyValuePair<string, int>> _investigatorList = null;
        List<KeyValuePair<string, int>> _leadOrganizationList = null;

        // List of Drug <name, id> pairs.  Note that the drug name instead of the id
        // is used as the key.
        List<KeyValuePair<string, int>> _drugList = null;


        // Location-related fields.

        /// <summary>
        /// Legacy searches may contain criteria for multiple location types.
        /// The LocationSearchType value stored in _locationType specifies which
        /// one should be used.
        /// </summary>
        LocationSearchType _locationSearchType = LocationSearchType.None;

        // Location criteria to use when _locationType is LocationSearchType.Zip. 
        int _locationZipCode = 0;
        int _locationZipProximity = int.MaxValue;

        // Location criteria to use when _locationType is LocationSearchType.NIH.
        bool _locationNihOnly = false;

        // Location criteria to use when _locationType is LocationSearchType.Institution.
        List<KeyValuePair<string, int>> _locationInstitutions = null;

        // Location criteria to use when _locationType is LocationSearchType.LocationCity.
        string _locationCountry;
        string _locationCountryName;
        List<string> _locationStateIDList = null;
        List<string> _locationStateNameList = null;
        string _locationCity;

        bool _requireAllDrugsMatch = false;

        string _keywords;

        List<string> _trialPhase = null;

        bool _restrictToRecent = false;

        List<string> _specificProtocolIDList;

        List<string> _sponsorIDList = null;

        List<string> _specialCategoryList = null;

        #endregion

        #region Contructors

        public CTSearchDefinition()
        {
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets/Sets the (Name, ID) pair for the the selected cancer type.
        /// </summary>
        public KeyValuePair<string, int> CancerType
        {
            get { return _cancerType; }
            set { _cancerType = value; }
        }

        /// <summary>
        /// Gets a reference to the list of cancer subtype IDs selected for the search.
        /// </summary>
        public List<int> CancerSubtypeIDList
        {
            get
            {
                if (_cancerSubtypeIDList == null)
                    _cancerSubtypeIDList = new List<int>();
                return _cancerSubtypeIDList;
            }
        }

        /// <summary>
        /// Gets a reference to the list of cancer subtype names selected for the search.
        /// For backwards compatability only when generating the criteria display.  
        /// This property is *not* stored in the database.
        /// </summary>
        public List<string> CancerSubtypeNameList
        {
            get
            {
                if (_cancerSubtypeNameList == null)
                    _cancerSubtypeNameList = new List<string>();
                return _cancerSubtypeNameList;
            }
        }

        /// <summary>
        /// Gets a reference to the list of trial type (Name, ID) pairs selected for the search.
        /// </summary>
        public List<string> TrialTypeList
        {
            get
            {
                if (_trialTypeList == null)
                    _trialTypeList = new List<string>();
                return _trialTypeList;
            }
        }

        /// <summary>
        /// Gets a reference to the list of drug Name, ID pairs selected for the search.
        /// </summary>
        public List<KeyValuePair<string, int>> DrugList
        {
            get
            {
                if (_drugList == null)
                    _drugList = new List<KeyValuePair<string, int>>();
                return _drugList;
            }
        }

        /// <summary>
        /// Gets a reference to the list of values of the treatments/intervention selected for the search.
        /// </summary>
        public List<KeyValuePair<string, int>> InterventionList
        {
            get
            {
                if (_interventionList == null)
                    _interventionList = new List<KeyValuePair<string, int>>();
                return _interventionList;
            }
        }

        /// <summary>
        /// Gets a reference to the list of investigator Name, ID pairs selected for the search.
        /// </summary>
        public List<KeyValuePair<string, int>> InvestigatorList
        {
            get
            {
                if (_investigatorList == null)
                    _investigatorList = new List<KeyValuePair<string, int>>();
                return _investigatorList;
            }
        }

        /// <summary>
        /// Gets a reference to the list of lead organization Name, ID pairs selected for the search.
        /// </summary>
        public List<KeyValuePair<string, int>> LeadOrganizationList
        {
            get
            {
                if (_leadOrganizationList == null)
                    _leadOrganizationList = new List<KeyValuePair<string, int>>();

                return _leadOrganizationList;
            }
        }

        /// <summary>
        /// Restricts the search based on specific location types.
        /// 
        /// Meaningful values are:
        ///     LocationSearchType.Zip:         Distance from a specific ZIP code.
        ///                                     Only meaningful if LocationZipCode and LocationZipProximity are
        ///                                     also set.
        ///     
        ///     LocationSearchType.Institution:    Specific list of hospitals/institutions.
        ///                                     Only meaningful if LocationInstitutions has a value.
        ///     
        ///     LocationSearchType.City:        Specific City, State, and/or Country.
        ///                                     Only meaningful if LocationCountry, LocationStateIDList and/or
        ///                                     LocationStateNameList have one or more values.
        ///     
        ///     LocationSearchType.NIH:         Only trials taking place at the NIH clinical center.
        ///                                     Only meaningful if LocationNihOnly is set to TRUE.
        ///     
        /// </summary>
        public LocationSearchType LocationSearchType
        {
            get
            {
                switch (_locationSearchType)
                {
                    case LocationSearchType.None:
                        break;
                    case LocationSearchType.Zip:
                        if (_locationZipCode <= 0 || _locationZipProximity <= 0)
                            _locationSearchType = LocationSearchType.None;
                        break;
                    case LocationSearchType.Institution:
                        if (_locationInstitutions == null || _locationInstitutions.Count == 0)
                            _locationSearchType = LocationSearchType.None;
                        break;
                    case LocationSearchType.City:
                        if (string.IsNullOrEmpty(_locationCity) && string.IsNullOrEmpty(_locationCountry) &&
                            (_locationStateIDList == null || _locationStateIDList.Count == 0))
                            _locationSearchType = LocationSearchType.None;
                        break;
                    case LocationSearchType.NIH:
                        if (!_locationNihOnly)
                            _locationSearchType = LocationSearchType.None;
                        break;
                }

                return _locationSearchType;
            }
            set { _locationSearchType = value; }
        }

        /// <summary>
        /// Gets a reference to the list of institution Name, ID pairs where trials should be taking place.
        /// This list is guaranteed to never be null to an external user.
        /// 
        /// Only used when LocationSearchType is set to LocationSearchType.Institution
        /// </summary>
        public List<KeyValuePair<string, int>> LocationInstitutions
        {
            get
            {
                if (_locationInstitutions == null)
                    _locationInstitutions = new List<KeyValuePair<string, int>>();
                return _locationInstitutions;
            }
        }

        /// <summary>
        /// Restricts search results to a specific ZIP code.  A distance from the ZIP code is specified
        /// via LocationZipProximity.
        /// 
        /// Only used when LocationSearchType is set to LocationSearchType.Zip
        /// </summary>
        public int LocationZipCode
        {
            get { return _locationZipCode; }
            set { _locationZipCode = value; }
        }

        /// <summary>
        /// Restricts search to a specific radius around the value specified for LocationZipCode.
        /// This value is meaningless unless LocationZipCode is also set.
        /// 
        /// Only used when LocationSearchType is set to LocationSearchType.Zip
        /// </summary>
        public int LocationZipProximity
        {
            get { return _locationZipProximity; }
            set { _locationZipProximity = value; }
        }

        /// <summary>
        /// If set TRUE, restricts search results to only those trials taking place
        /// at the NIH Clinical Center.
        /// 
        /// Only used when LocationSearchType is set to LocationSearchType.NIH
        /// </summary>
        public bool LocationNihOnly
        {
            get { return _locationNihOnly; }
            set { _locationNihOnly = value; }
        }

        /// <summary>
        /// Name of the country where search results are being sought.
        /// 
        /// Only used when LocationSearchType is set to LocationSearchType.City
        /// </summary>
        public string LocationCountry
        {
            get { return _locationCountry; }
            set
            {
                _locationCountry = value;

                // Country name may also include the state name (for non-US countries).
                // This requires some special handling.

                if (!string.IsNullOrEmpty(_locationCountry))
                {
                    /// Business Rules:
                    ///     If _locationCountry ends with a pipe '|' character, the country name comes before
                    ///         the pipe.
                    ///     If _locationCountry contains a pipe '|' not at the end, the country name comes before
                    ///         the pipe and a state name after.  (This only applies to non-US countries.)

                    string[] countryParts = _locationCountry.Split('|');
                    if (countryParts.Length >= 1)
                    {
                        _locationCountryName = countryParts[0];
                    }
                    if (countryParts.Length == 2)
                    {
                        // Use property to encapuslate list allocation.
                        LocationStateNameList.Add(countryParts[1]);
                    }
                }

            }
        }

        /// <summary>
        /// Extracts country name from the country value.
        /// </summary>
        public string LocationCountryName
        {
            get { return _locationCountryName; }
        }

        /// <summary>
        /// List of state IDs.   This list is guaranteed to
        /// never be null to an external user.
        /// 
        /// Only used when LocationSearchType is set to LocationSearchType.City
        /// </summary>
        public List<string> LocationStateIDList
        {
            get
            {
                if (_locationStateIDList == null)
                    _locationStateIDList = new List<string>();
                return _locationStateIDList;
            }
        }

        /// <summary>
        /// List of State names.  For backwards compatability only when generating the criteria
        /// display.  This property is *not* stored in the database.
        /// 
        /// Only used when LocationSearchType is set to LocationSearchType.City
        /// 
        /// This list is guaranteed to never be null to an external user.
        /// 
        /// This doesn't belong in the search definition and is only included for historical
        /// (and hysterical) purposes.
        /// </summary>
        public List<string> LocationStateNameList
        {
            get
            {
                if (_locationStateNameList == null)
                    _locationStateNameList = new List<string>();
                return _locationStateNameList;
            }
        }

        /// <summary>
        /// The name of the city where search results should be centered.
        /// </summary>
        public string LocationCity
        {
            get { return _locationCity; }
            set { _locationCity = value; }
        }

        /// <summary>
        /// Determines whether all entries in the drug list must match.
        /// If false, searches for trials matching any drug in the list.
        /// </summary>
        public bool RequireAllDrugsMatch
        {
            get { return _requireAllDrugsMatch; }
            set { _requireAllDrugsMatch = value; }
        }

        /// <summary>
        /// The keyword text to search for.
        /// </summary>
        public string Keywords
        {
            get { return _keywords; }
            set { _keywords = value; }
        }

        /// <summary>
        /// Trial Status.  Meaningful values are TrialStatusType.OpenOnly or TrialStatusType.ClosedOnly.
        /// </summary>
        public TrialStatusType TrialStatusRestriction
        {
            // The system only contains open trials.  This forces all attempts at changing the status restriction
            // to result in Open.
            get { return TrialStatusType.OpenOnly; }
            set { /* Discard any attempts at assigning a value */; }
        }

        /// <summary>
        /// List of trial phases.  This list is guaranteed to
        /// never be null to an external user.
        /// </summary>
        public List<string> TrialPhase
        {
            get
            {
                if (_trialPhase == null)
                    _trialPhase = new List<string>();
                return _trialPhase;
            }
        }

        /// <summary>
        /// Controls whether the search is restricted to 
        /// new clinical trials only.
        /// </summary>
        public bool RestrictToRecent
        {
            get { return _restrictToRecent; }
            set { _restrictToRecent = value; }
        }

        /// <summary>
        /// Restricts the search to a specific protocol ID.  This list is guaranteed to
        /// never be null to an external user.
        /// </summary>
        public List<string> SpecificProtocolIDList
        {
            get
            {
                if (_specificProtocolIDList == null)
                    _specificProtocolIDList = new List<string>();
                return _specificProtocolIDList;
            }
        }

        /// <summary>
        /// List of sponsor IDs to search against.  This list is guaranteed to
        /// never be null to an external user. (These "ID" values are also the sponsor names.)
        /// </summary>
        /// <remarks>This will require changes to usp_GetProtocolSponsors as well
        /// as usp_ProtocolSearchExtended_IDadv1FullText (or its successor).</remarks>
        public List<string> SponsorIDList
        {
            get
            {
                if (_sponsorIDList == null)
                    _sponsorIDList = new List<string>();
                return _sponsorIDList;
            }
        }

        /// <summary>
        /// List of special protocol category IDs.  This list is guaranteed to
        /// never be null to an external user.
        /// </summary>
        public List<string> SpecialCategoryList
        {
            get
            {
                if (_specialCategoryList == null)
                    _specialCategoryList = new List<string>();
                return _specialCategoryList;
            }
        }

        /// <summary>
        /// Create a string containing the elements which make a search unique.
        /// </summary>
        /// <remarks>
        /// Building the ID string is relatively expensive.  If the ID needs to be referenced multiple times
        /// in the same code block, it is preferable to cache the value in a local variable.
        /// </remarks>
        public string IDString
        {
            // TODO: Need to finalize IDString Property.
            get
            {
                StringBuilder strIdString = new StringBuilder();

                if (_searchInvocationType == SearchInvocationType.FromSearchLink)
                    strIdString.Append("L");
                else if (_searchInvocationType == SearchInvocationType.FromSearchForm)
                    strIdString.Append("F");

                AddIDStringValue(strIdString, "cancertype", _cancerType.Value); // Cancer Type
                AddIDStringValue(strIdString, "cancerstage", _cancerSubtypeIDList);   //Cancer Stage
                AddIDStringValue(strIdString, "trialtype", _trialTypeList);         //Trial Type

                // Location information
                switch (_locationSearchType)
                {
                    // ZIP code
                    case LocationSearchType.Zip:
                        if (_locationZipCode != 0)
                        {
                            AddIDStringValue(strIdString, "zip", _locationZipCode);
                            AddIDStringValue(strIdString, "zipprox", _locationZipProximity);
                        }
                        break;

                    // Trials at hospitals/instutions.
                    case LocationSearchType.Institution:
                        AddIDStringValue(strIdString, "institutionsIds", _locationInstitutions);
                        break;
                    
                    // City/State/Country
                    case LocationSearchType.City:
                        AddIDStringValue(strIdString, "state", _locationStateIDList);
                        AddIDStringValue(strIdString, "city", _locationCity);
                        AddIDStringValue(strIdString, "country", _locationCountry);
                        break;

                    // Trials at NIH.
                    case LocationSearchType.NIH:
                        if (_locationNihOnly)
                            AddIDStringValue(strIdString, "ncc", "y");
                        break;

                    case LocationSearchType.None:
                    default:
                        break;
                }
                
                AddIDStringValue(strIdString, "intervention", _interventionList);   // Interventions

                // Last 30 days only flag.  Don't include anything to indicate that when all are accepted.
                if(_restrictToRecent)
                    AddIDStringValue(strIdString, "new", "y");

                AddIDStringValue(strIdString, "sponsor", _sponsorIDList);   // Sponsors

                AddIDStringValue(strIdString, "phase", _trialPhase);                // Trial phase list
                AddIDStringValue(strIdString, "altid", _specificProtocolIDList);    // Protocol ID

                // We need this check so we don't include the drug formula unless drugs
                // have been specified.
                if (_drugList != null && _drugList.Count > 0)
                {
                    AddIDStringValue(strIdString, "drugids", _drugList);    // Drug IDs
                    if (_requireAllDrugsMatch)
                        AddIDStringValue(strIdString, "drugformula", "and");
                }

                AddIDStringValue(strIdString, "specialcat", _specialCategoryList);  // Special Categories.


                AddIDStringValue(strIdString, "investigatorids", _investigatorList);
                AddIDStringValue(strIdString, "leadorgsids", _leadOrganizationList);
                AddIDStringValue(strIdString, "keywords", _keywords);


                // This is more stupid legacy code.  Why's it called "param3"?  Because at some point
                // in the murky past, someone decided it would be a good idea to have a group of
                // parameters which could be used for more than one purpose.  Over time, this name
                // will hopefully be replaced with something a little more meaningful (like, oh,
                // I don't know, perhaps "cancerTypeName"???  But for now, it's necessary
                // to first make sure we don't break the existing stored proc before we go and
                // "fix" anything.
                AddIDStringValue(strIdString, "param3", _cancerType.Key);


                // The following snippet of code is deliberately left as a comment for reasons of
                // history, hysterics, and documentation.  The omission of param1 (with any value)
                // is intentional.  The concept of simple versus advanced search no longer exists
                // (There is no simple search, there is no advanced search.  There is only search.
                // Blame either Yoda or the Matrix.) and in the old code, this was only ever added
                // for simple search.
                //    if (this.ctstSearchType == ClinicalTrialSearchTypes.Simple)
                //        strIdString.Append("|param1=simple");

                return strIdString.ToString();
            }
        }

        /// <summary>
        /// Specifies the means by which the search was invoked.
        /// </summary>
        public SearchInvocationType SearchInvocationType
        {
            get { return _searchInvocationType; }
            set { _searchInvocationType = value; }
        }

        #endregion


        #region Public Methods

        /// <summary>
        /// Convenience method to set the Location type to ZIP along with a the zip code
        /// and proximity values.
        /// </summary>
        /// <param name="zipCode">ZIP code.</param>
        /// <param name="zipProximity">Allowed maximum distance from the ZIP code.</param>
        public void SetLocationZipCriteria(int zipCode, int zipProximity)
        {
            _locationSearchType = LocationSearchType.Zip;
            _locationZipCode = zipCode;
            _locationZipProximity = zipProximity;
        }

        /// <summary>
        /// Convenience method to set the Location type to ZIP along with a the zip code
        /// and proximity values.
        /// </summary>
        /// <param name="zipCode">ZIP code.</param>
        /// <param name="zipProximity">Allowed maximum distance from the ZIP code.</param>
        /// <remarks>If either input contains non-numeric characters, LocationZipCode is set
        /// to 0, LocationZipProximity is set to Int.MaxValue (anywhere) and
        /// LocationType to None.</remarks>
        public void SetLocationZipCriteria(string zipCode, string zipProximity)
        {
            int zip = Strings.ToInt(zipCode, -1);
            int proximity = Strings.ToInt(zipProximity, -1);

            // Only set a ZIP search location if zip and proximity are valid.
            if (zip > 0 && proximity > 0)
                SetLocationZipCriteria(zip, proximity);
            else
                _locationSearchType = LocationSearchType.None;
        }

        #endregion


        #region Helper Methods

        /// <summary>
        /// Overloaded method to add values to a stringbuilder, formatted for use in an ID String.
        /// If pairList is non-null, its values are converted to a comma-separated list
        /// and appended in the format: |valuename=list-of-values
        /// </summary>
        /// <typeparam name="KeyType"></typeparam>
        /// <typeparam name="ValueType"></typeparam>
        /// <param name="sb">A stringbuilder object</param>
        /// <param name="valueName">the name of the value</param>
        /// <param name="pairList">Collection of (name,value) pairs.</param>
        /// <remarks>The value appended to the string builder starts with a | (pipe) in order
        /// to mark the boundary between consecutive values.</remarks>
        static private void AddIDStringValue<KeyType, ValueType>(StringBuilder sb, string valueName, List<KeyValuePair<KeyType, ValueType>> pairList)
        {
            if (pairList != null && pairList.Count > 0)
            {
                StringBuilder sbTemp = new StringBuilder();
                foreach (KeyValuePair<KeyType,ValueType> pair in pairList)
                {
                    if (sbTemp.Length > 0)
                        sbTemp.Append(',');
                    sbTemp.Append(pair.Value);
                }
                sb.AppendFormat("|{0}={1}", valueName, sbTemp.ToString());
            }
        }

        /// <summary>
        /// Overloaded method to add string values to a stringbuilder, formatted for use in an ID String.
        /// If the value is non-null, it is appended in the format: |valuename=value
        /// </summary>
        /// <param name="sb">A stringbuilder object</param>
        /// <param name="valueName">the name of the value</param>
        /// <param name="dictionary">dictionary containing a list of (name,value) pairs.</param>
        /// <remarks>The value appended to the string builder starts with a | (pipe) in order
        /// to mark the boundary between consecutive values.
        /// 
        /// Building the ID string is relatively expensive.  If the ID needs to be referenced multiple times
        /// in the same code block, it may be preferable to cache the value in a local variable.
        /// </remarks>
        static private void AddIDStringValue(StringBuilder sb, string valueName, string value)
        {
            if (!string.IsNullOrEmpty(value))
            {
                sb.AppendFormat("|{0}={1}", valueName, value);
            }
        }

        /// <summary>
        /// Overloaded method to add an array of string values to a stringbuilder, formatted for use in an ID String.
        /// If the value is non-null, it is appended in the format: |valuename=value
        /// </summary>
        /// <param name="sb">A stringbuilder object</param>
        /// <param name="valueName">the name of the value</param>
        /// <param name="valueList">collection of values to add.</param>
        /// <remarks>The value appended to the string builder starts with a | (pipe) in order
        /// to mark the boundary between consecutive values.
        /// </remarks>
        static private void AddIDStringValue(StringBuilder sb, string valueName, IList<string> valueList)
        {
            if (valueList != null && valueList.Count > 0)
            {
                StringBuilder sbTemp = new StringBuilder();
                foreach (string value in valueList)
                {
                    if (!string.IsNullOrEmpty(value))
                    {
                        if (sbTemp.Length > 0)
                            sbTemp.Append(',');
                        sbTemp.Append(value);
                    }
                }
                if (sbTemp.Length > 0)
                    sb.AppendFormat("|{0}={1}", valueName, sbTemp.ToString());
            }
        }

        /// <summary>
        /// Overloaded method to add an integer value to a stringbuilder, formatted for use in an ID String.
        /// The value is appended in the format: |valuename=value
        /// </summary>
        /// <param name="sb">A stringbuilder object</param>
        /// <param name="valueName">the name of the value</param>
        /// <param name="value">The value to append.</param>
        /// <remarks>The value appended to the string builder starts with a | (pipe) in order
        /// to mark the boundary between consecutive values.
        /// </remarks>
        static private void AddIDStringValue(StringBuilder sb, string valueName, int value)
        {
            if (value > 0)
                sb.AppendFormat("|{0}={1}", valueName, value);
        }

        /// <summary>
        /// Overloaded method to add an array of integer values to a stringbuilder, formatted for use
        /// in an ID String. If the array is non-null, it is appended in the format: |valuename=value
        /// </summary>
        /// <param name="sb">A stringbuilder object</param>
        /// <param name="valueName">the name of the value</param>
        /// <param name="valueList">A collection of integer ID values.</param>
        /// <remarks>The value appended to the string builder starts with a | (pipe) in order
        /// to mark the boundary between consecutive values.</remarks>
        static private void AddIDStringValue(StringBuilder sb, string valueName, IList<int> valueList)
        {
            if (valueList != null && valueList.Count > 0)
            {
                StringBuilder sbTemp = new StringBuilder();
                foreach (int value in valueList)
                {
                    if (value == 0)
                        continue;

                    if (sbTemp.Length > 0)
                        sbTemp.Append(',');
                    sbTemp.Append(value);
                }
                sb.AppendFormat("|{0}={1}", valueName, sbTemp.ToString());
            }
        }

        #endregion
    }
}
