using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;

using CancerGov.Common.ErrorHandling;
using CancerGov.CDR.DataManager;
using CancerGov.UI.CDR;
using NCI.Util;

namespace CancerGov.CDR.ClinicalTrials.Search
{
    internal class CTSearchQuery
    {
        #region Fields

        string _cdrDbConnectionString;

        #endregion

        #region Constructors

        public CTSearchQuery()
        {
            _cdrDbConnectionString = ConfigurationManager.AppSettings["CDRDbConnectionString"];
        }

        #endregion

        #region Methods


        /// <summary>
        /// Saves the provided search criteria in the database and executes the search.
        /// </summary>
        /// <param name="idString"></param>
        /// <param name="invocationType"></param>
        /// <param name="drugIDList">comma-separated list of CDR IDs</param>
        /// <param name="drugNameList">comma-separated list of Drug Names</param>
        /// <param name="allDrugsMustMatch">true -- all selected drugs must be used in the trial
        /// false -- any of the selected drugs must be used in the trial</param>
        /// <param name="institutionIDList">comma-separated list of institution IDs</param>
        /// <param name="institutionNameList">comma-separated list of institution names</param>
        /// <param name="investigatorIDList">comma-separated list of investigator IDs</param>
        /// <param name="investigatorNameList">comma-separated list of investigator names</param>
        /// <param name="leadOrganizationIDList">comma-separated list of lead organization IDs</param>
        /// <param name="leadOrganizationNameList">comma-separated list of lead organization names</param>
        /// <param name="cancerType">cancer type ID</param>
        /// <param name="cancerTypeName">Name of the cancer type identified by cancerType</param>
        /// <param name="cancerSubtypeList">comma-separated list of cancer subtypes.</param>
        /// <param name="trialType">comma-separated list of trial type IDs</param>
        /// <param name="trialStatus">The type of trials to search for.  Valid values
        /// are TrialStatusType.ClosedOnly and TrialStatusType.OpenOnly.</param>
        /// <param name="onlyClinicalCenterTrials">true -- only find trials taking place
        /// at the NIH clinical center.</param>
        /// <param name="newTrialsOnly">true -- only find trials which were added in the past 30 days.</param>
        /// <param name="phaseList">List of trial phases</param>
        /// <param name="protocolIDList">comma-separated list of alternate protocol IDs</param>
        /// <param name="zipCode">zip code for location search</param>
        /// <param name="zipProximity">distance from zip code for location search</param>
        /// <param name="city">city for location search</param>
        /// <param name="stateList">list of comma-separated state abbreviations.</param>
        /// <param name="country">country name for location search</param>
        /// <param name="sponsorList">comma-separated list of trial sponsors</param>
        /// <param name="specialCategoryList">comma-separated list of special categories</param>
        /// <param name="treatmentTypeIDList">comma-separated list of treatment type IDs.</param>
        /// <param name="treatmentTypeNameList">comma-separated list of treatment type Names.</param>
        /// <param name="keywordText">text to use for keyword search</param>
        /// <param name="criteriaXML"></param>
        /// <returns>Unique identifier for referencing this search.</returns>
        public int SaveAndExecute(string idString, SearchInvocationType invocationType,
            string drugIDList, string drugNameList, bool allDrugsMustMatch,
            string institutionIDList, string institutionNameList,
            string investigatorIDList, string investigatorNameList,
            string leadOrganizationIDList, string leadOrganizationNameList,
            string cancerType, string cancerTypeName, string cancerSubtypeList, string trialType,
            TrialStatusType trialStatus, bool onlyClinicalCenterTrials, bool newTrialsOnly,
            string phaseList, string protocolIDList, string zipCode, int zipProximity, string city, string stateList,
            string country, string sponsorList, string specialCategoryList, string treatmentTypeIDList,
            string treatmentTypeNameList, string keywordText, string criteriaXML)
        {
            int searchID = 0;

            #region Conversion from m.tier to proc inputs

            // Convert trial status to Y for Open/Active, N for Closed/Inactive
            string trialStatusFlag = "Y";
            if (trialStatus == TrialStatusType.ClosedOnly)
                trialStatusFlag = "N";

            // Convert drugname match to OR/AND formula
            string drugSearchFormula = "OR";
            if (allDrugsMustMatch)
                drugSearchFormula = "AND";

            // Convert new trial requirement
            string newTrialsOnlyFlag = null;
            if (newTrialsOnly)
                newTrialsOnlyFlag = "Y";

            // Convert Clinical Center Only location.
            string clinicalCenterOnlyFlag = null;
            if (onlyClinicalCenterTrials)
                clinicalCenterOnlyFlag = "Y";

            // Convert invocation type into whether this is from a link
            int linkFlag = 0;
            if (invocationType == SearchInvocationType.FromSearchLink)
                linkFlag = 1;

            #endregion

            using (SqlConnection conn = new SqlConnection(_cdrDbConnectionString))
            {
                conn.Open();
                SqlCommand scProtocolSearch = new SqlCommand("usp_ProtocolSearchExtended_IDadv1FullText", conn);
                scProtocolSearch.CommandType = CommandType.StoredProcedure;
                scProtocolSearch.CommandTimeout = Strings.ToInt(ConfigurationManager.AppSettings["CTSearchTimeout"], 300);

                // This is a reference, not a new object!
                SqlParameterCollection paramAlias = scProtocolSearch.Parameters;
                AddParameter(paramAlias, "@idstring", idString);
                AddParameter(paramAlias, "@idstringhash", idString.GetHashCode());
                AddParameter(paramAlias, "@CancerType", cancerType);
                AddParameter(paramAlias, "@CancerTypeStage", cancerSubtypeList);
                AddParameter(paramAlias, "@TrialType", trialType);
                if (!string.IsNullOrEmpty(zipCode))
                {
                    AddParameter(paramAlias, "@ZIP", zipCode);
                    AddParameter(paramAlias, "@ZIPProximity", zipProximity);
                }
                else
                {
                    AddParameter(paramAlias, "@ZIP");
                    AddParameter(paramAlias, "@ZIPProximity");
                }
                AddParameter(paramAlias, "@IsNIHClinicalCenterTrial", clinicalCenterOnlyFlag);
                AddParameter(paramAlias, "@ParameterOne", "advancedsearch");    // Always use advanced logic.
                AddParameter(paramAlias, "@ParameterTwo", criteriaXML);
                AddParameter(paramAlias, "@ParameterThree", cancerTypeName);
                AddParameter(paramAlias, "@IsLink", linkFlag);

                // Treatment Types
                AddParameter(paramAlias, "@TreatmentType", treatmentTypeIDList);
                AddParameter(paramAlias, "@treatmentTypeName", treatmentTypeNameList);

                AddParameter(paramAlias, "@TrialStatus", trialStatusFlag);
                AddParameter(paramAlias, "@IsNew", newTrialsOnlyFlag);

                // Hospital/Institution locations
                AddParameter(paramAlias, "@Institutionid", institutionIDList);
                AddParameter(paramAlias, "@Institution", institutionNameList);

                // Investigators
                AddParameter(paramAlias, "@Investigatorid", investigatorIDList);
                AddParameter(paramAlias, "@Investigator", investigatorNameList);

                // Lead Org.
                AddParameter(paramAlias, "@LeadOrganizationid", leadOrganizationIDList);
                AddParameter(paramAlias, "@LeadOrganization", leadOrganizationNameList);

                AddParameter(paramAlias, "@TrialSponsor", sponsorList);
                AddParameter(paramAlias, "@Phase", phaseList);
                AddParameter(paramAlias, "@AlternateProtocolID", protocolIDList);

                // City, State, Country location
                AddParameter(paramAlias, "@State", stateList);
                AddParameter(paramAlias, "@City", city);
                AddParameter(paramAlias, "@Country", country);

                // Drugs
                if (!string.IsNullOrEmpty(drugIDList))
                {
                    AddParameter(paramAlias, "@Drug", drugNameList);
                    AddParameter(paramAlias, "@DrugId", drugIDList);
                    AddParameter(paramAlias, "@DrugSearchFormula", drugSearchFormula);
                }
                else
                {
                    AddParameter(paramAlias, "@Drug");
                    AddParameter(paramAlias, "@DrugId");
                    AddParameter(paramAlias, "@DrugSearchFormula");
                }

                AddParameter(paramAlias, "@SpecialCategory", specialCategoryList);
                AddParameter(paramAlias, "@keyword", keywordText);

                // Execute the search.
                object objTmpSearch = (int)scProtocolSearch.ExecuteScalar();

                // Did we get a result?  If so, go ahead and unbox it.
                if (objTmpSearch != null)
                    searchID = (int)objTmpSearch;
                else
                    // something is wrong.  Crash!
                    throw new NullProtocolSearchIDException();
            }

            return searchID;
        }

