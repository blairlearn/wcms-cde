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
    // NOTE: If you change the class name "DictionaryService" here, you must also update the reference to "DictionaryService" in Web.config and the associated .svc file.
    public class DictionaryService : IDictionaryService
    {
        const String API_VERSION = "v1";

        #region private class InputValidator

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

        public TermReturn GetTerm(String termId, String dictionary, String language)
        {
            DictionaryType dict = GetDictionaryType(dictionary);
            Language lang = GetLanguage(language);

            InputValidator validator = new InputValidator(dict, lang);
            if (validator.IsValid())
            {

                DictionaryManager mgr = new DictionaryManager();

                TermReturn ret = mgr.GetTerm(termId, dict, lang, API_VERSION);
                return ret;
            }
            else
                return validator.GetInvalidResult();
        }

        // Placeholder.  We really want to return something which *contains* an array.
        public DictionaryTerm[] Search(String param1, String param2, String dictionary, String language)
        {
            return new DictionaryTerm[] { };
        }

        // Placeholder.  We really want to return something which *contains* an array.
        public DictionaryTerm[] SearchPost(SearchInputs paramBlock, String dictionary, String language)
        {
            return new DictionaryTerm[] { };
        }


        private DictionaryType GetDictionaryType(string dictionary)
        {
            DictionaryType type = ConvertEnum<DictionaryType>.Convert(dictionary, DictionaryType.Unknown);
            return type;
        }

        private Language GetLanguage(string language)
        {
            Language type = Language.Unknown;
            if (!String.IsNullOrEmpty(language))
            {
                language = language.ToLower(CultureInfo.CurrentCulture).Trim();
                switch (language)
                {
                    case "en": type = Language.English; break;
                    case "es": type = Language.Spanish; break;
                }
            }

            return type;
        }
    }
}
