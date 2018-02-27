using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using Common.Logging;
using NCI.Data;

namespace NCI.Services.Dictionary
{
    internal class DictionaryQuery
    {
        static ILog log = Common.Logging.LogManager.GetLogger(typeof(DictionaryQuery));

        const string SP_GET_DICTIONARY_TERM = "usp_GetDictionaryTerm";
        const string SP_GET_DICTIONARY_TERM_FOR_AUDIENCE = "usp_GetDictionaryTermForAudience";
        const string SP_SEARCH_DICTIONARY = "usp_SearchDictionary";
        const string SP_SEARCH_SUGGEST_DICTIONARY = "usp_SearchSuggestDictionary";
        const string SP_EXPAND_DICTIONARY = "usp_SearchExpandDictionary";
        const string SP_DO_DICTIONARY_ENTRIES_EXIST = "dictionaryTerm_exist";

        private string DBConnectionString { get; set; }

        /// <summary>
        /// Initialization. Set up connection strings.
        /// </summary>
        public DictionaryQuery()
        {
            if (ConfigurationManager.ConnectionStrings["CDRDbConnectionString"] == null)
            {
                log.Fatal("Connection string 'CDRDbConnectionString' is missing.");
                throw new ConfigurationErrorsException("Database connection configuration error.");
            }

            string connStr = ConfigurationManager.ConnectionStrings["CDRDbConnectionString"].ConnectionString;
            if (string.IsNullOrEmpty(connStr))
            {
                log.Fatal("Connection string 'CDRDbConnectionString' is missing.");
                throw new ConfigurationErrorsException("Database connection configuration error.");
            }

            DBConnectionString = connStr;

        }

        /// <summary>
        /// Calls the database to retrieve a single dictionary term based on its specific Term ID.
        /// </summary>
        /// <param name="termId">The ID of the Term to be retrieved</param>
        /// <param name="dictionary">The dictionary to retreive the Term from.
        ///     Valid values are
        ///        Term - Dictionary of Cancer Terms
        ///        drug - Drug Dictionary
        ///        genetic - Dictionary of Genetics Terms
        /// </param>
        /// <param name="language">The Term's desired language.
        ///     Supported values are:
        ///         en - English
        ///         es - Spanish
        /// </param>
        /// <param name="audience">Target audieince for the definition.</param>
        /// <param name="version">String identifying which vereion of the API to match.</param>
        /// <returns></returns>
        public DataTable GetTerm(int termId, DictionaryType dictionary, Language language, AudienceType audience, String version)
        {
            log.DebugFormat("Enter GetTerm( {0}, {1}, {2}, {3}, {4} ).", termId, dictionary, language, audience, version);

            DataTable results = null;

            SqlParameter[] parameters = new SqlParameter[] {
                new SqlParameter("@TermID", SqlDbType.Int){Value = termId},
	            new SqlParameter("@Dictionary", SqlDbType.NVarChar){Value = dictionary.ToString()},
	            new SqlParameter("@Language", SqlDbType.NVarChar){Value = language.ToString()},
	            new SqlParameter("@Audience", SqlDbType.NVarChar){Value = audience.ToString()},
	            new SqlParameter("@ApiVers", SqlDbType.NVarChar){Value = version},
            };

            using (SqlConnection conn = SqlHelper.CreateConnection(DBConnectionString))
            {
                results = SqlHelper.ExecuteDatatable(conn, CommandType.StoredProcedure, SP_GET_DICTIONARY_TERM, parameters);
            }

            return results;
        }