        /// <summary>
        /// Convenience method to conditionally add parameters to a SqlParameterCollection.  If paramValue is non-null, it is
        /// added to the collection.  Otherwise, no action is taken.
        /// </summary>
        /// <param name="paramList">A SqlParameterCollection.</param>
        /// <param name="paramName">String containing the name of the parameter (including the leading '@').</param>
        /// <param name="paramValue">String containing the parameter value.</param>
        private void AddParameter(SqlParameterCollection paramList, string paramName, string paramValue)
        {
            // Don't add NULL parameters.
            if (!string.IsNullOrEmpty(paramValue))
            {
                paramList.AddWithValue(paramName, paramValue);
            }
        }

        /// <summary>
        /// Convenience method to add integer parameters to a SqlParameterCollection.
        /// </summary>
        /// <param name="paramList">A SqlParameterCollection.</param>
        /// <param name="paramName">String containing the name of the parameter (including the leading '@').</param>
        /// <param name="paramValue">integer parameter value.</param>
        private void AddParameter(SqlParameterCollection paramList, string paramName, int paramValue)
        {
            paramList.AddWithValue(paramName, paramValue);
        }

        /// <summary>
        /// Convenience method to add null parameters to a SqlParameterCollection.
        /// </summary>
        /// <param name="paramList">A SqlParameterCollection.</param>
        /// <param name="paramName">String containing the name of the parameter (including the leading '@').</param>
        private void AddParameter(SqlParameterCollection paramList, string paramName)
        {
            // Intentionally blank.  Don't add NULL parameters.
            //paramList.AddWithValue(paramName, DBNull.Value);
        }

