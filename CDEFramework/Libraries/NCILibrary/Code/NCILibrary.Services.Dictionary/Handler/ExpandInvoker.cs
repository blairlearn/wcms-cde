
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace NCI.Services.Dictionary.Handler
{
    internal class ExpandInvoker : Invoker
    {
        private String SearchText { get; set; }
        private String IncludeTypes { get; set; }
        private int Offset { get; set; }
        private int MaxResults { get; set; }
        private DictionaryType Dictionary { get; set; }
        private Language Language { get; set; }

        public ExpandInvoker(HttpRequest request)
            : base(request)
        {
            SearchText = GetSearchText();
            IncludeTypes = GetIncludeTypes();
            Offset = GetOffset();
            MaxResults = GetMaxResults();
            Dictionary = GetDictionary();
            Language = GetLanguage();
        }

        public override void Invoke()
        {
            Object o = Service.Expand(SearchText, IncludeTypes, Offset, MaxResults, Dictionary, Language);
        }
    }
}
