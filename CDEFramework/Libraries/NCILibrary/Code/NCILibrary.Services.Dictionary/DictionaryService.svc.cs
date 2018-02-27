using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.ServiceModel.Web;
using Common.Logging;
using NCI.Services.Dictionary.BusinessObjects;

namespace NCI.Services.Dictionary
{

    /// <summary>
    /// Implements the public methods for the dictionary service and defines the rules for
    /// mapping the parts of the dictionary URLs into method parameters.
    /// </summary>
    /// <remarks>
    /// Changing the class name "DictionaryService" here requires that you also update the reference
    /// to "DictionaryService" in the Web.config and the associated .svc file.
    /// 
    /// NOTE: A possibly important implementation detail: These methods take enums as parameters.
    /// 
    /// The plus side is, unlike strings, WCF will guarantee that the parameter values are valid,
    /// thus simplifying the validation code.
    /// 
    /// There are some recommendations against using enums in web services,
    /// http://www.25hoursaday.com/weblog/2005/08/31/WhyYouShouldAvoidUsingEnumeratedTypesInXMLWebServices.aspx
    /// but these revolve around using them as outputs (A calling routine might not handle a newly added
    /// enum value correctly).
    /// 
    /// These parameters are input-only.     * 
    /// 
    /// </remarks>
    [ServiceContract]
    public class DictionaryService
    {
        static ILog log = Common.Logging.LogManager.GetLogger(typeof(DictionaryService));

        const String API_VERSION = "v1";

        #region private class InputValidator

        /// <summary>
        /// Validates input for the DictionaryService public interface.
        /// </summary>
        private static class InputValidator
        {
            /// <summary>
            /// Validate inputs for the GetTerm method.  Throws DictionaryValidationException if the inputs
            /// are not valid.
            /// </summary>
            /// <param name="termID">The ID of the term being retrieved.  Must be greater than zero.</param>
            /// <param name="dictionary">The dictioanry to retrieve from.  Must be Term, Drug, or Genetic.</param>
            /// <param name="language">The desired result language. Must be English or Spanish.</param>
            public static void ValidateGetTerm(int termID, DictionaryType dictionary, Language language)
            {
                String message = string.Empty;
                bool failed = false;

                if (termID <= 0)
                {
                    failed = true;
                    message += string.Format("TermID is expected to be a positive number. Found '{0}' instead.", termID);
                }

                if (!Enum.IsDefined(typeof(DictionaryType), dictionary) || dictionary == DictionaryType.Unknown)
                {
                    failed = true;
                    message += "Dictionary must be 'Term', 'drug' or 'genetic'.\n";
                }

                if (!Enum.IsDefined(typeof(Language), language) || language == Language.Unknown)
                {
                    failed = true;
                    message += String.Format("Unsupported languge '{0}'.", language);
                }

                if (failed)
                {
                    log.Debug(message);
                    throw new DictionaryValidationException(message);
                }
            }

            /// <summary>
            /// Validate inputs for the GetTerm method.  Throws DictionaryValidationException if the inputs
            /// are not valid.
            /// </summary>
            /// <param name="termID">The ID of the term being retrieved.  Must be greater than zero.</param>
            /// <param name="dictionary">The dictioanry to retrieve from.  Must be Term, Drug, or Genetic.</param>
            /// <param name="language">The desired result language. Must be English or Spanish.</param>
            /// <param name="audience">The desired target audience. Must be Patient or HealthProfessional.</param>
            public static void ValidateGetTerm(int termID, DictionaryType dictionary, Language language, AudienceType audience)
            {
                String message = string.Empty;
                bool failed = false;

                if (termID <= 0)
                {
                    failed = true;
                    message += string.Format("TermID is expected to be a positive number. Found '{0}' instead.", termID);
                }

                if (!Enum.IsDefined(typeof(DictionaryType), dictionary) || dictionary == DictionaryType.Unknown)
                {
                    failed = true;
                    message += "Dictionary must be 'Term', 'drug' or 'genetic'.\n";
                }

                if (!Enum.IsDefined(typeof(Language), language) || language == Language.Unknown)
                {
                    failed = true;
                    message += String.Format("Unsupported languge '{0}'.", language);
                }

                if (!Enum.IsDefined(typeof(AudienceType), audience) || audience == AudienceType.Unknown)
                {
                    failed = true;
                    message += string.Format("Unsupported audience '{0}'.", audience);
                }

                if (failed)
                {
                    log.Debug(message);
                    throw new DictionaryValidationException(message);
                }
            }

