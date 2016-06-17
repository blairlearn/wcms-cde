using System;
using System.Collections.Specialized;
using System.Web;

using NCI.Logging;
using NCI.Util;

using NCI.Services.Dictionary.BusinessObjects;

namespace NCI.Services.Dictionary.Handler
{
    /// <summary>
    /// Provides infrastructure code, allowing the Dictionary Handler to generically
    /// invoke individual dictionary methods.
    /// 
    /// Individual subclasses are responsible for calling the GetXXXXX() methods
    /// to retrieve parameters needed for the method they invoke, and implementing
    /// an Invoke() method to call the parameter.
    /// </summary>
    abstract class Invoker
    {
        static Log log = new Log(typeof(Invoker));

        /// <summary>
        /// Stores a reference to the request's QueryString parameters.
        /// </summary>
        private NameValueCollection RawParams { get; set; }

        /// <summary>
        /// Stores a reference to an instance of the Dictionary Service.
        /// </summary>
        protected DictionaryService Service { get; private set; }

        /// <summary>
        /// Initialization.  Use Invoker.Create() to instanatiate an Invoker object.
        /// </summary>
        /// <param name="request">The current request object.</param>
        protected Invoker(HttpRequest request)
        {
            Service = new DictionaryService();
            RawParams = request.QueryString;
        }

        /// <summary>
        /// Creates an instance of a specific Invoker subclass.  Use this method instead of
        /// trying to directly instantiate an Invoker.
        /// </summary>
        /// <param name="method">An ApiMethodType value corresponding to the Dictionary Service method
        /// to be invoked.</param>
        /// <param name="request">The current HTTP request.</param>
        /// <returns>A dictionary service Invoker object.</returns>
        public static Invoker Create(ApiMethodType method, HttpRequest request)
        {
            Invoker invoker;

            switch (method)
            {
                case ApiMethodType.GetTerm:
                    invoker = new GetTermInvoker(request);
                    log.trace("Invoker.Create() - creating GetTermInvoker."); 
                    break;
                case ApiMethodType.Search:
                    invoker = new SearchInvoker(request);
                    log.trace("Invoker.Create() - creating SearchInvoker.");
                    break;
                case ApiMethodType.SearchSuggest:
                    invoker = new SearchSuggestInvoker(request);
                    log.trace("Invoker.Create() - creating SearchSuggestInvoker.");
                    break;
                case ApiMethodType.Expand:
                    invoker = new ExpandInvoker(request);
                    log.trace("Invoker.Create() - creating ExpandInvoker.");
                    break;

                case ApiMethodType.Unknown:
                default:
                    {
                        string msg = String.Format("Invoker Create() - Invalid method '{0}' requested.", method);
                        log.error(msg);
                        throw new DictionaryException(msg);
                    }
            }

            return invoker;
        }

        /// <summary>
        /// Subclasses are responsible for supplying an implementation of this method which
        /// invokes a specific dictionary service method.
        /// </summary>
        /// <returns>A data structure which can be converted to JSON.</returns>
        public abstract IJsonizable Invoke();

        /// <summary>
        /// Infrastructure method for retrieving the "termID" parameter.
        /// </summary>
        /// <returns>The value of the "termID" parameter.</returns>
        protected int GetTermID()
        {
            String rawValue = GetRequiredParameter("termID");
            int val = Strings.ToInt(rawValue);
            if (val <= 0)
                throw new ArgumentException("TermID must be greater than zero.");
            return val;
        }

        /// <summary>
        /// Infrastructure method for retrieving the "dictionary" parameter.
        /// </summary>
        /// <returns>The value of the "dictionary" parameter.</returns>
        protected DictionaryType GetDictionary()
        {
            String rawValue = GetRequiredParameter("dictionary");
            DictionaryType val = ConvertEnum<DictionaryType>.Convert(rawValue, DictionaryType.Unknown);
            if (val == DictionaryType.Unknown)
                throw new ArgumentException(String.Format("Unknown dictionary type '{0}'.", val));
            return val;
        }

        /// <summary>
        /// Infrastructure method for retrieving the "dictionary" parameter, returning appropriate Audience-aware 
        /// default values if dictionary is not otherwise set.
        /// </summary>
        /// <returns>The value of the "dictionary" parameter.</returns>
        protected DictionaryType GetDictionaryWithDefaults()
        {
            String rawValue = GetOptionalParameter("dictionary");
            rawValue = Strings.Clean(rawValue, "Unknown");
            
            DictionaryType val = ConvertEnum<DictionaryType>.Convert(rawValue, DictionaryType.Unknown);
            if (val == DictionaryType.Unknown)
            {
                // is dictionary is unknown, check Audience and set default value
                String audienceValue = GetOptionalParameter("audience");
                audienceValue = Strings.Clean(audienceValue, "Unknown");

                AudienceType audience = ConvertEnum<AudienceType>.Convert(audienceValue, AudienceType.Unknown);
                switch(audience)
                {
                    case AudienceType.HealthProfessional:
                        val = DictionaryType.NotSet;
                        break;
                    default:
                        val = DictionaryType.term;
                        break;
                }
            }
            return val;
        }


