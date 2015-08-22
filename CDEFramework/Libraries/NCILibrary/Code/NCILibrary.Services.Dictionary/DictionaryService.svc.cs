using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;

using NCI.Logging;
using NCI.Services.Dictionary.BusinessObjects;
using NCI.Util;

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
        static Log log = new Log(typeof(DictionaryService));

        const String API_VERSION = "v1";

        #region private class InputValidator

        /// <summary>
        /// Validates input for the DictionaryService public interface.
        /// </summary>
        private static class InputValidator
        {
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
                    log.debug(message);
                    throw new DictionaryValidationException(message);
                }
            }

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
                    log.debug(message);
                    throw new DictionaryValidationException(message);
                }
            }

            public static void SearchSuggest(SearchType searchType, DictionaryType dictionary, Language language)
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
                    log.debug(message);
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
        [WebGet(ResponseFormat = WebMessageFormat.Json,
            UriTemplate = "v1/GetTerm?termID={termId}&language={language}&dictionary={dictionary}")]
        [OperationContract]
        public TermReturn GetTerm(int termId, DictionaryType dictionary, Language language)
        {
            log.debug(string.Format("Enter GetTerm( {0}, {1}, {2}).", termId, dictionary, language));

            TermReturn ret = null;

            try
            {
                InputValidator.ValidateGetTerm(termId, dictionary, language);

                DictionaryManager mgr = new DictionaryManager();
                ret = mgr.GetTerm(termId, dictionary, language, API_VERSION);
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

            log.debug("Successfully retrieved a term.");

            return ret;
        }


        /// <summary>
        /// Performs a search for terms with names matching searchText.
        /// </summary>
        /// <param name="searchText">text to search for.</param>
        /// <param name="searchType">The type of search to perform.
        ///     Valid values are:
        ///         Begins - Search for terms beginning with searchText.
        ///         Contains - Search for terms containing searchText.
        ///         Magic - Search for terms beginning with searchText, followed by those containing searchText.
        /// </param>
        /// <param name="offset">Offset into the list of matches for the first result to return.</param>
        /// <param name="maxResults">The maximum number of results to return. Must be at least 10.</param>
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
            UriTemplate = "v1/search?searchText={searchText}&searchType={searchType}&offset={offset}&maxResults={maxResults}&language={language}&dictionary={dictionary}")]
        [OperationContract]
        public SearchReturn Search(String searchText, SearchType searchType, int offset, int maxResults, DictionaryType dictionary, Language language)
        {
            log.debug(string.Format("Enter Search( {0}, {1}, {2}, {3}, {4}, {5}).", searchText, searchType, offset, maxResults, dictionary, language));

            SearchReturn ret;

            try
            {
                InputValidator.ValidateSearch(searchType, dictionary, language);

                DictionaryManager mgr = new DictionaryManager();
                ret = mgr.Search(searchText, searchType, offset, maxResults, dictionary, language, API_VERSION);

                log.debug(string.Format("Returning {0} results.", ret.Result.Count()));
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

            log.debug(string.Format("Enter SearchSuggest( {0}, {1}, {2}, {3}).", searchText, searchType, dictionary, language));

            SuggestReturn ret;

            try
            {
                InputValidator.ValidateSearch(searchType, dictionary, language);

                DictionaryManager mgr = new DictionaryManager();
                ret = mgr.SearchSuggest(searchText, searchType, MaxResultsAllowed, dictionary, language, API_VERSION);

                log.debug(string.Format("Returning {0} results.", ret.Result.Count()));
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
    }
}