            /// <summary>
            /// Validate inputs for the ValidateGetTermForAudience method.  Throws DictionaryValidationException
            /// if the inputs are not valid.
            /// </summary>
            /// <param name="termID">The ID of the term being retrieved.  Must be greater than zero.</param>
            /// <param name="language">The desired result language. Must be English or Spanish.</param>
            /// <param name="audience">The desired target audience. Must be Patient or HealthProfessional.</param>
            public static void ValidateGetTermForAudience(int termID, Language language, AudienceType audience)
            {
                String message = string.Empty;
                bool failed = false;

                if (termID <= 0)
                {
                    failed = true;
                    message += string.Format("TermID is expected to be a positive number. Found '{0}' instead.", termID);
                }

                if (!Enum.IsDefined(typeof(Language), language) || language == Language.Unknown)
                {
                    failed = true;
                    message += String.Format("Unsupported languge '{0}'.", language);
                }

                if (!Enum.IsDefined(typeof(AudienceType), audience) || audience == AudienceType.Unknown)
                {
                    failed = true;
                    message += string.Format("Unsupported audience '{0}'.", audience);
                }

                if (failed)
                {
                    log.Debug(message);
                    throw new DictionaryValidationException(message);
                }
            }

            /// <summary>
            /// Validate inputs for the Search method.  Throws DictionaryValidationException if the inputs
            /// are not valid.
            /// </summary>
            /// <param name="searchType">The type of search to being.  Must be Begins or Contains.</param>
            /// <param name="dictionary">The dictioanry to retrieve from.  Must be Term, Drug, or Genetic.</param>
            /// <param name="language">The desired result language. Must be English or Spanish.</param>
            public static void ValidateSearch(SearchType searchType, DictionaryType dictionary, Language language)
            {
                String message = string.Empty;
                bool failed = false;

                if (!Enum.IsDefined(typeof(SearchType), searchType)
                    && searchType != SearchType.Begins && searchType != SearchType.Contains)
                {
                    failed = true;
                    message += "Search type must be 'Begins' or 'Contains'.\n";
                }

                if (!Enum.IsDefined(typeof(DictionaryType), dictionary) || dictionary == DictionaryType.Unknown)
                {
                    failed = true;
                    message += "Dictionary must be 'Term', 'drug' or 'genetic'.\n";
                }

                if (!Enum.IsDefined(typeof(Language), language) || language == Language.Unknown)
                {
                    failed = true;
                    message += String.Format("Unsupported languge '{0}'.", language);
                }

                if (failed)
                {
                    log.Debug(message);
                    throw new DictionaryValidationException(message);
                }
            }

            /// <summary>
            /// Validate inputs for the SearchSuggest method.  Throws DictionaryValidationException if the inputs
            /// are not valid.
            /// </summary>
            /// <param name="searchType">The type of search to being.  Must be Begins, Contains, or Magic.</param>
            /// <param name="dictionary">The dictioanry to retrieve from.  Must be Term, Drug, or Genetic.</param>
            /// <param name="language">The desired result language. Must be English or Spanish.</param>
            public static void ValidateSearchSuggest(SearchType searchType, DictionaryType dictionary, Language language)
            {
                String message = string.Empty;
                bool failed = false;

                if (!Enum.IsDefined(typeof(SearchType), searchType) || searchType == SearchType.Unknown)
                {
                    failed = true;
                    message += "Search type must be 'Begins', 'Contains', or 'Magic'.\n";
                }

                if (!Enum.IsDefined(typeof(DictionaryType), dictionary) || dictionary == DictionaryType.Unknown)
                {
                    failed = true;
                    message += "Dictionary must be 'Term', 'drug' or 'genetic'.\n";
                }

                if (!Enum.IsDefined(typeof(Language), language) || language == Language.Unknown)
                {
                    failed = true;
                    message += String.Format("Unsupported languge '{0}'.", language);
                }

                if (failed)
                {
                    log.Debug(message);
                    throw new DictionaryValidationException(message);
                }
            }

