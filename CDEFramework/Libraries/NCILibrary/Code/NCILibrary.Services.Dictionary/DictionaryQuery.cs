using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

using NCI.Data;
using NCI.Logging;

namespace NCI.Services.Dictionary
{
    internal class DictionaryQuery
    {
        static Log log = new Log(typeof(DictionaryQuery));

        const string SP_GET_DICTIONARY_TERM = "usp_GetDictionaryTerm";
        const string SP_SEARCH_DICTIONARY = "usp_SearchDictionary";
        const string SP_SEARCH_SUGGEST_DICTIONARY = "usp_SearchSuggestDictionary";
        const string SP_EXPAND_DICTIONARY = "usp_SearchExpandDictionary";

        private string DBConnectionString { get; set; }

        public DictionaryQuery()
        {
            if (ConfigurationManager.ConnectionStrings["CDRDbConnectionString"] == null)
            {
                log.fatal("Connection string 'CDRDbConnectionString' is missing.");
                throw new ConfigurationErrorsException("Database connection configuration error.");
            }

            string connStr = ConfigurationManager.ConnectionStrings["CDRDbConnectionString"].ConnectionString;
            if (string.IsNullOrEmpty(connStr))
            {
                log.fatal("Connection string 'CDRDbConnectionString' is missing.");
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
            log.debug(string.Format("Enter GetTerm( {0}, {1}, {2}, {3}, {4} ).", termId, dictionary, language, audience, version));

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

        public SearchResults Search(String searchText, SearchType searchType, int offset, int maxResults, DictionaryType dictionary, Language language, AudienceType audience, String version)
        {
            log.debug(string.Format("Enter Search( {0}, {1}, {2}, {3}, {4}, {5}, {6} ).", searchText, offset, maxResults, dictionary, language, audience, version));

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
                        log.error(message);
                        throw new ArgumentException(message);
                    }
            }

            SqlParameter matchCountParam = new SqlParameter("@matchCount", SqlDbType.Int) { Direction = ParameterDirection.Output };
            int matchCount;

            SqlParameter[] parameters = new SqlParameter[] {
                new SqlParameter("@searchText", SqlDbType.NVarChar){ Value = searchText},
                new SqlParameter("@offset", SqlDbType.Int){ Value = offset },
                new SqlParameter("@maxResults", SqlDbType.Int){ Value = maxResults },

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
                // value is not null (or DBNull) and if so, log an error and retrieve a value that will allow
                // execution to continue.
                if (DBNull.Value.Equals(matchCountParam.Value) || matchCountParam.Value == null)
                {
                    log.warning("Search() encountered null when attempting to retrieve the @matchCount parameter.");
                    matchCount = int.MaxValue;
                }
                else
                    matchCount = (int)matchCountParam.Value;
            }

            return new SearchResults(results, matchCount);
        }

        public SuggestionResults SearchSuggest(String searchText, SearchType searchType, int maxResults, DictionaryType dictionary, Language language, AudienceType audience, String version)
        {
            log.debug(string.Format("Enter SearchSuggest( {0}, {1}, {2}, {3}, {4}, {5}, {6} ).", searchText, searchType, maxResults, dictionary, language, audience, version));

            DataTable results = null;

            SqlParameter matchCountParam = new SqlParameter("@matchCount", SqlDbType.Int) { Direction = ParameterDirection.Output };
            int matchCount;

            SqlParameter[] parameters = new SqlParameter[] {
                new SqlParameter("@searchText", SqlDbType.NVarChar){ Value = searchText},
                new SqlParameter("@searchType", SqlDbType.NVarChar){ Value = searchType.ToString() },
                new SqlParameter("@maxResults", SqlDbType.Int){ Value = maxResults },

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
                    log.warning("Search() encountered null when attempting to retrieve the @matchCount parameter.");
                    matchCount = int.MaxValue;
                }
                else
                    matchCount = (int)matchCountParam.Value;
            }

            return new SuggestionResults(results, matchCount);
        }

        public SearchResults Expand(String searchText, String[] includeTypes, int offset, int maxResults, DictionaryType dictionary, Language language, AudienceType audience, String version)
        {
            log.debug(string.Format("Enter Expand( {0}, {1}, {2}, {3}, {4}, {5}, {6}, {7} ).", searchText, includeTypes, offset, maxResults, dictionary, language, audience, version));

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
                new SqlParameter("@maxResults", SqlDbType.Int){ Value = maxResults },

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
                    log.warning("Search() encountered null when attempting to retrieve the @matchCount parameter.");
                    matchCount = int.MaxValue;
                }
                else
                    matchCount = (int)matchCountParam.Value;
            }

            return new SearchResults(results, matchCount);
        }
    }
}
