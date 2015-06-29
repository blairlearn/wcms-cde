﻿using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;

using NCI.Services.Dictionary.BusinessObjects;
using NCI.Util;

namespace NCI.Services.Dictionary
{
    // NOTE: If you change the class name "Service1" here, you must also update the reference to "Service1" in Web.config and in the associated .svc file.
    public class DictionaryService : IDictionaryService
    {
        #region private class InputValidator

        private class InputValidator
        {
            private String apiVersion;
            private DictionaryType dictionary;
            private Language language;

            public InputValidator(String apiVersion, DictionaryType dictionary, Language language)
            {
                this.apiVersion = apiVersion.ToLower(CultureInfo.CurrentCulture).Trim();
                this.dictionary = dictionary;
                this.language = language;
            }

            public bool IsValid()
            {
                bool isValid = Enum.IsDefined(typeof(DictionaryType), dictionary) && dictionary != DictionaryType.Unknown
                    && Enum.IsDefined(typeof(Language), language) && language != Language.Unknown
                    && !String.IsNullOrEmpty(apiVersion) && apiVersion == "v1";

                return isValid;
            }

            public TermReturn GetInvalidResult()
            {
                List<String> messages = new List<string>();

                if (!Enum.IsDefined(typeof(DictionaryType), dictionary) || dictionary == DictionaryType.Unknown)
                    messages.Add("Dictionary must be 'term', 'drug' or 'genetic'.");

                if (!Enum.IsDefined(typeof(Language), language) || language == Language.Unknown)
                    messages.Add("Language must be 'en' or 'es'.");

                if (String.IsNullOrEmpty(apiVersion) || apiVersion != "v1")
                    messages.Add("API version must be 'v1'.");

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

        public TermReturn GetTerm(String termId, String dictionary, String language, String version)
        {
            DictionaryType dict = GetDictionaryType(dictionary);
            Language lang = GetLanguage(language);

            InputValidator validator = new InputValidator(version, dict, lang);
            if (validator.IsValid())
            {

                DictionaryManager mgr = new DictionaryManager();

                TermReturn ret = mgr.GetTerm(termId, dict, lang, version);
                return ret;
            }
            else
                return validator.GetInvalidResult();
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