            /// <summary>
            /// Validate inputs for the Expand method.  Throws DictionaryValidationException if the inputs
            /// are not valid.
            /// </summary>
            /// <param name="dictionary">The dictioanry to retrieve from.  Must be Term, Drug, or Genetic.</param>
            /// <param name="language">The desired result language. Must be English or Spanish.</param>
            public static void ValidateExpand(DictionaryType dictionary, Language language)
            {
                String message = string.Empty;
                bool failed = false;
                if (!Enum.IsDefined(typeof(DictionaryType), dictionary) || dictionary == DictionaryType.Unknown)
                {
                    failed = true;
                    message += "Dictionary must be 'Term', 'drug' or 'genetic'.\n";
                }

                if (!Enum.IsDefined(typeof(Language), language) || language == Language.Unknown)
                {
                    failed = true;
                    message += String.Format("Unsupported languge '{0}'.", language);
                }

                if (failed)
                {
                    log.Debug(message);
                    throw new DictionaryValidationException(message);
                }

            }
        }

        #endregion

        /// <summary>
        /// Retrieves a single dictionary Term based on its specific Term ID.
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
        /// <returns></returns>
        public TermReturn GetTerm(int termId, DictionaryType dictionary, Language language)
        {
            log.DebugFormat("Enter GetTerm( {0}, {1}, {2}).", termId, dictionary, language);

            AudienceType audience = AudienceType.Patient;

            // determine what audience to use based on the dictionary
            DictionaryManager dictionaryManager = new DictionaryManager();
            audience = dictionaryManager.GetDefaultAudienceFromDictionaryType(dictionary);

            // route to GetTerm with all arguments
            return GetTerm(termId, dictionary, language, audience);
        }

        /// <summary>
        /// Retrieves a single dictionary Term based on its specific Term ID.
        /// </summary>
        /// <param name="termId">The ID of the Term to be retrieved</param>
        /// <param name="dictionary">The dictionary to retreive the Term from.
        ///     Valid values are
        ///        term - Dictionary of Cancer Terms
        ///        drug - Drug Dictionary
        ///        genetic - Dictionary of Genetics Terms
        ///        NotSet - non-dictionary-specific definitions, often related to Health Professionals
        /// </param>
        /// <param name="language">The Term's desired language.
        ///     Supported values are:
        ///         en - English
        ///         es - Spanish
        /// </param>
        /// <param name="audience">The Term's desired target audience.
        ///     Supported values are:
        ///         Patient - terms for cancer patients
        ///         HealthProfessional - terms for health professionals
        /// </param>
        /// <returns></returns>
        [WebGet(ResponseFormat = WebMessageFormat.Json,
            UriTemplate = "v1/GetTerm?termID={termId}&language={language}&dictionary={dictionary}&audience={audience}")]
        [OperationContract]
        public TermReturn GetTerm(int termId, DictionaryType dictionary, Language language, AudienceType audience)
        {
            log.DebugFormat("Enter GetTerm( {0}, {1}, {2}, {3}).", termId, dictionary, language, audience);

            TermReturn ret = null;

            try
            {
                InputValidator.ValidateGetTerm(termId, dictionary, language, audience);

                DictionaryManager mgr = new DictionaryManager();
                ret = mgr.GetTerm(termId, dictionary, language, audience, API_VERSION);
            }
            // If there was a problem with the inputs for this request, fail with
            // an HTTP status message and an explanation.
            catch (DictionaryValidationException ex)
            {
                WebOperationContext ctx = WebOperationContext.Current;
                ctx.OutgoingResponse.SetStatusAsNotFound(ex.Message);
                ret = new TermReturn()
                {
                    Meta = new TermReturnMeta()
                    {
                        Messages = new string[] { ex.Message }
                    }
                };
            }

            log.Debug("Successfully retrieved a term.");

            return ret;
        }