        public DataTable LoadSavedCriteria(int protocolSearchID)
        {
            DataTable results = new DataTable();

            using (SqlDataAdapter da = new SqlDataAdapter("usp_GetProtocolSearchParamsID", _cdrDbConnectionString))
            {
                try
                {
                    da.SelectCommand.CommandType = CommandType.StoredProcedure;
                    da.SelectCommand.Parameters.AddWithValue("@ProtocolSearchID", protocolSearchID);
                    da.Fill(results);
                }
                catch (SqlException ex)
                {
                    CancerGovError.LogError("", "CTSearchQuery.LoadSavedCriteria", ErrorType.DbUnavailable, ex);
                }
            }

            return results;
        }

        /// <summary>
        /// Retrieves the string containing the XML structure representing a set of search criteria.
        /// </summary>
        /// <param name="protocolSearchID">The ID of the set of search criteria to retrieve.</param>
        /// <returns>string containing an XML structure.</returns>
        public string LoadDisplayCriteria(int protocolSearchID)
        {
            string displayCriteriaXml = null;

            using (SqlConnection conn = new SqlConnection(_cdrDbConnectionString))
            {
                conn.Open();
                SqlCommand scProtocolSearch = new SqlCommand("usp_GetDisplayCriteria", conn);
                scProtocolSearch.CommandType = CommandType.StoredProcedure;
                scProtocolSearch.Parameters.AddWithValue("@ProtocolSearchID", protocolSearchID);

                object objTmpSearch = scProtocolSearch.ExecuteScalar();

                // Did we get a result?  If so, go ahead and cast it.
                // For TESTING - LH
                if (objTmpSearch != null && objTmpSearch != System.DBNull.Value)
                    displayCriteriaXml = (string)objTmpSearch;
                //else
                //// something is wrong.  Crash!
                //throw new NullProtocolSearchIDException();



            }

            return displayCriteriaXml;
        }