        /// <summary>
        /// Infrastructure method for retrieving the "language" parameter.
        /// </summary>
        /// <returns>The value of the "language" parameter.</returns>
        protected Language GetLanguage()
        {
            String rawValue = GetRequiredParameter("language");
            Language val = ConvertEnum<Language>.Convert(rawValue, Language.Unknown);
            if (val == Language.Unknown)
                throw new ArgumentException(String.Format("Unknown language '{0}'.", val));
            return val;
        }

        /// <summary>
        /// Infrastructure method for retrieving the "searchText" parameter.
        /// </summary>
        /// <returns>The value of the "searchText" parameter.</returns>
        protected String GetSearchText()
        {
            return GetRequiredParameter("searchText");
        }

        /// <summary>
        /// Infrastructure method for retrieving the "searchType" parameter.
        /// </summary>
        /// <returns>The value of the "searchType" parameter.</returns>
        protected SearchType GetSearchType()
        {
            String rawValue = GetRequiredParameter("searchType");
            SearchType val = ConvertEnum<SearchType>.Convert(rawValue, SearchType.Unknown);
            if (val == SearchType.Unknown)
                throw new ArgumentException(String.Format("Unknown searchType '{0}'.", val));
            return val;
        }

        /// <summary>
        /// Infrastructure method for retrieving the "offset" parameter.
        /// </summary>
        /// <returns>The value of the "offset" parameter.</returns>
        protected int GetOffset()
        {
            string rawValue = GetOptionalParameter("offset");
            int val = Strings.ToInt(rawValue, 0);
            return val;
        }

        /// <summary>
        /// Infrastructure method for retrieving the "numResults" parameter.
        /// </summary>
        /// <returns>The value of the "numResults" parameter.</returns>
        protected int GetNumResults()
        {
            string rawValue = GetOptionalParameter("numResults");
            int val = Strings.ToInt(rawValue, 100);
            return val;
        }

        /// <summary>
        /// Infrastructure method for retrieving the "includeTypes" parameter.
        /// </summary>
        /// <returns>The value of the "includeTypes" parameter.</returns>
        protected String GetIncludeTypes()
        {
            return GetOptionalParameter("includeTypes");
        }

        /// <summary>
        /// Low-level infrastructure for retrieving required query-string parameters.
        /// Throws ArgumentException if the parameter is not available.
        /// </summary>
        /// <param name="name">Name of the parameter to retrieve</param>
        /// <returns>The string value of the parameter.</returns>
        private String GetRequiredParameter(String name)
        {
            string value = RawParams[name];

            if (String.IsNullOrEmpty(value))
                throw new ArgumentException(string.Format("Required parameter '{0}' not found.", name));

            return value.Trim();
        }

        /// <summary>
        /// Low-level infrastructure for retrieving optional query-string parameters.
        /// </summary>
        /// <param name="name">Name of the parameter to retrieve</param>
        /// <returns>The string value of the parameter, or null if the parameter doesn't exist.</returns>
        private String GetOptionalParameter(String name)
        {
            return RawParams[name];
        }

        /// <summary>
        /// Infrastructure method for retrieving the "audience" parameter, returning appropriate Dictionary-aware 
        /// default values if audience is not otherwise set.
        /// </summary>
        /// <returns>The value of the "audience" parameter.</returns>
        protected AudienceType GetAudienceWithDefaults()
        {
            String rawValue = GetOptionalParameter("audience");
            rawValue = Strings.Clean(rawValue, "Unknown");

            AudienceType val = ConvertEnum<AudienceType>.Convert(rawValue, AudienceType.Unknown);
            if (val == AudienceType.Unknown)
            {
                // is dictionary is unknown, check Audience and set default value
                String dictionaryValue = GetOptionalParameter("dictionary");
                dictionaryValue = Strings.Clean(dictionaryValue, "Unknown");

                DictionaryType dictionary = ConvertEnum<DictionaryType>.Convert(dictionaryValue, DictionaryType.Unknown);
                switch (dictionary)
                {
                    case DictionaryType.NotSet:
                    case DictionaryType.genetic:
                        val = AudienceType.HealthProfessional;
                        break;
                    default:
                        val = AudienceType.Patient;
                        break;
                }
            }
            return val;
        }


    }
}