        /// <summary>
        /// Retrieves a single dictionary term based on its specific Term ID.
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
        ///<param name="audience">The Term's desired audience.
        ///     Supported values are:
        ///         Patient
        ///         HealthProfessional
        /// </param>
        /// <returns></returns>
        public TermReturn GetTermForAudience(int termId, Language language, AudienceType audience)
        {
            log.DebugFormat("Enter GetTermForAudience( {0}, {1}, {2}).", termId, language, audience);

            DictionaryType dictionary = DictionaryType.term;

            switch (audience)
            {
                case AudienceType.HealthProfessional:
                    dictionary = DictionaryType.NotSet;
                    break;
                default:
                    dictionary = DictionaryType.term;
                    break;
            }

            return GetTerm(termId, dictionary, language, audience);
        }

        /// <summary>
        /// Performs a search for terms with names or aliases that start with or contain searchText.
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
        /// <returns>An object structure containing the results of the search and various metadata.</returns>
        [WebGet(ResponseFormat = WebMessageFormat.Json,
            UriTemplate = "v1/search?searchText={searchText}&searchType={searchType}&offset={offset}&numResults={numResults}&language={language}&dictionary={dictionary}")]
        [OperationContract]
        public SearchReturn Search(String searchText, SearchType searchType, int offset, int numResults, DictionaryType dictionary, Language language)
        {
            log.DebugFormat("Enter Search( {0}, {1}, {2}, {3}, {4}, {5}).", searchText, searchType, offset, numResults, dictionary, language);

            SearchReturn ret;

            try
            {
                InputValidator.ValidateSearch(searchType, dictionary, language);

                DictionaryManager mgr = new DictionaryManager();
                ret = mgr.Search(searchText, searchType, offset, numResults, dictionary, language, API_VERSION);

                log.DebugFormat("Returning {0} results.", ret.Result.Count());
            }
            // If there was a problem with the inputs for this request, fail with
            // an HTTP status message and an explanation.
            catch (DictionaryValidationException ex)
            {
                WebOperationContext ctx = WebOperationContext.Current;
                ctx.OutgoingResponse.SetStatusAsNotFound(ex.Message);
                ret = new SearchReturn()
                {
                    Meta = new SearchReturnMeta()
                    {
                        Messages = new string[] { ex.Message }
                    }
                };
            }

    
            return ret;
        }


        /// <summary>
        /// Lightweight method to search for terms matching searchText. This method is intended for use with autosuggest
        /// and returns a maximum of 10 results
        /// </summary>
        /// <param name="searchText">text to search for.</param>
        /// <param name="searchType">The type of search to perform.
        ///     Valid values are:
        ///         Begins - Search for terms beginning with searchText.
        ///         Contains - Search for terms containing searchText.
        ///         Magic - Search for terms beginning with searchText, followed by those containing searchText.
        /// </param>
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
        /// <returns></returns>
        [WebGet(ResponseFormat = WebMessageFormat.Json,
            UriTemplate = "v1/searchSuggest?searchText={searchText}&searchType={searchType}&language={language}&dictionary={dictionary}")]
        [OperationContract]
        public SuggestReturn SearchSuggest(String searchText, SearchType searchType, DictionaryType dictionary, Language language)
        {
            // This should possibly be made a parameter
            int MaxResultsAllowed = 10;

            log.DebugFormat("Enter ValidateSearchSuggest( {0}, {1}, {2}, {3}).", searchText, searchType, dictionary, language);

            SuggestReturn ret;

            try
            {
                InputValidator.ValidateSearch(searchType, dictionary, language);

                DictionaryManager mgr = new DictionaryManager();
                ret = mgr.SearchSuggest(searchText, searchType, MaxResultsAllowed, dictionary, language, API_VERSION);

                log.DebugFormat("Returning {0} results.", ret.Result.Count());
            }
            // If there was a problem with the inputs for this request, fail with
            // an HTTP status message and an explanation.
            catch (DictionaryValidationException ex)
            {
                WebOperationContext ctx = WebOperationContext.Current;
                ctx.OutgoingResponse.SetStatusAsNotFound(ex.Message);
                ret = new SuggestReturn()
                {
                    Meta = new SuggestReturnMeta()
                    {
                        Messages = new string[] { ex.Message }
                    }
                };
            }

            return ret;
        }

