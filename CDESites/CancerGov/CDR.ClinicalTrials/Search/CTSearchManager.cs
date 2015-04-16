using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Text;
using System.Linq;
using System.Xml.Serialization;

using NCI.Util;

using CancerGov.CDR.DataManager;
using CancerGov.CDR.ClinicalTrials.Helpers;

namespace CancerGov.CDR.ClinicalTrials.Search
{
 
    
    public static class CTSearchManager
    {
        /// <summary>
        /// Saves a CTSearchDefinition object to the database. If the criteria match a previously
        /// executed search, the existing search query is executed and, if necessary, the results
        /// are cached for later retrieval. If the criteria represent a new search, the new search
        /// is executed.
        /// </summary>
        /// <param name="search">Criteria forming the basis of a clinical trial search.</param>
        /// <returns>An identifier corresponding to the unique set of search criteria.  Searches
        /// with identical criteria are matched to the same identifier value.</returns>

        const string CT_LIST_STRING_DELIMITER = "~~";



        public static int SaveAndExecuteSearch(CTSearchDefinition search)
        {
            int searchID = 0;

            // Sort out the location fields.
            string locationHospitalIDs = null;
            string locationHospitalNames = null;
            string locationZip = null;
            int locationProximity = 0;
            string locationCity = null;
            string locationStateList = null;
            string locationCountry = null;
            bool locationNIHOnly = false;

            string criteriaXML;

            switch (search.LocationSearchType)
            {
                case LocationSearchType.Institution:
                    locationHospitalIDs = BuildIDList(search.LocationInstitutions);
                    locationHospitalNames = BuildNameList(search.LocationInstitutions);
                    break;

                case LocationSearchType.City:
                    locationCity = search.LocationCity;
                    ApplyStateCountryBusinessRulesForSave(BuildIDList(search.LocationStateIDList), search.LocationCountry, out locationStateList, out locationCountry);
                    break;

                case LocationSearchType.NIH:
                    locationNIHOnly = search.LocationNihOnly;
                    break;

                case LocationSearchType.None:
                case LocationSearchType.Zip:
                default:
                    if (search.LocationZipCode > 0)
                    {
                        locationZip = string.Format("{0:D5}", search.LocationZipCode);
                        locationProximity = search.LocationZipProximity;
                    }
                    break;
            }

            // Turn cancer type into strings for the data layer.
            string cancerTypeID = null;
            string cancerTypeName = null;
            if (search.CancerType.Value > 0)
            {
                cancerTypeID = search.CancerType.Value.ToString();
                cancerTypeName = search.CancerType.Key;
            }

            // Build the criteria XML.
            DisplayCriteria displayCriteria = new DisplayCriteria(search);
            XmlSerializer serial = new XmlSerializer(typeof(DisplayCriteria));
            TextWriter writer = new StringWriter();
            serial.Serialize(writer, displayCriteria);
            criteriaXML = writer.ToString();

            CTSearchQuery query = new CTSearchQuery();
            searchID = query.SaveAndExecute(search.IDString, search.SearchInvocationType,
                BuildIDList(search.DrugList), BuildNameList(search.DrugList), search.RequireAllDrugsMatch,
                locationHospitalIDs, locationHospitalNames,
                BuildIDList(search.InvestigatorList), BuildNameList(search.InvestigatorList),
                BuildIDList(search.LeadOrganizationList), BuildNameList(search.LeadOrganizationList),
                cancerTypeID, cancerTypeName, BuildIDList(search.CancerSubtypeIDList),
                BuildIDList(search.TrialTypeList), TrialStatusType.OpenOnly,
                locationNIHOnly, search.RestrictToRecent, BuildIDList(search.TrialPhase),
                BuildIDList(search.SpecificProtocolIDList), locationZip, locationProximity,
                locationCity, locationStateList, locationCountry,
                BuildIDList(search.SponsorIDList), BuildIDList(search.SpecialCategoryList),
                BuildIDList(search.InterventionList), BuildNameList(search.InterventionList),
                search.Keywords, criteriaXML);

            return searchID;
        }