        /// <summary>
        /// Calls the database to retrieve a single dictionary term based on its specific Term ID.
        /// Similar, but not identical, to GetTerm().  Instead of retrieving the term for a specific
        /// dictionary, the term is fetched for a preferred audience.  If no records are available for that audience,
        /// then any other avaiable records are returned instead.
        /// </summary>
        /// <param name="termId">The ID of the Term to be retrieved</param>
        /// <param name="language">The Term's desired language.
        ///     Supported values are:
        ///         en - English
        ///         es - Spanish
        /// </param>
        /// <param name="preferredAudience">Preferred target audieince for the definition.</param>
        /// <param name="version">String identifying which vereion of the API to match.</param>
        /// <returns></returns>
        public DataTable GetTermForAudience(int termId, Language language, AudienceType preferredAudience, String version)
        {
            log.DebugFormat("Enter GetTermForAudience( {0}, {1}, {2}, {3} ).", termId, language, preferredAudience, version);

            DataTable results = null;

            SqlParameter[] parameters = new SqlParameter[] {
                new SqlParameter("@TermID", SqlDbType.Int){Value = termId},
	            new SqlParameter("@Language", SqlDbType.NVarChar){Value = language.ToString()},
	            new SqlParameter("@PreferredAudience", SqlDbType.NVarChar){Value = preferredAudience.ToString()},
	            new SqlParameter("@ApiVers", SqlDbType.NVarChar){Value = version},
            };

            using (SqlConnection conn = SqlHelper.CreateConnection(DBConnectionString))
            {
                results = SqlHelper.ExecuteDatatable(conn, CommandType.StoredProcedure, SP_GET_DICTIONARY_TERM_FOR_AUDIENCE, parameters);
            }

            return results;
        }

        /// <summary>
        /// Performs a search for terms with names matching searchText. Results are sorted by the matching term name.
        /// </summary>
        /// <param name="searchText">text to search for.</param>
        /// <param name="searchType">The type of search to perform.
        ///     Valid values are:
        ///         Begins - Search for terms beginning with searchText.
        ///         Contains - Search for terms containing searchText.
        ///         Magic - Search for terms beginning with searchText, followed by those containing searchText.
        /// </param>
        /// <param name="offset">Offset into the list of matches for the first result to return.</param>
        /// <param name="numResults">The maximum number of results to return. Must be at least 10.</param>
        /// <param name="dictionary">The dictionary to retreive the Term from.
        ///     Valid values are
        ///        Term - Dictionary of Cancer Terms
        ///        drug - Drug Dictionary
        ///        genetic - Dictionary of Genetics Terms
        /// </param>
        /// <param name="language">The Term's desired language.
        ///     Supported values are:
        ///         en - English
        ///         es - Spanish
        /// </param>
        /// <param name="audience">The desired target audience - Patient or Health professional</param>
        /// <param name="version">String identifying which vereion of the JSON structure to retrieve.</param>
        /// <returns>DataTable containing a list of matching records.  Results are sorted by the matching term name.</returns>
        public SearchResults Search(String searchText, SearchType searchType, int offset, int numResults, DictionaryType dictionary, Language language, AudienceType audience, String version)
        {
            log.DebugFormat("Enter Search( {0}, {1}, {2}, {3}, {4}, {5}, {6} ).", searchText, offset, numResults, dictionary, language, audience, version);

            DataTable results = null;

            switch (searchType)
            {
                case SearchType.Begins:
                    searchText += "%";
                    break;
                case SearchType.Contains:
                    searchText = "%" + searchText + "%";
                    break;
                default:
                    {
                        String message = String.Format("Unsupport search type '{0}'.", searchType);
                        log.Error(message);
                        throw new ArgumentException(message);
                    }
            }

            SqlParameter matchCountParam = new SqlParameter("@matchCount", SqlDbType.Int) { Direction = ParameterDirection.Output };
            int matchCount;

            SqlParameter[] parameters = new SqlParameter[] {
                new SqlParameter("@searchText", SqlDbType.NVarChar){ Value = searchText},
                new SqlParameter("@offset", SqlDbType.Int){ Value = offset },
                new SqlParameter("@maxResults", SqlDbType.Int){ Value = numResults },

                new SqlParameter("@Dictionary", SqlDbType.NVarChar){Value = dictionary.ToString()},
	            new SqlParameter("@Language", SqlDbType.NVarChar){Value = language.ToString()},
	            new SqlParameter("@Audience", SqlDbType.NVarChar){Value = audience.ToString()},
	            new SqlParameter("@ApiVers", SqlDbType.NVarChar){Value = version},
                matchCountParam                
            };

            using (SqlConnection conn = SqlHelper.CreateConnection(DBConnectionString))
            {
                results = SqlHelper.ExecuteDatatable(conn, CommandType.StoredProcedure, SP_SEARCH_DICTIONARY, parameters);

                // There's some unresolved weirdness with matchCountParam.Value coming back as NULL even though
                // the value is set unconditionally.  This appears to have been due to retrieving the value
                // after the connection had been closed. But, since that's not definite, check that the parameter
                // value is not null (or DBNull) and if so, log an error and attempt to mretrieve a value that
                // will allow execution to continue.
                if (DBNull.Value.Equals(matchCountParam.Value) || matchCountParam.Value == null)
                {
                    log.Warn("Search() encountered null when attempting to retrieve the @matchCount parameter.");
                    matchCount = int.MaxValue;
                }
                else
                    matchCount = (int)matchCountParam.Value;
            }

            return new SearchResults(results, matchCount);
        }