        public DataSet LoadCachedSearchResults(int protocolSearchID, CTSSortFilters sortFilter, int pageNumber,
            int pageSize, string sectionList, bool includeStudySites, ProtocolVersions version,
            out int totalResultCount)
        {
            DataSet data = new DataSet();

            using (SqlConnection conn = new SqlConnection(_cdrDbConnectionString))
            {
                using (SqlDataAdapter adapter = new SqlDataAdapter("usp_GetProtocolsBySearchID", conn))
                {
                    adapter.SelectCommand.CommandType = CommandType.StoredProcedure;
                    adapter.SelectCommand.CommandTimeout = Convert.ToInt32(ConfigurationSettings.AppSettings["CTSearchTimeout"]);

                    // This is a reference, not a new object!
                    SqlParameterCollection paramAlias = adapter.SelectCommand.Parameters;

                    AddParameter(paramAlias, "@SectionList", sectionList);
                    AddParameter(paramAlias, "@ProtocolSearchID", protocolSearchID);
                    AddParameter(paramAlias, "@DrawStudySites", Convert.ToInt16(includeStudySites));
                    AddParameter(paramAlias, "@Version", (int)version);
                    AddParameter(paramAlias, "@SortFilter", (int)sortFilter);
                    AddParameter(paramAlias, "@Page", pageNumber);
                    AddParameter(paramAlias, "@ResultsPerPage", pageSize);

                    SqlParameter sqlParam = new SqlParameter("@TotalResults", SqlDbType.Int);
                    sqlParam.Direction = ParameterDirection.Output;
                    paramAlias.Add(sqlParam);

                    // Retrieve the protocol data
                    try
                    {
                        adapter.Fill(data);
                    }
                    catch (SqlException seSQL)
                    {
                        throw new ProtocolFetchFailureException(seSQL.Message.ToString());
                    }

                    // For this to be null, something must be broken in the stored procedure.
                    if (data.Tables == null)
                        throw new ProtocolTableEmptyException("No Tables Returned");

                    // retrieve total number of records;
                    totalResultCount = Strings.ToInt(adapter.SelectCommand.Parameters["@TotalResults"].Value.ToString());
                }
            }

            return data;
        }

        public DataSet LoadSelectedProtocols(string selectedProtocols,
            string selectedSections, bool includeStudySites,
            ProtocolVersions audienceType, CTSSortFilters sortOrder, int protocolSearchID)
        {
            DataSet data = new DataSet();

            using (SqlConnection conn = new SqlConnection(_cdrDbConnectionString))
            {
                using (SqlDataAdapter adapter = new SqlDataAdapter("usp_GetProtocolsByListOfProtocolIDs", conn))
                {
                    adapter.SelectCommand.CommandType = CommandType.StoredProcedure;

                    // This is a reference, not a new object!
                    SqlParameterCollection paramAlias = adapter.SelectCommand.Parameters;

                    AddParameter(paramAlias, "@SectionList", selectedSections);
                    AddParameter(paramAlias, "@ProtocolSearchID", protocolSearchID);
                    AddParameter(paramAlias, "@DrawStudySites", Convert.ToInt16(includeStudySites));
                    AddParameter(paramAlias, "@Version", (int)audienceType);
                    AddParameter(paramAlias, "@sortfilter", (int)sortOrder);
                    AddParameter(paramAlias, "@ProtocolList", selectedProtocols);

                    // Retrieve the protocol data
                    try
                    {
                        adapter.Fill(data);
                    }
                    catch (SqlException seSQL)
                    {
                        throw new ProtocolFetchFailureException(seSQL.Message.ToString());
                    }

                    // For this to be null, something must be broken in the stored procedure.
                    if (data.Tables == null)
                        throw new ProtocolTableEmptyException("No Tables Returned");
                }
            }

            return data;
        }

