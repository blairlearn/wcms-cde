using System;
using System.Web;

using NCI.Services.Dictionary.BusinessObjects;

namespace NCI.Services.Dictionary.Handler
{
    /// <summary>
    /// Subclass of Invoker for calling the Dictionary Service's Search method.
    /// </summary>
    internal class SearchInvoker : Invoker
    {
        // Parameters for the Search method.
        private String SearchText { get; set; }
        private SearchType SearchType { get; set; }
        private int Offset { get; set; }
        private int MaxResults { get; set; }
        private DictionaryType Dictionary { get; set; }
        private Language Language { get; set; }

        /// <summary>
        /// Initialization.  Use Invoker.Create() with method set to ApiMethodType.Search
        /// to instanatiate an GetTermInvoker object.
        /// </summary>
        /// <param name="request">The current request object.</param>
        public SearchInvoker(HttpRequest request)
            : base(request)
        {
            SearchText = GetSearchText();
            SearchType = GetSearchType();
            Offset = GetOffset();
            MaxResults = GetMaxResults();
            Dictionary = GetDictionary();
            Language = GetLanguage();
        }

        /// <summary>
        /// Invokes the GetTermInvoker() method.
        /// </summary>
        /// <returns>A data structure representing the results of
        /// the Search.</returns>
        public override IJsonizable Invoke()
        {
            return Service.Search(SearchText, SearchType, Offset, MaxResults, Dictionary, Language);
        }
    }
}
