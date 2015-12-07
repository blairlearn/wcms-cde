using System;
using System.Web;

using NCI.Services.Dictionary.BusinessObjects;

namespace NCI.Services.Dictionary.Handler
{
    /// <summary>
    /// Subclass of Invoker for calling the Dictionary Service's SearchSuggest method.
    /// </summary>
    internal class SearchSuggestInvoker : Invoker
    {
        // Parameters for the Search method.
        private String SearchText { get; set; }
        private SearchType SearchType { get; set; }
        private DictionaryType Dictionary { get; set; }
        private Language Language { get; set; }

        /// <summary>
        /// Initialization.  Use Invoker.Create() with method set to ApiMethodType.SearchSuggest
        /// to instanatiate an SearchSuggestInvoker object.
        /// </summary>
        /// <param name="request">The current request object.</param>
        public SearchSuggestInvoker(HttpRequest request)
            : base(request)
        {
            SearchText = GetSearchText();
            SearchType = GetSearchType();
            Dictionary = GetDictionary();
            Language = GetLanguage();
        }

        /// <summary>
        /// Invokes the GetTermInvoker() method.
        /// </summary>
        /// <returns>A data structure containing the Search suggestion.</returns>
        public override IJsonizable Invoke()
        {
            return Service.SearchSuggest(SearchText, SearchType, Dictionary, Language);
        }
    }
}