        /// <summary>
        /// Retrieves the search criteria corresponding to a unique search ID.
        /// </summary>
        /// <param name="protocolSearchID">Identifier corresponding to a unique set of search criteria.</param>
        /// <returns></returns>
        public static CTSearchDefinition LoadSavedCriteria(int protocolSearchID)
        {
            CTSearchDefinition criteria = new CTSearchDefinition();

            CTSearchQuery query = new CTSearchQuery();
            DataTable records = query.LoadSavedCriteria(protocolSearchID);

            if (records.Rows.Count > 0)
            {

                DataRow searchDef = records.Rows[0];
                int tempInt;
                string tempString;
                char[] commaDelimiter = { ',' };

                // cancer type
                tempInt = Strings.ToInt(searchDef["cancerType"].ToString());
                tempString = Strings.Clean(searchDef["cancertypename"].ToString());
                if (tempInt > 0 && !string.IsNullOrEmpty(tempString))
                    criteria.CancerType = new KeyValuePair<string, int>(tempString, tempInt);

                // Cancer subtype
                FillFromDelimitedList(criteria.CancerSubtypeIDList,
                    Strings.Clean(searchDef["cancertypestage"].ToString()),
                    commaDelimiter);

                // Determine which Location to use.
                LoadLocationCriteria(criteria, searchDef);

                // Trial Type
                FillFromDelimitedList(criteria.TrialTypeList,
                    Strings.Clean(searchDef["trialtype"].ToString()),
                    commaDelimiter);

                // Drug list
                LoadDrugCriteria(criteria, searchDef);

                // Treatment/Intervention List
                LoadTreatmentIntervention(criteria, searchDef);

                // Keywords
                tempString = Strings.Clean(searchDef["keyword"].ToString());
                if (!string.IsNullOrEmpty(tempString))
                {
                    criteria.Keywords = tempString.Trim();
                }

                // Force trial status to Open as all trials are open.
                criteria.TrialStatusRestriction = TrialStatusType.OpenOnly;

                // Load Trial Phase
                FillFromDelimitedList(criteria.TrialPhase,
                    Strings.Clean(searchDef["Phase"].ToString()),
                    commaDelimiter);

                // Trials from past 30 days.
                criteria.RestrictToRecent = Strings.ToBoolean(searchDef["isnew"].ToString());

                // Alternative Protocol ID(s)
                FillFromDelimitedList(criteria.SpecificProtocolIDList,
                    Strings.Clean(searchDef["alternateprotocolid"].ToString()),
                    commaDelimiter);

                // Trial Sponsors
                FillFromDelimitedList(criteria.SponsorIDList,
                    Strings.Clean(searchDef["trialsponsor"].ToString()),
                    commaDelimiter);

                // Trial Investigators
                LoadTrialInvestigators(criteria, searchDef);

                // Lead Organizations
                LoadLeadOrganizations(criteria, searchDef);

                // Special Catetgories
                FillFromDelimitedList(criteria.SpecialCategoryList,
                    Strings.Clean(searchDef["specialCategory"].ToString()),
                    commaDelimiter);
            }
            else
            {
                throw new ProtocolSearchIDNotValidException();
            }

            return criteria;
        }

        /// <summary>
        /// Retrieves a data structure containing display-ready version of the specified search criteria.
        /// </summary>
        /// <param name="protocolSearchID">Identifier corresponding to a unique set of search criteria.</param>
        /// <returns>DisplayCriteria object containing the search criteria</returns>
        public static DisplayCriteria LoadDisplayCriteria(int protocolSearchID)
        {
            DisplayCriteria criteria = null;

            CTSearchQuery query = new CTSearchQuery();
            string criteriaString = query.LoadDisplayCriteria(protocolSearchID);

            if (!string.IsNullOrEmpty(criteriaString))
            {
                XmlSerializer serial = new XmlSerializer(typeof(DisplayCriteria));
                TextReader reader = new StringReader(criteriaString);
                criteria = (DisplayCriteria)serial.Deserialize(reader);
            }

            return criteria;
        }

        /// <summary>
        /// Helper method to load the proper set of Location criteria.
        /// </summary>
        /// <param name="criteria"></param>
        /// <param name="searchDef"></param>
        /// <remarks>Only one set of Location criteria is loaded, however for historical
        /// reasons there is no flag to indicate which one is to be used.  Additionally,
        /// existing saved searches may include multiple sets.
        /// 
        /// Location criteria are loaded according to hierarchy:
        ///     ZIP Proximity
        ///     City/State/Country
        ///     Institution
        ///     NIH Clinical Center Only.
        /// </remarks>
        private static void LoadLocationCriteria(CTSearchDefinition criteria, DataRow searchDef)
        {
            int zip = 0;
            int.TryParse(Strings.Clean(searchDef["zip"].ToString()), out zip);
            int zipProximity = 0;
            int.TryParse(Strings.Clean(searchDef["zipproximity"].ToString()), out zipProximity);

            string city = Strings.Clean(searchDef["city"].ToString());
            string stateIDList = Strings.Clean(searchDef["state"].ToString());
            string country = Strings.Clean(searchDef["country"].ToString());

            string institutionNames = Strings.Clean(searchDef["institution"].ToString());
            string institutionIDs = Strings.Clean(searchDef["institutionid"].ToString());

            string nihOnly = Strings.Clean(searchDef["isnihclinicalcentertrial"].ToString());

            // ZIP
            if (zip > 0)
            {
                criteria.LocationSearchType = LocationSearchType.Zip;
                criteria.LocationZipCode = zip;
                criteria.LocationZipProximity = zipProximity;
            }
            // City, State, Country
            else if (!string.IsNullOrEmpty(city) || !string.IsNullOrEmpty(stateIDList) || !string.IsNullOrEmpty(country))
            {
                criteria.LocationSearchType = LocationSearchType.City;
                criteria.LocationCity = city;

                string tempCountry, tempStateList;
                ApplyStateCountryBusinessRulesForLoad(stateIDList, country, out tempStateList, out tempCountry);

                criteria.LocationCountry = tempCountry;
                if (!string.IsNullOrEmpty(tempStateList))
                {
                    foreach (string stateID in stateIDList.Split(','))
                        criteria.LocationStateIDList.Add(stateID);
                }
            }
            // Institution List
            else if (!string.IsNullOrEmpty(institutionIDs) && !string.IsNullOrEmpty(institutionNames))
            {
                string[] ids = institutionIDs.Split(',');
                string[] names = MultipleDelimiterStringToStringArray(ids, institutionNames, CTListTypes.Institutions );
                int count = Math.Min(ids.Length, names.Length);

                criteria.LocationSearchType = LocationSearchType.Institution;
                for (int i = 0; i < count; i++)
                {
                    int singleID;
                    bool success = int.TryParse(ids[i], out singleID);
                    if (success)
                        criteria.LocationInstitutions.Add(new KeyValuePair<string, int>(names[i], singleID));
                }
            }
            // NIH Clinical Center
            // Historically, the "IsNihClinicalCenterTrial" value been a boolean with no way to distinguish
            // a yes/no search from one which was based on other location criteria.  Rather than have the majority
            // of otherwise indeterminate saved searches come up with the location set to NIH, we'll only
            // set the location type to NIH if the value is "Y".
            else if (!string.IsNullOrEmpty(nihOnly) && nihOnly.ToUpper() == "Y")
            {
                criteria.LocationSearchType = LocationSearchType.NIH;
                if (nihOnly == "Y")
                    criteria.LocationNihOnly = true;
                else
                    criteria.LocationNihOnly = false;
            }
            // Set a default LocationSearchType
            else
            {
                criteria.LocationSearchType = LocationSearchType.Zip;
            }

        }


