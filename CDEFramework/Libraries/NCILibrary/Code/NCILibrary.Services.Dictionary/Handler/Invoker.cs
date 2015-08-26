using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Web;

using NCI.Logging;
using NCI.Util;

using NCI.Services.Dictionary.BusinessObjects;

namespace NCI.Services.Dictionary.Handler
{
    /// <summary>
    /// Base class for invoking individual service members.
    /// Provides infrastructure methods for gathering individul parameters.
    /// </summary>
    abstract class Invoker
    {
        static Log log = new Log(typeof(Invoker));

        protected NameValueCollection RawParams { get; private set; }

        protected DictionaryService Service { get; private set; }

        protected Invoker(HttpRequest request)
        {
            Service = new DictionaryService();
            RawParams = request.QueryString;
        }

        public static Invoker Create(ApiMethodType method, HttpRequest request)
        {
            Invoker invoker;

            switch (method)
            {
                case ApiMethodType.GetTerm:
                    invoker = new GetTermInvoker(request);
                    break;
                case ApiMethodType.Search:
                    invoker = new SearchInvoker(request);
                    break;
                case ApiMethodType.SearchSuggest:
                    invoker = new SearchSuggestInvoker(request);
                    break;
                case ApiMethodType.Expand:
                    invoker = new ExpandInvoker(request);
                    break;

                case ApiMethodType.Unknown:
                default:
                    throw new DictionaryException(method.ToString());
            }

            return invoker;
        }

        public abstract void Invoke();

        protected int GetTermID()
        {
            String rawValue = GetRequiredParameter("termID");
            int val = Strings.ToInt(rawValue);
            if (val <= 0)
                throw new ArgumentException("TermID must be greater than zero.");
            return val;
        }

        protected DictionaryType GetDictionary()
        {
            String rawValue = GetRequiredParameter("dictionary");
            DictionaryType val = ConvertEnum<DictionaryType>.Convert(rawValue, DictionaryType.Unknown);
            if (val == DictionaryType.Unknown)
                throw new ArgumentException(String.Format("Unknown dictionary type '{0}'.", val));
            return val;
        }

        protected Language GetLanguage()
        {
            String rawValue = GetRequiredParameter("language");
            Language val = ConvertEnum<Language>.Convert(rawValue, Language.Unknown);
            if (val == Language.Unknown)
                throw new ArgumentException(String.Format("Unknown language '{0}'.", val));
            return val;
        }

        protected String GetSearchText()
        {
            return GetRequiredParameter("searchText");
        }

        protected SearchType GetSearchType()
        {
            String rawValue = GetRequiredParameter("searchType");
            SearchType val = ConvertEnum<SearchType>.Convert(rawValue, SearchType.Unknown);
            if (val == SearchType.Unknown)
                throw new ArgumentException(String.Format("Unknown searchType '{0}'.", val));
            return val;
        }

        protected int GetOffset()
        {
            string rawValue = GetOptionalParameter("offset");
            int val = Strings.ToInt(rawValue, 0);
            return val;
        }

        protected int GetMaxResults()
        {
            string rawValue = GetOptionalParameter("maxResults");
            int val = Strings.ToInt(rawValue, 100);
            return val;
        }

        protected String GetIncludeTypes()
        {
            return GetOptionalParameter("includeTypes");
        }


        private String GetRequiredParameter(String name)
        {
            string value = RawParams[name];

            if (String.IsNullOrEmpty(value))
                throw new ArgumentException(string.Format("Required parameter '{0}' not found.", name));

            return value.Trim();
        }

        private String GetOptionalParameter(String name)
        {
            return RawParams[name];
        }

        private int GetRequiredIntParameter(String name)
        {
            string value = RawParams[name];

            if (String.IsNullOrEmpty(value))
                throw new ArgumentException(string.Format("Required parameter '{0}' not found.", name));

            return Strings.ToInt(value);
        }

    }
}