        /// <summary>
        /// Calls the database to search for terms matching searchText. This method is intended for use with autosuggest
        /// and returns a maximum of 10 results
        /// </summary>
        /// <param name="searchText">text to search for.</param>
        /// <param name="searchType">The type of search to perform.
        ///     Valid values are:
        ///         Begins - Search for terms beginning with searchText.
        ///         Contains - Search for terms containing searchText.
        ///         Magic - Search for terms beginning with searchText, followed by those containing searchText.
        /// </param>
        /// <param name="numResults">Maximum number of results to return.</param>
        /// <param name="dictionary">The dictionary to retreive the Term from.
        ///     Valid values are
        ///        Term - Dictionary of Cancer Terms
        ///        drug - Drug Dictionary
        ///        genetic - Dictionary of Genetics Terms
        /// </param>
        /// <param name="language">The Term's desired language.
        ///     Supported values are:
        ///         en - English
        ///         es - Spanish
        /// </param>
        /// <param name="audience">The desired target audience - Patient or Health professional</param>
        /// <param name="version">String identifying which vereion of the JSON structure to retrieve.</param>
        /// <returns>DataTable containing a list of matching records.</returns>
        public SuggestionResults SearchSuggest(String searchText, SearchType searchType, int numResults, DictionaryType dictionary, Language language, AudienceType audience, String version)
        {
            log.DebugFormat("Enter SearchSuggest( {0}, {1}, {2}, {3}, {4}, {5}, {6} ).", searchText, searchType, numResults, dictionary, language, audience, version);

            DataTable results = null;

            SqlParameter matchCountParam = new SqlParameter("@matchCount", SqlDbType.Int) { Direction = ParameterDirection.Output };
            int matchCount;

            SqlParameter[] parameters = new SqlParameter[] {
                new SqlParameter("@searchText", SqlDbType.NVarChar){ Value = searchText},
                new SqlParameter("@searchType", SqlDbType.NVarChar){ Value = searchType.ToString() },
                new SqlParameter("@maxResults", SqlDbType.Int){ Value = numResults },

                new SqlParameter("@Dictionary", SqlDbType.NVarChar){Value = dictionary.ToString()},
	            new SqlParameter("@Language", SqlDbType.NVarChar){Value = language.ToString()},
	            new SqlParameter("@Audience", SqlDbType.NVarChar){Value = audience.ToString()},
	            new SqlParameter("@ApiVers", SqlDbType.NVarChar){Value = version},
                matchCountParam                
            };

            using (SqlConnection conn = SqlHelper.CreateConnection(DBConnectionString))
            {
                results = SqlHelper.ExecuteDatatable(conn, CommandType.StoredProcedure, SP_SEARCH_SUGGEST_DICTIONARY, parameters);

                // There's some unresolved weirdness with matchCountParam.Value coming back as NULL even though
                // the value is set unconditionally.  This appears to have been due to retrieving the value
                // after the connection had been closed. But, since that's not definite, check that the parameter
                // value is not null (or DBNull) and if so, log an error and retrieve a value that will allow
                // execution to continue.
                if (DBNull.Value.Equals(matchCountParam.Value) || matchCountParam.Value == null)
                {
                    log.Warn("SearchSuggest() encountered null when attempting to retrieve the @matchCount parameter.");
                    matchCount = int.MaxValue;
                }
                else
                    matchCount = (int)matchCountParam.Value;
            }

            return new SuggestionResults(results, matchCount);
        }