        /// <summary>
        /// Method to convert a string delimited with mutliple possible delimiters in to a string array
        /// </summary>
        /// <param name="ids">String array of ID that correspond to the names list</param>
        /// <param name="names">Delimited string containing a list of names</param>
        /// <param name="listType">Enum CTListTypes defines what type of data</param>
        /// <remarks>This method determine is the names string is delimited by a semicolon, a comma, 
        /// or delimiter CT_LIST_STRING_DELIMITER and them splits the string into a string array. 
        /// IF the method can't determine the delimiter used, it calls database lookup routine to look 
        /// up the names based on the ids
        /// </remarks>
        private static string[] MultipleDelimiterStringToStringArray(string[] ids, string names, CTListTypes listType)
        { 
            int idCount = ids.Length; 
            int ctListStringDelimiterLocation = names.IndexOf(CT_LIST_STRING_DELIMITER); 
            //Extension method/lambda expression - counts semicolons
            int semicolonCount = names.Count(p => p == ';');
            //Extension method/lambda expression - counts commas 
            int comaCount = names.Count(p => p == ',');

            if (idCount == 1) 
            {
                string[] outgoing = new string[1];
                outgoing[0] = names;
                return outgoing;
            }
            else if(ctListStringDelimiterLocation > 0)
            {
                //split on CT_LIST_STRING_DELIMITER used to return string array
                return names.Split(CT_LIST_STRING_DELIMITER.Split(' '), System.StringSplitOptions.None);
            }
            else if(semicolonCount == idCount - 1) 
            {
                return names.Split(';');
            }
            else if(comaCount == idCount -1)
            {
                return names.Split(',');
            }
            else
            {
                //DB Lookup
                return LookupListItems(ids, listType);
            }
        }

        /// <summary>
        /// Helper method to load Lookup list contains by ID.
        /// </summary>
        /// <param name="ids"></param>
        /// <param name="listType"></param>
        public static string[] LookupListItems(string[] ids, CTListTypes listType)
        {
            CTSearchQuery query = new CTSearchQuery();
            DataTable results;

            switch (listType)
            {
                case CTListTypes.Drugs:
                    results = query.LoadDrugsByID(ids);
                    break;
                case CTListTypes.Institutions:
                    results = query.LoadInstitutionsByID(ids);
                    break;
                case CTListTypes.Investigators:
                    results = query.LoadInvestigatorsByID(ids);
                    break;
                case CTListTypes.LeadOrganizations:
                    results = query.LoadLeadOrganizationsByID(ids);
                    break;
                case CTListTypes.Treatment:
                    //Treatment strings are a recent addition to the database structure so a stored 
                    //procedure does not exist and there should never be delimiter issues with this data 
                    //this could should never be called. (famous last words...)
                    results = null;
                    break;
                default:
                    results = null;
                    break;
            }

            if (results == null || results.Rows.Count == 0)
                return new string[1]; //return empty string array if results == null
            else
            {
                string[] resultArray = new string[results.Rows.Count];
                int i = 0;
                foreach (DataRow drRow in results.Rows)
                {
                    resultArray[i] = Strings.Clean(drRow["Name"].ToString());
                    ++i;
                }

                return resultArray;
            }
        }

        /// <summary>
        /// Helper method to load the drug list.
        /// </summary>
        /// <param name="criteria"></param>
        /// <param name="searchDef"></param>
        private static void LoadDrugCriteria(CTSearchDefinition criteria, DataRow searchDef)
        {
            string names = Strings.Clean(searchDef["drug"].ToString());
            string ids = Strings.Clean(searchDef["drugid"].ToString());

            if (!string.IsNullOrEmpty(names) && !string.IsNullOrEmpty(ids))
            {
                string[] idList = ids.Split(',');
                string[] nameList = MultipleDelimiterStringToStringArray(idList, names, CTListTypes.Drugs);

                for (int i = 0; i < Math.Min(nameList.Length, idList.Length); i++)
                {
                    int idValue;
                    bool succeeded = int.TryParse(idList[i], out idValue);
                    string name = nameList[i].Trim();
                    if (succeeded)
                    {
                        criteria.DrugList.Add(new KeyValuePair<string, int>(name, idValue));
                    }
                }
            }

            string drugFormula = Strings.Clean(searchDef["drugSearchFormula"].ToString());
            if (!string.IsNullOrEmpty(drugFormula))
            {
                if (drugFormula.ToUpper() == "AND")
                    criteria.RequireAllDrugsMatch = true;
                else
                    criteria.RequireAllDrugsMatch = false;
            }
        }

