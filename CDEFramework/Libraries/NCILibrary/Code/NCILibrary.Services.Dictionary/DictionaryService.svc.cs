using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;

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
        const String API_VERSION = "v1";

        #region private class InputValidator

        /// <summary>
        /// 
        /// </summary>
        private class InputValidator
        {
            private DictionaryType dictionary;
            private Language language;

            public InputValidator(DictionaryType dictionary, Language language)
            {
                this.dictionary = dictionary;
                this.language = language;
            }

            public bool IsValid()
            {
                bool isValid = Enum.IsDefined(typeof(DictionaryType), dictionary) && dictionary != DictionaryType.Unknown
                    && Enum.IsDefined(typeof(Language), language) && language != Language.Unknown;

                return isValid;
            }

            public TermReturn GetInvalidResult()
            {
                List<String> messages = new List<string>();

                if (!Enum.IsDefined(typeof(DictionaryType), dictionary) || dictionary == DictionaryType.Unknown)
                    messages.Add("Dictionary must be 'Term', 'drug' or 'genetic'.");

                if (!Enum.IsDefined(typeof(Language), language) || language == Language.Unknown)
                    messages.Add("Language must be 'en' or 'es'.");

                return new TermReturn()
                {
                    Meta = new TermReturnMeta()
                    {
                        Messages = messages.ToArray(),
                        Language = language.ToString(),
                        Audience = "n/a"
                    },
                    Term = null
                };
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
        public TermReturn GetTerm(String termId, DictionaryType dictionary, Language language)
        {
            TermReturn ret = null;

            try
            {
                InputValidator validator = new InputValidator(dictionary, language);
                if (validator.IsValid())
                {
                    DictionaryManager mgr = new DictionaryManager();

                    ret = mgr.GetTerm(termId, dictionary, language, API_VERSION);
                }
                else
                    ret = validator.GetInvalidResult();
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
                        Messages = new string[] {ex.Message}
                    }
                };
            }

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
            SearchReturn ret;

            try
            {
                DictionaryManager mgr = new DictionaryManager();
                ret = mgr.Search(searchText, searchType, offset, maxResults, dictionary, language);

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
            SuggestReturn ret;

            try
            {
                DictionaryManager mgr = new DictionaryManager();
                ret = mgr.SearchSuggest(searchText, searchType, dictionary, language);

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