        /// <summary>
        /// Calls the database to search for terms matching searchText. Results are sorted by the matched term name or alias.
        /// </summary>
        /// <param name="searchText">text to search for.</param>
        /// <param name="includeTypes">A filter for the types of name aliases to include.  Multiple values are separated by the pipe character (|).
        /// If no filter is supplied, the result </param>
        /// <param name="offset">Offset into the list of matches for the first result to return.</param>
        /// <param name="numResults">The maximum number of results to return. Must be at least 10.</param>
        /// <param name="dictionary">The dictionary to retreive the Term from.
        ///     Valid values are
        ///        Term - Dictionary of Cancer Terms
        ///        drug - Drug Dictionary
        ///        genetic - Dictionary of Genetics Terms
        /// </param>
        /// <param name="language">The Term's desired language.
        ///     Supported values are:
        ///         en - English
        ///         es - Spanish
        /// </param>
        /// <param name="version">String identifying which vereion of the JSON structure to retrieve.</param>
        /// <returns>DataTable containing a list of matching records.  Results are sorted by the matching term name.</returns>
        public SearchResults Expand(String searchText, String[] includeTypes, int offset, int numResults, DictionaryType dictionary, Language language, AudienceType audience, String version)
        {
            log.DebugFormat("Enter Expand( {0}, {1}, {2}, {3}, {4}, {5}, {6}, {7} ).", searchText, includeTypes, offset, numResults, dictionary, language, audience, version);

            DataTable results;

            // Set up table parameter for specific types to include.
            DataTable includeFilter = new DataTable("includes");
            includeFilter.Columns.Add("NameType");
            Array.ForEach(includeTypes, typeName => includeFilter.Rows.Add(typeName));

            SqlParameter matchCountParam = new SqlParameter("@matchCount", SqlDbType.Int) { Direction = ParameterDirection.Output };
            int matchCount;

            SqlParameter[] parameters = new SqlParameter[] {
                new SqlParameter("@searchText", SqlDbType.NVarChar){Value = searchText},
                new SqlParameter("@IncludeTypes", SqlDbType.Structured){Value = includeFilter},
                new SqlParameter("@offset", SqlDbType.Int){ Value = offset },
                new SqlParameter("@maxResults", SqlDbType.Int){ Value = numResults },

                new SqlParameter("@Dictionary", SqlDbType.NVarChar){Value = dictionary.ToString()},
	            new SqlParameter("@Language", SqlDbType.NVarChar){Value = language.ToString()},
	            new SqlParameter("@Audience", SqlDbType.NVarChar){Value = audience.ToString()},
	            new SqlParameter("@ApiVers", SqlDbType.NVarChar){Value = version},
                matchCountParam                
            };

            using (SqlConnection conn = SqlHelper.CreateConnection(DBConnectionString))
            {
                results = SqlHelper.ExecuteDatatable(conn, CommandType.StoredProcedure, SP_EXPAND_DICTIONARY, parameters);

                // There's some unresolved weirdness with matchCountParam.Value coming back as NULL even though
                // the value is set unconditionally.  This appears to have been due to retrieving the value
                // after the connection had been closed. But, since that's not definite, check that the parameter
                // value is not null (or DBNull) and if so, log an error and retrieve a value that will allow
                // execution to continue.
                if (DBNull.Value.Equals(matchCountParam.Value) || matchCountParam.Value == null)
                {
                    log.Warn("Expand() encountered null when attempting to retrieve the @matchCount parameter.");
                    matchCount = int.MaxValue;
                }
                else
                    matchCount = (int)matchCountParam.Value;
            }

            return new SearchResults(results, matchCount);
        }

