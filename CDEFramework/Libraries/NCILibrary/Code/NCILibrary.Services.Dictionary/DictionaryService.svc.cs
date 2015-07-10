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
    ///    
    /// </summary>
    /// <remarks>
    /// Changing the class name "DictionaryService" here requires that you also update the reference
    /// to "DictionaryService" in Web.config and the associated .svc file.
    /// </remarks>
    public class DictionaryService : IDictionaryService
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

        public SearchReturn Search(String searchText, SearchType searchType, int offset, int maxResults, DictionaryType dictionary, Language language)
        {
            DictionaryManager mgr = new DictionaryManager();
            SearchReturn ret = mgr.Search(searchText, searchType, offset, maxResults, dictionary, language);

            return ret;
        }

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