        public DataTable LoadCancerTypeList()
        {

            DataTable dtCancerTypes = new DataTable("stages");
            SqlDataAdapter daCancerTypes = null;

            try
            {
                daCancerTypes = new SqlDataAdapter("usp_GetMenuTypeInformation", ConfigurationSettings.AppSettings["CDRDbConnectionString"]);
                daCancerTypes.SelectCommand.CommandType = CommandType.StoredProcedure;
                daCancerTypes.SelectCommand.Parameters.Add(new SqlParameter("@MenuTypeID", (int)CDRMenuTypes.CancerTypes));
                daCancerTypes.SelectCommand.Parameters.Add(new SqlParameter("@ParentID", DBNull.Value));

                daCancerTypes.Fill(dtCancerTypes);

            }
            catch (SqlException sqlE)
            {
                CancerGovError.LogError("", "CTSearchQuery.LoadCancerTypeList", ErrorType.DbUnavailable, sqlE);
                dtCancerTypes = null;
            }
            finally
            {

                if (daCancerTypes != null)
                {
                    daCancerTypes.Dispose();
                }

            }
            return dtCancerTypes;
        }

        public DataTable LoadCancerStageList(int cancerTypeID)
        {
            //SCR 832 -- BryanP
            DataTable dtCancerStages = new DataTable("stages");
            SqlDataAdapter daCancerStages = null;

            if (cancerTypeID > 0)
            {

                try
                {
                    daCancerStages = new SqlDataAdapter("usp_GetMenuTypeInformation", ConfigurationSettings.AppSettings["CDRDbConnectionString"]);
                    daCancerStages.SelectCommand.CommandType = CommandType.StoredProcedure;
                    daCancerStages.SelectCommand.Parameters.Add(new SqlParameter("@MenuTypeID", (int)CDRMenuTypes.CancerTypes));
                    daCancerStages.SelectCommand.Parameters.Add(new SqlParameter("@ParentID", cancerTypeID));

                    daCancerStages.Fill(dtCancerStages);

                }
                catch (SqlException sqlE)
                {
                    CancerGovError.LogError("", "CTSearchQuery.LoadCancerSubTypeList", ErrorType.DbUnavailable, sqlE);
                    dtCancerStages = null;
                }
                finally
                {

                    if (daCancerStages != null)
                    {
                        daCancerStages.Dispose();
                    }

                }
            }

            return dtCancerStages;
        }

        public DataTable LoadStateAndCountryList()
        {
            DataTable dtStatesAndCountries = new DataTable();
            SqlDataAdapter daStatesAndCountries = null;

            try
            {
                daStatesAndCountries = new SqlDataAdapter("usp_GetProtocolCountryState", ConfigurationSettings.AppSettings["CDRDbConnectionString"]);
                daStatesAndCountries.SelectCommand.CommandType = CommandType.StoredProcedure;

                daStatesAndCountries.Fill(dtStatesAndCountries);
            }
            catch (SqlException sqlE)
            {
                CancerGovError.LogError("", "CTSearchQuery.LoadStateAndCountryList", ErrorType.DbUnavailable, sqlE);
            }
            finally
            {
                if (daStatesAndCountries != null)
                {
                    daStatesAndCountries.Dispose();
                }
            }

            return dtStatesAndCountries;
        }

        public DataTable LoadTrialTypeList()
        {

            DataTable dtTrialTypes = new DataTable("trialtypes");
            SqlDataAdapter daTrialTypes = null;

            try
            {
                daTrialTypes = new SqlDataAdapter("usp_getTrialTypeManualList", ConfigurationSettings.AppSettings["CDRDbConnectionString"]);
                daTrialTypes.SelectCommand.CommandType = CommandType.StoredProcedure;

                daTrialTypes.Fill(dtTrialTypes);
            }
            catch (SqlException sqlE)
            {
                CancerGovError.LogError("", "CTSearchQuery.LoadTrialList", ErrorType.DbUnavailable, sqlE);
                dtTrialTypes = null;
            }
            finally
            {

                if (daTrialTypes != null)
                {
                    daTrialTypes.Dispose();
                }
            }

            return dtTrialTypes;
        }