        /// <summary>
        /// Helper method to load the treatment/intervention list.
        /// </summary>
        /// <param name="criteria"></param>
        /// <param name="searchDef"></param>
        private static void LoadTreatmentIntervention(CTSearchDefinition criteria, DataRow searchDef)
        {
            string names = Strings.Clean(searchDef["TreatmentTypeName"].ToString());
            string ids = Strings.Clean(searchDef["TreatmentType"].ToString());

            if (!string.IsNullOrEmpty(names) && !string.IsNullOrEmpty(ids))
            {
                string[] idList = ids.Split(',');
                string[] nameList = MultipleDelimiterStringToStringArray(idList,names, CTListTypes.Treatment);
                
                for (int i = 0; i < Math.Min(nameList.Length, idList.Length); i++)
                {
                    int idValue;
                    bool succeeded = int.TryParse(idList[i], out idValue);
                    string name = nameList[i].Trim();
                    if (succeeded)
                    {
                        criteria.InterventionList.Add(new KeyValuePair<string, int>(name, idValue));
                    }
                }
            }
        }

        /// <summary>
        /// Helper method to load the list of trial investigators.
        /// </summary>
        /// <param name="criteria"></param>
        /// <param name="searchDef"></param>
        private static void LoadTrialInvestigators(CTSearchDefinition criteria, DataRow searchDef)
        {
            string names = Strings.Clean(searchDef["investigator"].ToString());
            string ids = Strings.Clean(searchDef["investigatorid"].ToString());
            
            if (!string.IsNullOrEmpty(names) && !string.IsNullOrEmpty(ids))
            {
                string[] idList = ids.Split(',');
                string[] nameList = MultipleDelimiterStringToStringArray(idList, names, CTListTypes.Investigators);
                
                for (int i = 0; i < Math.Min(nameList.Length, idList.Length); i++)
                {
                    int idValue;
                    bool succeeded = int.TryParse(idList[i], out idValue);
                    string name = nameList[i].Trim();
                    if (succeeded)
                    {
                        criteria.InvestigatorList.Add(new KeyValuePair<string, int>(name, idValue));
                    }
                }
            }
        }

        /// <summary>
        /// Helper method to load the list of lead organizations.
        /// </summary>
        /// <param name="criteria"></param>
        /// <param name="searchDef"></param>
        private static void LoadLeadOrganizations(CTSearchDefinition criteria, DataRow searchDef)
        {
            string names = Strings.Clean(searchDef["leadorganization"].ToString());
            string ids = Strings.Clean(searchDef["leadorganizationid"].ToString());
            
            if (!string.IsNullOrEmpty(names) && !string.IsNullOrEmpty(ids))
            {
                string[] idList = ids.Split(',');
                string[] nameList = MultipleDelimiterStringToStringArray(idList, names, CTListTypes.LeadOrganizations);

                for (int i = 0; i < Math.Min(nameList.Length, idList.Length); i++)
                {
                    int idValue;
                    bool succeeded = int.TryParse(idList[i], out idValue);
                    string name = nameList[i].Trim();
                    if (succeeded)
                    {
                        criteria.LeadOrganizationList.Add(new KeyValuePair<string, int>(name, idValue));
                    }
                }
            }
        }

        /// <summary>
        /// Helper method to load an IList<string> with values from a delimited string.
        /// Leading and trailing whitespace is ignored.
        /// </summary>
        /// <param name="target">An IList made up of strings.</param>
        /// <param name="delimitedList">A list of values separated by delimiters</param>
        /// <param name="delimiters">An array of one or more delimiter values.</param>
        private static void FillFromDelimitedList(IList<string> target, string delimitedList, char[] delimiters)
        {
            if (!string.IsNullOrEmpty(delimitedList))
            {
                foreach (string item in delimitedList.Split(delimiters))
                {
                    target.Add(item.Trim());
                }
            }
        }

        /// <summary>
        /// Helper method to load an IList<int> with values from a delimited string.
        /// </summary>
        /// <param name="target">An IList made up of ints.</param>
        /// <param name="delimitedList">A list of integer values separated by delimiters</param>
        /// <param name="delimiters">An array of one or more delimiter values.</param>
        private static void FillFromDelimitedList(IList<int> target, string delimitedList, char[] delimiters)
        {
            if (!string.IsNullOrEmpty(delimitedList))
            {
                foreach (string item in delimitedList.Split(delimiters))
                {
                    target.Add(Strings.ToInt(item));
                }
            }
        }

        /// <summary>
        /// Load the search results related to a specified protocol search with a level of detail
        /// specified in the content of the options object.
        /// </summary>
        /// <param name="protocolSearchID">Identifier for the saved search.</param>
        /// <param name="options">A CTSearchResultOptions object specifying the amount of detail
        /// to be retrieved.</param>
        /// <param name="recordOffset">Zero based offset from the first record in the set of matches.</param>
        /// <param name="pageSize">The maximum number of records to be retrieved.  If pageSize is 0,
        /// all records starting at recordOffset are returned.</param>
        /// <param name="totalResultCount">Output variable containing the total number of protocols
        /// matching the search criteria.</param>
        /// <returns>A list of protocol objects</returns>
        public static ProtocolCollection LoadCachedSearchResults(int protocolSearchID, CTSearchResultOptions options,
            int pageNumber, int pageSize, out int totalResultCount)
        {
            string sectionList = BuildIDList(Array.ConvertAll<ProtocolSectionTypes, int>(options.ProtocolSections,
                        delegate(ProtocolSectionTypes type) { return (int)type; }));

            CTSearchQuery query = new CTSearchQuery();
            DataSet protocolData = query.LoadCachedSearchResults(protocolSearchID, options.SortOrder, pageNumber, pageSize,
                sectionList, options.IncludeStudySites, options.Audience, out totalResultCount);

            ProtocolCollection results =
                ProtocolLoader.BuildProtocolCollection(protocolData, protocolSearchID, options.Audience, sectionList);
            results.TotalResults = totalResultCount;
            results.ProtocolSearchID = protocolSearchID;

            return results;
        }

