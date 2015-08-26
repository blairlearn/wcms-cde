using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace NCI.Services.Dictionary.Handler
{
    internal class SearchSuggestInvoker : Invoker
    {
        private String SearchText { get; set; }
        private SearchType SearchType { get; set; }
        private DictionaryType Dictionary { get; set; }
        private Language Language { get; set; }

        public SearchSuggestInvoker(HttpRequest request)
            : base(request)
        {
            SearchText = GetSearchText();
            SearchType = GetSearchType();
            Dictionary = GetDictionary();
            Language = GetLanguage();
        }

        public override void Invoke()
        {
            Object o = Service.SearchSuggest(SearchText, SearchType, Dictionary, Language);
        }
    }
}