        /* It would be really awesome if includeTypes could be an array of values instead of a delimited list, but
         * that doesn't seem to be easily available "out of the box."  A possible solution is available at
         * http://stackoverflow.com/questions/6445171/passing-an-array-to-wcf-service-via-get, but this has
         * not yet been fully researched.
         */
        /// <summary>
        /// Perform a search for terms with names or aliases that start with searchText, sorted by the  matched term name or alias.
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
        /// <returns>An object structure containing the results of the search and various metadata.</returns>
        [WebGet(ResponseFormat = WebMessageFormat.Json,
            UriTemplate = "v1/expand?searchText={searchText}&includeTypes={includeTypes}&offset={offset}&numResults={numResults}&language={language}&dictionary={dictionary}")]
        [OperationContract]
        public SearchReturn Expand(String searchText, String includeTypes, int offset, int numResults, DictionaryType dictionary, Language language)
        {
            log.DebugFormat("Enter searchText( {0}, {1}, {2}, {3}, {4}, {5} ).", searchText, includeTypes, offset, numResults, dictionary, language);

            SearchReturn ret;

            try
            {
                InputValidator.ValidateExpand(dictionary, language);

                DictionaryManager mgr = new DictionaryManager();
                ret = mgr.Expand(searchText, includeTypes, offset, numResults, dictionary, language, API_VERSION);

                log.DebugFormat("Returning {0} results.", ret.Result.Count());
            }
            // If there was a problem with the inputs for this request, fail with
            // an HTTP status message and an explanation.
            catch (DictionaryValidationException ex)
            {
                WebOperationContext ctx = WebOperationContext.Current;
                ctx.OutgoingResponse.SetStatusAsNotFound(ex.Message);
                ret = new SearchReturn()
                {
                    Meta = new SearchReturnMeta()
                    {
                        Messages = new string[] { ex.Message }
                    }
                };
            }

            return ret;
        }

        /// <summary>
        /// Performs a check to see if the DictionaryEntryMetadata items provided exist in the database.
        /// </summary>
        /// <param name="entriesList">A list of DictionaryEntryMetadata items to validate in the DB.</param>
        /// <returns>A list of DictionaryEntryMetadata items.
        [WebGet(ResponseFormat = WebMessageFormat.Json,
            UriTemplate = "v1/doDictionaryEntriesExist")]
        [OperationContract]
        public List<DictionaryEntryMetadata> DoDictionaryEntriesExist(List<DictionaryEntryMetadata> entriesList)
        {
            List<DictionaryEntryMetadata> ret;

            try
            {
                DictionaryManager mgr = new DictionaryManager();
                ret = mgr.DoDictionaryEntriesExist(entriesList);

                log.DebugFormat("Returning {0} results.", ret.Count());
            }
            // If there was a problem with the inputs for this request, fail with
            // an HTTP status message and an explanation.
            catch (Exception ex)
            {
                WebOperationContext ctx = WebOperationContext.Current;
                ctx.OutgoingResponse.SetStatusAsNotFound(ex.Message);
                ret = new List<DictionaryEntryMetadata>();
            }

            return ret;
        }
    }
}