        public DataTable LoadTrialPhaseList()
        {
            DataTable dtPhases = new DataTable();
            dtPhases.Columns.Add("Trail Phase", Type.GetType("System.String"));
            dtPhases.Columns.Add("Phase ID", Type.GetType("System.String"));

            dtPhases.Rows.Add(new Object[] { "Phase IV", "4" });
            dtPhases.Rows.Add(new Object[] { "Phase III", "3" });
            dtPhases.Rows.Add(new Object[] { "Phase II", "2" });
            dtPhases.Rows.Add(new Object[] { "Phase I", "1" });

            return dtPhases;
        }

        public DataTable LoadSponsorList()
        {
            DataTable dtSponsors = new DataTable();
            SqlDataAdapter daSponsors = null;

            try
            {
                daSponsors = new SqlDataAdapter("usp_GetProtocolSponsors", ConfigurationSettings.AppSettings["CDRDbConnectionString"]);
                daSponsors.SelectCommand.CommandType = CommandType.StoredProcedure;

                daSponsors.Fill(dtSponsors);

            }
            catch (SqlException sqlE)
            {
                CancerGovError.LogError("", "CTSearchQuery.LoadSponsorList", ErrorType.DbUnavailable, sqlE);
                dtSponsors = null;
            }
            finally
            {
                if (daSponsors != null)
                {
                    daSponsors.Dispose();
                }
            }

            return dtSponsors;
        }

        public DataTable LoadSpecialCategoryList()
        {

            DataTable dtSpecialCategories = new DataTable();
            SqlDataAdapter daSpecialCategories = null;

            try
            {
                daSpecialCategories = new SqlDataAdapter("usp_GetProtocolSpecialCategory", ConfigurationSettings.AppSettings["CDRDbConnectionString"]);
                daSpecialCategories.SelectCommand.CommandType = CommandType.StoredProcedure;

                daSpecialCategories.Fill(dtSpecialCategories);

            }
            catch (SqlException sqlE)
            {
                CancerGovError.LogError("", "CTSearchQuery.LoadSpecialCategoryList", ErrorType.DbUnavailable, sqlE);
            }
            finally
            {
                if (daSpecialCategories != null)
                {
                    daSpecialCategories.Dispose();
                }
            }

            return dtSpecialCategories;
        }

        /// <summary>
        /// Saves a permanent copy of page HTML for later retrieval.  The saved HTML does not
        /// change to reflect changes to the underlying data.
        /// </summary>
        /// <param name="pageHtml">An HTML string to be cached.</param>
        /// <returns>A unique integer ID for retrieving the cached HTML.</returns>
        public int CachePageHtml(string pageHtml)
        {
            int cacheID = 0;

            using (SqlConnection conn = new SqlConnection(_cdrDbConnectionString))
            {
                conn.Open();
                using (SqlCommand pageCacheCommand = new SqlCommand("usp_CacheSearchResultView", conn))
                {
                    pageCacheCommand.CommandType = CommandType.StoredProcedure;

                    //Ok, this is one of those oddities.  Basically if you want to insert text you need 
                    //to go the long way to create a param
                    SqlParameter spTextParam = new SqlParameter("@ResultViewHtml", SqlDbType.NText, pageHtml.Length + 1);
                    spTextParam.Value = pageHtml;

                    pageCacheCommand.Parameters.Add(spTextParam);

                    object executeReturn = pageCacheCommand.ExecuteScalar();
                    if (executeReturn != null)
                        cacheID = Strings.ToInt(executeReturn);
                    else // Something went wrong and we couldn't cache the string. (Execute probably threw the exception already.)
                        throw new ProtocolNullPrintCacheIDException();
                }
            }

            return cacheID;
        }

        public DataTable LoadCachedPage(int cacheID)
        {
            DataTable result = new DataTable();

            using (SqlConnection conn = new SqlConnection(_cdrDbConnectionString))
            {
                conn.Open();
                using (SqlDataAdapter adapter = new SqlDataAdapter("usp_GetCachedSearchResultView", conn))
                {
                    adapter.SelectCommand.CommandType = CommandType.StoredProcedure;
                    AddParameter(adapter.SelectCommand.Parameters, "@ResultViewCacheID", cacheID);

                    try
                    {
                        adapter.Fill(result);
                    }
                    catch (SqlException seSQL)
                    {
                        throw new ProtocolPrintCacheFailureException(seSQL.Message);
                    }
                }
            }

            return result;
        }

