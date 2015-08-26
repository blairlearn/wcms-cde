
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace NCI.Services.Dictionary.Handler
{
    internal class SearchInvoker : Invoker
    {
        private String SearchText { get; set; }
        private SearchType SearchType { get; set; }
        private int Offset { get; set; }
        private int MaxResults { get; set; }
        private DictionaryType Dictionary { get; set; }
        private Language Language { get; set; }

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

        public override void Invoke()
        {
            Object o = Service.Search(SearchText, SearchType, Offset, MaxResults, Dictionary, Language);
        }
    }
}
