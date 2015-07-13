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
                    messages.Add("Dictionary must be 'term', 'drug' or 'genetic'.");

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
        /// Retrieves a single dictionary term based on its specific term ID.
        /// </summary>
        /// <param name="termId">The ID of the term to be retrieved</param>
        /// <param name="dictionary">The dictionary to retreive the term from.
        ///     Valid values are
        ///        term - Dictionary of Cancer Terms
        ///        drug - Drug Dictionary
        ///        genetic - Dictionary of Genetics Terms
        /// </param>
        /// <param name="language">The term's desired language.
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
            InputValidator validator = new InputValidator(dictionary, language);
            if (validator.IsValid())
            {

                DictionaryManager mgr = new DictionaryManager();

                TermReturn ret = mgr.GetTerm(termId, dictionary, language, API_VERSION);
                return ret;
            }
            else
                return validator.GetInvalidResult();
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
        /// <param name="dictionary">The dictionary to retreive the term from.
        ///     Valid values are
        ///        term - Dictionary of Cancer Terms
        ///        drug - Drug Dictionary
        ///        genetic - Dictionary of Genetics Terms
        /// </param>
        /// <param name="language">The term's desired language.
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
            DictionaryManager mgr = new DictionaryManager();
            SearchReturn ret = mgr.Search(searchText, searchType, offset, maxResults, dictionary, language);

            return ret;
        }


        /// <summary>
        /// Performs a search for terms with names matching searchText.  This alternate version is
        /// invoked by a POST request.
        /// </summary>
        /// <param name="paramBlock"></param>
        /// <param name="dictionary">The dictionary to retreive the term from.
        ///     Valid values are
        ///        term - Dictionary of Cancer Terms
        ///        drug - Drug Dictionary
        ///        genetic - Dictionary of Genetics Terms
        /// </param>
        /// <param name="language">The term's desired language.
        ///     Supported values are:
        ///         en - English
        ///         es - Spanish
        /// </param>
        /// <returns></returns>
        [WebInvoke(ResponseFormat = WebMessageFormat.Json,
            UriTemplate = "v1/SearchPost")]
        [OperationContract]
        public SearchReturn SearchPost(SearchInputs paramBlock)
        {
            // Convert paramBlock to individual values and re-use the GET version of Search.
            String searchText = paramBlock.searchText;
            SearchType searchType = paramBlock.searchType;

            DictionaryType dictionary = paramBlock.dictionaryType;
            Language language = paramBlock.languge;

            int offset = paramBlock.offset;
            int maxResults = paramBlock.maxResults;

            return Search(searchText, searchType, offset, maxResults, dictionary, language);
        }
    }
}