        public DataTable LoadPreferredNames(int nameIdentifier, ClinicalTrialSearchLinkIdType identifierType, int diagnosisID)
        {
            DataTable results = new DataTable();

            using (SqlConnection conn = new SqlConnection(_cdrDbConnectionString))
            {
                using (SqlDataAdapter adapter = new SqlDataAdapter("usp_getPreferredNameByID", conn))
                {
                    adapter.SelectCommand.CommandType = CommandType.StoredProcedure;

                    // This is a reference, not a new object!
                    SqlParameterCollection paramAlias = adapter.SelectCommand.Parameters;

                    if (nameIdentifier > 0)
                    {
                        AddParameter(paramAlias, "@CDRID", nameIdentifier);
                        AddParameter(paramAlias, "@TYPE", (int)identifierType);
                    }
                    if (diagnosisID > 0)
                    {
                        AddParameter(paramAlias, "@DIAGNOSISID", diagnosisID);
                    }

                    try
                    {
                        adapter.Fill(results);
                    }
                    catch (SqlException sqlE)
                    {
                        CancerGovError.LogError("CTSearchQuery", 1, "SQL::Failed to get the preferred name : " + sqlE.ToString());
                        throw new Exception("SQL::Failed to get the preferred name : " + sqlE.ToString());
                    }
                    catch (Exception e)
                    {
                        CancerGovError.LogError("CTSearchQuery", 1, "Failed in lookupPreferredNames()");
                        throw new Exception("Failed in CTSearchQuery.LoadPreferredNames().", e);
                    }
                }
            }

            return results;
        }

        public DataTable LoadDrugsByID(string[] ids)
        {
            DataTable result = new DataTable();
            using (SqlConnection conn = new SqlConnection(_cdrDbConnectionString))
            {
                conn.Open();
                using (SqlDataAdapter adapter = new SqlDataAdapter("usp_GetProtocolDrugsByID", conn))
                {
                    adapter.SelectCommand.CommandType = CommandType.StoredProcedure;
                    AddParameter(adapter.SelectCommand.Parameters, "@IDList", String.Join(",", ids));
                    adapter.Fill(result);
                    return result;
               }
            }
        }

        public DataTable LoadInstitutionsByID(string[] ids)
        {
            DataTable result = new DataTable();
            using (SqlConnection conn = new SqlConnection(_cdrDbConnectionString))
            {
                conn.Open();
                using (SqlDataAdapter adapter = new SqlDataAdapter("usp_GetProtocolInstitutionsByID", conn))
                {
                    adapter.SelectCommand.CommandType = CommandType.StoredProcedure;
                    AddParameter(adapter.SelectCommand.Parameters, "@IDList", String.Join(",", ids));
                    adapter.Fill(result);
                    return result;
                }
            }
        }

        public DataTable LoadInvestigatorsByID(string[] ids)
        {
            DataTable result = new DataTable();
            using (SqlConnection conn = new SqlConnection(_cdrDbConnectionString))
            {
                conn.Open();
                using (SqlDataAdapter adapter = new SqlDataAdapter("usp_GetProtocolInvestigatorsByID", conn))
                {
                    adapter.SelectCommand.CommandType = CommandType.StoredProcedure;
                    AddParameter(adapter.SelectCommand.Parameters, "@IDList", String.Join(",", ids));
                    adapter.Fill(result);
                    return result;
               }
            }
        }

        public DataTable LoadLeadOrganizationsByID(string[] ids)
        {
            DataTable result = new DataTable();
            using (SqlConnection conn = new SqlConnection(_cdrDbConnectionString))
            {
                conn.Open();
                using (SqlDataAdapter adapter = new SqlDataAdapter("usp_GetProtocolLeadOrganizationsByID", conn))
                {
                    adapter.SelectCommand.CommandType = CommandType.StoredProcedure;
                    AddParameter(adapter.SelectCommand.Parameters, "@IDList", String.Join(",", ids));
                    adapter.Fill(result);
                    return result;
                }
            }
        }

        #endregion
    }
}