        /// <summary>
        /// Retrieves a specific subset of the protocols from a given search.
        /// </summary>
        /// <param name="selectedProtocols">The protocol IDs to retrieve.</param>
        /// <param name="options">Search result display options</param>
        /// <param name="protocolSearchID">The ID of the protocol search the selected protocols are from.</param>
        /// <returns>A collection of protocols</returns>
        public static ProtocolCollection LoadSelectedProtocols(int[] selectedProtocols, CTSearchResultOptions options,
            int protocolSearchID)
        {
            string sectionList = BuildIDList(Array.ConvertAll<ProtocolSectionTypes,int>(options.ProtocolSections,
                        delegate(ProtocolSectionTypes type) { return (int)type; }));

            CTSearchQuery query = new CTSearchQuery();
            DataSet protocolData = query.LoadSelectedProtocols(BuildIDList(selectedProtocols),
                sectionList, options.IncludeStudySites, options.Audience, options.SortOrder, protocolSearchID);

            ProtocolCollection results =
                ProtocolLoader.BuildProtocolCollection(protocolData, protocolSearchID, options.Audience, sectionList);
            results.TotalResults = results.Count;
            results.ProtocolSearchID = protocolSearchID;

            return results;
        }

        /// <summary>
        /// Retrieves a DataTable containing the available cancer types and accompanying CDRID
        /// </summary>
        /// <returns>A List of KeyValuePair objects (CTSearchFieldList<int>) containing cancer types and an integer value
        /// identifying the cancer type</returns>
        public static CTSearchFieldList<int> LoadCancerTypeList()
        {

            CTSearchQuery query = new CTSearchQuery();
            CTSearchFieldList<int> cancerTypeList = new CTSearchFieldList<int>();
            DataTable dtTable;
            string key, value;
            int ivalue;
            
            dtTable = query.LoadCancerTypeList();
            if (dtTable != null)
            {
                foreach (DataRow drRow in dtTable.Rows)
                {
                    key = Strings.Clean(drRow["DisplayName"].ToString());
                    value = Strings.Clean(drRow["CDRID"].ToString());
                    ivalue = Convert.ToInt32(drRow["CDRID"]);

                    cancerTypeList.Add(new KeyValuePair<string, int>(key, ivalue));
                }
            }

            return cancerTypeList;
        }

        /// <summary>
        /// Retrieves a DataTable containing the cancer stages related to the 
        /// specified cancer type and accompanying CDRID, a value identifying each stage.
        /// </summary>
        /// <param name="cancerTypeID">String identifier from LoadCancerTypeList() uniquely identifying
        /// the parent cancer type.</param>
        /// <returns>A List of KeyValuePair objects (CTSearchFieldList<int>) containing cancer stages and an integer value
        /// identifying the cancer stage (CDRID)</returns>
        public static CTSearchFieldList<int> LoadCancerStageList(int cancerTypeID)
        {
            CTSearchQuery query = new CTSearchQuery();
            CTSearchFieldList<int> cancerStageList = new CTSearchFieldList<int>();
            DataTable dtTable;
            string key; 
            int value;

            dtTable = query.LoadCancerStageList(cancerTypeID);
            if (dtTable != null)
            {
                foreach (DataRow drRow in dtTable.Rows)
                {
                    key = Strings.Clean(drRow["DisplayName"].ToString());
                    value = Convert.ToInt32(drRow["CDRID"]);
                    cancerStageList.Add(new KeyValuePair<string, int>(key, value));
                }
            }

            return cancerStageList;
        }

        /// <summary>
        /// Retrieves a DataTable containing a list of country names and identifiers as well as US State names and identifiers.
        /// </summary>
        /// <param name="countryList">Output variable List of KeyValuePair objects (CTSearchFieldList<string>) containing countries where clinical trials
        /// are performed, paired with an identifier for each country.</param>
        /// <param name="stateList">Output variable List of KeyValuePair objects (CTSearchFieldList<string>) containing a list of states in the USA where clinical trials
        /// are performed, paired with an identifier for each state.</param>
        /// <remarks>For countries outside the USA, the political sub-units (province, state, etc) are returned
        /// as entries in the country list instead of the state list.  This inelegant approach is due to
        /// legacy concerns and will presumably be remedied in a later release.</remarks>
        public static void LoadStateAndCountryList(out CTSearchFieldList<string> countryList,
            out CTSearchFieldList<string> stateList)
        {
            CTSearchQuery query = new CTSearchQuery();
            CTSearchFieldList<string> localCountryList = new CTSearchFieldList<string>();
            CTSearchFieldList<string> localStateList = new CTSearchFieldList<string>();
            DataTable dtTable;
            string key, value;

            dtTable = query.LoadStateAndCountryList();
            if (dtTable != null)
            {
                foreach (DataRow drRow in dtTable.Rows)
                {
                    if (drRow["country"].ToString() == "U.S.A.")
                    {
                        key = Strings.Clean(drRow["name"].ToString());
                        value = Strings.Clean(drRow["state"].ToString());
                        localStateList.Add(new KeyValuePair<string, string>(key, value));
                    }
                    else
                    {
                        key = Strings.Clean(drRow["name"].ToString());
                        value = Strings.Clean(drRow["value"].ToString());
                        localCountryList.Add(new KeyValuePair<string, string>(key, value));
                    }
                }
                countryList = localCountryList;
                stateList = localStateList;
            }
            else
            {
                countryList = null;
                stateList = null;
            }
        }