        /// <summary>
        /// Performs a check of whether the given dictionary entries are valid in the database.
        /// </summary>
        /// <param name="entriesList">A list of DictionaryEntryMetadata items to query the database for existence.</param>
        /// <returns>DataTable containing a list of matching dictionary entries metadata.</returns>
        public DataTable DoDictionaryEntriesExist(List<DictionaryEntryMetadata> entriesList)
        {
            // Set up DataTable of DictionaryEntryMetadatas
            DataTable entries = ReturnTermMetadataSqlParam(entriesList);
            DataTable results = null;
            DataTable joinedResults = GetTermMetadataDataTable();

            // Set up the entries DataTable SQL param
            SqlParameter termMetadataTableParam = new SqlParameter("@dictionaryterms", entries);
            termMetadataTableParam.SqlDbType = SqlDbType.Structured;
            termMetadataTableParam.TypeName = "dbo.udt_DictionaryTerm";

            SqlParameter[] parameters = new SqlParameter[] {
                termMetadataTableParam               
            };

            using (SqlConnection conn = SqlHelper.CreateConnection(DBConnectionString))
            {
                // This returns a table with all of the given "entries" table's CDRIDs, and true or false depending on whether the item exists in the database.
                results = SqlHelper.ExecuteDatatable(conn, CommandType.StoredProcedure, SP_DO_DICTIONARY_ENTRIES_EXIST, parameters);
            }
            
            // Remove all entries from the results that are not valid in the database
            var validResults = results.AsEnumerable().Where(r => r.Field<int>("Column1") == 0);

            foreach (var row in validResults.ToList())
            {
                results.Rows.Remove(row);
            }

            // Join the given "results" table and the "entries" table. The joined match (or matches, since a term CDRID will match two CDRIDs - Spanish and English - in the "results table)
            // are put into a list and the first of this list is selected to prevent duplicates.
            IEnumerable<DataRow> joined = from dr1 in results.AsEnumerable()
                                          join dr2 in entries.AsEnumerable()
                                          on dr1.Field<int>("cdrid") equals dr2.Field<int>("CDRID") into lstGroup
                                          select lstGroup.First();

            joinedResults = joined.CopyToDataTable<DataRow>();

            return joinedResults;
        }

        // Set up datatable structure for Term Metadata table
        private DataTable GetTermMetadataDataTable()
        {
            // This datatable must be structured like the datatable in the stored proc, 
            // in order to be passed in correctly as a parameter.
            DataTable dt = new DataTable();

            // First column, "CDRID", is an int
            DataColumn dc = new DataColumn();
            dc.DataType = System.Type.GetType("System.Int32");
            dc.ColumnName = "CDRID";
            dt.Columns.Add(dc);

            // Second column, "Dictionary", is a varchar of 20 characters
            DataColumn dc1 = new DataColumn();
            dc1.DataType = System.Type.GetType("System.String");
            dc1.ColumnName = "Dictionary";
            dc1.MaxLength = 20;
            dt.Columns.Add(dc1);

            // Third column, "Language", is a varchar of 40 characters
            DataColumn dc2 = new DataColumn();
            dc2.DataType = System.Type.GetType("System.String");
            dc2.ColumnName = "Language";
            dc2.MaxLength = 40;
            dt.Columns.Add(dc2);

            // Fourth column, "Audience", is a varchar of 50 characters
            DataColumn dc3 = new DataColumn();
            dc3.DataType = System.Type.GetType("System.String");
            dc3.ColumnName = "Audience";
            dc3.MaxLength = 50;
            dt.Columns.Add(dc3);

            return dt;
        }

        // Create TermMetadata DataTable from entries list for SQL query
        private DataTable ReturnTermMetadataSqlParam(List<DictionaryEntryMetadata> entries)
        {
            // This datatable must be structured like the datatable in the stored proc, 
            // in order to be passed in correctly as a parameter.
            DataTable dt = GetTermMetadataDataTable();

            // Loop through each of the different DictionaryEntryMetadata entries
            foreach (DictionaryEntryMetadata entry in entries)
            {
                // Create a new row in the datatable for each dictionary entry, and assign the
                // values to the columns accordingly.
                DataRow row = dt.NewRow();
                row["CDRID"] = entry.CDRID;
                row["Dictionary"] = entry.Dictionary;
                row["Language"] = entry.Language;
                row["Audience"] = entry.Audience;
                dt.Rows.Add(row);
            }

            return dt;
        }
    }
}
