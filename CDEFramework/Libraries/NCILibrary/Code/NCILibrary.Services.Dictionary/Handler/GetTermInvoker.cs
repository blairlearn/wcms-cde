using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using NCI.Services.Dictionary.BusinessObjects;

namespace NCI.Services.Dictionary.Handler
{
    internal class GetTermInvoker : Invoker
    {
        private int TermID { get; set; }
        private DictionaryType Dictionary { get; set; }
        private Language Language { get; set; }

        public GetTermInvoker(HttpRequest request)
            : base(request)
        {
            TermID = GetTermID();
            Dictionary = GetDictionary();
            Language = GetLanguage();
        }

        public override IJsonizable Invoke()
        {
            return Service.GetTerm(TermID, Dictionary, Language);
        }
    }
}