        /// <summary>
        /// Retrieves a DataTable containing clinical trial types and trial type identifiers.
        /// </summary>
        /// <returns>A List of KeyValuePair objects (CTSearchFieldList<int>) containing clinical trial types and an integer value
        /// identifying the clinical trial type</returns>
        public static CTSearchFieldList<int> LoadTrialTypeList()
        {
            CTSearchQuery query = new CTSearchQuery();
            CTSearchFieldList<int> trialTypeList = new CTSearchFieldList<int>();
            DataTable dtTable;
            string key;
            int value;

            dtTable = query.LoadTrialTypeList();
            if (dtTable != null)
            {
                foreach (DataRow drRow in dtTable.Rows)
                {
                    key = Strings.Clean(drRow["DisplayName"].ToString());
                    value = Convert.ToInt32(drRow["displayPosition"]);

                    trialTypeList.Add(new KeyValuePair<string, int>(key, value));
                }
            }

            return trialTypeList;
       }

        /// <summary>
        /// Retrieves a DataTable containing trial phases and
        /// identifiers.
        /// </summary>
        /// <returns>A List of KeyValuePair objects (CTSearchFieldList<int>) containing trial phases and an integer value identifying the trial phase.</returns>
        public static CTSearchFieldList<int> LoadTrialPhaseList()
        {
            CTSearchQuery query = new CTSearchQuery();
            CTSearchFieldList<int> trialPhaseList = new CTSearchFieldList<int>();
            DataTable dtTable;
            string key;
            int value;

            dtTable = query.LoadTrialPhaseList();
            if (dtTable != null)
            {
                foreach (DataRow drRow in dtTable.Rows)
                {
                    key = Strings.Clean(drRow["Trail Phase"].ToString());
                    value = Convert.ToInt32(drRow["Phase ID"]);

                    trialPhaseList.Add(new KeyValuePair<string, int>(key, value));
                }
            }

            return trialPhaseList;

        }

        /// <summary>
        /// Retrieves a DataTable containing the names of organizations
        /// sponsoring trials.
        /// </summary>
        /// <returns>A List of KeyValuePair objects (CTSearchFieldList<string>) containing clinical trial 
        /// sponsor names (clinical trial sponsor value is the same as clinical trial sponsor key) .</returns>
        public static CTSearchFieldList<string> LoadSponsorList()
        {
            CTSearchQuery query = new CTSearchQuery();
            CTSearchFieldList<string> SponsorList = new CTSearchFieldList<string>();
            DataTable dtTable;
            string key, value;

            dtTable = query.LoadSponsorList();
            if (dtTable != null)
            {
                foreach (DataRow drRow in dtTable.Rows)
                {
                    key = Strings.Clean(drRow["SponsorName"].ToString());
                    value = Strings.Clean(drRow["SponsorName"].ToString());

                    SponsorList.Add(new KeyValuePair<string, string>(key, value));
                }
            }

            return SponsorList;
        }

        /// <summary>
        /// Retrieves a DataTable containing special categories and 
        /// unique identifiers.
        /// </summary>
        /// <returns>A List of KeyValuePair objects (CTSearchFieldList<string>) containing special categories
        /// (special category value is the same as special categories key).</returns>
        public static CTSearchFieldList<string> LoadSpecialCategoryList()
        {
            CTSearchQuery query = new CTSearchQuery();
            CTSearchFieldList<string> SpecialCategoryList = new CTSearchFieldList<string>();
            DataTable dtTable;
            string key, value;

            dtTable = query.LoadSpecialCategoryList();
            if (dtTable != null)
            {
                foreach (DataRow drRow in dtTable.Rows)
                {
                    key = Strings.Clean(drRow["ProtocolSpecialCategory"].ToString());
                    value = Strings.Clean(drRow["ProtocolSpecialCategory"].ToString());

                    SpecialCategoryList.Add(new KeyValuePair<string, string>(key, value));
                }
            }

            return SpecialCategoryList;
        }

        /// <summary>
        /// Saves a permanent copy of page HTML for later retrieval.  The saved HTML does not
        /// change to reflect changes to the underlying data.
        /// 
        /// The cached HMTL may be subsequently retrieved by calling LoadCachedPage().
        /// </summary>
        /// <param name="pageHtml">A block of HTML to preserve.</param>
        /// <returns>Cache ID for retriving the HTML.</returns>
        public static int CachePageHtml(string pageHtml)
        {
            CTSearchQuery query = new CTSearchQuery();
            return query.CachePageHtml(pageHtml);
        }

        /// <summary>
        /// Loads a block of HTML which was saved via CachePageHtml().
        /// </summary>
        /// <param name="cacheID">Unique identifier for the cached HTML.</param>
        /// <returns>A block of HTML.</returns>
        public static CTCachedPrintPage LoadCachedPage(int cacheID)
        {
            CTSearchQuery query = new CTSearchQuery();
            DataTable cachedInfo = query.LoadCachedPage(cacheID);

            string pageHtml = (string)cachedInfo.Rows[0]["ResultViewHtml"];
            DateTime cacheDate = Strings.ToDateTime(cachedInfo.Rows[0]["cachedDate"]);

            CTCachedPrintPage page = new CTCachedPrintPage(cacheID, pageHtml, cacheDate);

            return page;
        }

        /// <summary>
        /// Loads the preferred names associated with an entity and/or diagnosis ID.
        /// </summary>
        /// <param name="nameIdentifier">Numeric identifier for a clinical trial search type.  Use 0 if no value is specified.
        /// If nameIdentifier is specified, identifierType must also be specified.</param>
        /// <param name="identifierType">A ClinicalTrialSearchLinkIdType identifying the type of entity nameIdentifier is 
        /// associated with.  This value is ignored if nameIdentifier is 0.</param>
        /// <param name="diagnosisID">A diagnosis ID</param>
        /// <returns>A CTSearchLinkPreferredNames object containing the prerferred names and types corresponding to the
        /// nameIdentifier (for that ClinicalTrialSearchLinkIdType) as well as the cancer and subtype id/name
        /// matching the diagnosisID.</returns>
        public static CTSearchLinkPreferredNames LoadPreferredNames(int nameIdentifier, ClinicalTrialSearchLinkIdType identifierType, int diagnosisID)
        {
            CTSearchLinkPreferredNames names = null;

            DataTable nameData;

            string preferredName = string.Empty;
            int cancerTypeId = 0;
            string cancerTypeName = string.Empty;
            int cancerSubtypeId = 0;
            string cancerSubtypeName = string.Empty;

            // Don't look up values unless an identifier was provided to search for.
            if (nameIdentifier > 0 || diagnosisID > 0)
            {
                CTSearchQuery query = new CTSearchQuery();
                nameData = query.LoadPreferredNames(nameIdentifier, identifierType, diagnosisID);

                // Load preferred names, if available.
                if (nameData != null && nameData.Rows.Count > 0)
                {
                    DataRow data = nameData.Rows[0];

                    preferredName = Strings.Clean(data["preferredName"]);
                    cancerTypeId = Strings.ToInt(data["cancertypeID"]);
                    cancerTypeName = Strings.Clean(data["cancertypeName"]);
                    cancerSubtypeId = Strings.ToInt(data["cancertypeStageID"]);
                    cancerSubtypeName = Strings.Clean(data["cancertypeStageName"]);
                }
            }

            // Fix up for empty names.

            // For missing interventions, return the ID.  Otherwise, report an error.
            if (nameIdentifier > 0 && 
                Enum.IsDefined(typeof(ClinicalTrialSearchLinkIdType), identifierType) &&
                identifierType != ClinicalTrialSearchLinkIdType.None)
            {
                if (String.IsNullOrEmpty(preferredName))
                {
                    if (identifierType == ClinicalTrialSearchLinkIdType.Intervention)
                        preferredName = "cdrid=" + nameIdentifier.ToString("d");
                    else
                        throw new ProtocolSearchParameterException("The specified id is invalid.");  // Something goes here.
                }
            }

            // Return the ID for empty cancer types and subtypes.
            if (string.IsNullOrEmpty(cancerTypeName))
                cancerTypeName = "cdrid=" + cancerTypeId.ToString("d");
            if (string.IsNullOrEmpty(cancerSubtypeName))
                cancerSubtypeName = "cdrid=" + cancerSubtypeId.ToString("d");

            names = new CTSearchLinkPreferredNames(preferredName, nameIdentifier, identifierType,
                cancerTypeId, cancerTypeName, cancerSubtypeId, cancerSubtypeName);

            return names;
        }

        /// <summary>
        /// Look up a list of trial type names.
        /// </summary>
        /// <param name="trialTypeIDList">Integer list of trial type IDs.</param>
        /// <returns>A comma-separated list of trial type names.</returns>
        public static string[] LookupTrialTypeNames(int[] trialTypeIDList)
        {
            string[] results = null;

            if (trialTypeIDList != null)
            {
                // Load the list of trial types
                CTSearchFieldList<int> trialTypeNameList = CTSearchManager.LoadTrialTypeList();

                List<string> tempResults = new List<string>(trialTypeIDList.Length);
                KeyValuePair<string, int> kvp;
                foreach (int id in trialTypeIDList)
                {
                    kvp = trialTypeNameList.Find(delegate(KeyValuePair<string, int> pair) { return pair.Value == id; });

                    // KeyValuePair is a struct, so we can't compare to null.
                    if (!string.IsNullOrEmpty(kvp.Key) && kvp.Value != 0)
                        tempResults.Add(kvp.Key);
                }

                if (tempResults.Count > 0)
                    results = tempResults.ToArray();
            }

            return results;
        }


        #region Helper Methods

        /// <summary>
        /// Helper method to apply business rules for turning states and countries into useful values on save..
        /// </summary>
        /// <param name="stateListIn">comma-separated list of states</param>
        /// <param name="countryIn">country name.</param>
        /// <param name="stateListOut">output value for returning the possibly modified state list.</param>
        /// <param name="countryOut">output value containing the possibly modified country name.</param>
        private static void ApplyStateCountryBusinessRulesForSave(string stateListIn, string countryIn,
            out string stateListOut, out string countryOut)
        {
            // If countryIn is empty, or if something is just plain broken and the country name
            // isn't set up correctly, then we want to output the values unchanged.
            stateListOut = stateListIn;
            countryOut = countryIn;

            if (!string.IsNullOrEmpty(countryIn))
            {
                /// Business Rules:
                ///     If countryIn ends with a pipe '|' character, the country name comes before
                ///         the pipe.
                ///     If countryIn contains a pipe '|' not at the end, the country names comes before
                ///         the pipe and a state name after.  (This only applies to non-US countries.)

                if (countryIn.EndsWith("|")) // Contains only a country.
                {
                    // countryIn only contains a country.
                    // Remove the pipe, preserve the state list.
                    countryOut = countryIn.TrimEnd('|');
                    stateListOut = stateListIn;
                }
                else
                {
                    // countryIn contains both a country, and a state.
                    if (countryIn.IndexOf('|') != -1) // Just in case something is broken.
                    {
                        string[] countryParts = countryIn.Split('|');
                        if (countryParts.Length == 2)
                        {
                            countryOut = countryParts[0];
                            stateListOut = countryParts[1];
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Helper method to apply business rules translating the state/country values saved for the search
        /// into the state/country values output by for UI use by the LoadStateAndCountryList method.
        /// </summary>
        /// <param name="stateListIn">comma-separated list of state IDs loaded from the database</param>
        /// <param name="countryIn">country name loaded from the database.</param>
        /// <param name="stateListOut">output value for returning the possibly modified state list.</param>
        /// <param name="countryOut">output value containing the possibly modified country name.</param>
        private static void ApplyStateCountryBusinessRulesForLoad(string stateListIn, string countryIn,
            out string stateListOut, out string countryOut)
        {
            // If countryIn is empty, then we want to output the values unchanged.
            stateListOut = stateListIn;     // Minimally, this satisfies the need to assign to the out parameters.
            countryOut = countryIn;

            /// Business Rules:
            ///     If countryIn is null/empty, leave the state list alone.
            ///     If countryIn contains a value, then we must append a pipe "|" character.
            ///     If countryIn == "U.S.A." then stateListIn is considered a valid list of states
            ///         and no further changes are needed.
            ///     If countryIn is any other country, then any value contained in stateListIn is
            ///         a state in that country and is appended to the country name.  (The pipe
            ///         acts a delimiter which is separated out in ApplyStateCountryBusinessRulesForSave()
            ///         during the save process.  stateListOut is set to empty.
            if (!string.IsNullOrEmpty(countryIn))
            {
                countryOut = countryIn + "|";   // Needed for any non-null country regardless.
                if (countryOut != "U.S.A.|")
                {
                    countryOut += stateListIn;
                    stateListOut = null;
                }
            }
        }

        /// <summary>
        /// Helper method to create a comma-separated list of the values contained in a collection
        /// of KeyValuePair objects.
        /// </summary>
        /// <typeparam name="KeyType">(implicit) Type used to declare the Key portion of the KeyValuePair.</typeparam>
        /// <typeparam name="ValType">(implicit) Type used to declare the Value portion of the KeyValuePair.</typeparam>
        /// <param name="list">A collection of key/value objects.</param>
        /// <returns>A comma-separated list of values from the list.  If list is null or empty, returns null.</returns>
        private static string BuildIDList<KeyType, ValType>(IList<KeyValuePair<KeyType, ValType>> list)
        {
            StringBuilder sb = new StringBuilder();
            bool first = true;

            if (list != null)
            {
                foreach (KeyValuePair<KeyType, ValType> item in list)
                {
                    if (!first)
                        sb.Append(',');
                    else
                        first = false;

                    sb.AppendFormat("{0}", item.Value);
                }
            }

            if (sb.Length > 0)
                return sb.ToString();
            else
                return null;
        }

        /// <summary>
        /// Helper method to create a comma-separated list of the values contained in a list.
        /// </summary>
        /// <typeparam name="ListType">(implicit) Type used to declare the list</typeparam>
        /// <param name="list">A collection of values</param>
        /// <returns>A comma-separated list of values from the list.  If list is null or empty, returns null.</returns>
        private static string BuildIDList<ListType>(IEnumerable<ListType> list)
        {
            StringBuilder sb = new StringBuilder();
            bool first = true;

            if (list != null)
            {
                foreach (ListType item in list)
                {
                    if (!first)
                        sb.Append(',');
                    else
                        first = false;

                    sb.AppendFormat("{0}", item);
                }
            }

            if (sb.Length > 0)
                return sb.ToString();
            else
                return null;
        }

        /// <summary>
        /// Helper method to create a comma-separated list of the keys contained in a collection
        /// of KeyValuePair objects.
        /// </summary>
        /// <typeparam name="KeyType">(implicit) Type used to declare the Key portion of the KeyValuePair.</typeparam>
        /// <typeparam name="ValType">(implicit) Type used to declare the Value portion of the KeyValuePair.</typeparam>
        /// <param name="list">A collection of key/value objects.</param>
        /// <returns>A comma-separated list of Keys from the list.  If list is null or empty, returns null.</returns>
        private static string BuildNameList<KeyType, ValType>(IList<KeyValuePair<KeyType, ValType>> list)
        {
            StringBuilder sb = new StringBuilder();
            bool first = true;

            if (list != null)
            {
                foreach (KeyValuePair<KeyType, ValType> item in list)
                {
                    if (!first)
                        sb.Append(CT_LIST_STRING_DELIMITER);
                    else
                        first = false;

                    sb.AppendFormat("{0}", item.Key);
                }
            }

            if (sb.Length > 0)
                return sb.ToString();
            else
                return null;
        }

        #endregion
    }
}
