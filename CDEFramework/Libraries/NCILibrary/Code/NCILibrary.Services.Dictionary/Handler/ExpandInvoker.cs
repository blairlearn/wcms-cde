using System;
using System.Web;

using NCI.Services.Dictionary.BusinessObjects;

namespace NCI.Services.Dictionary.Handler
{
    /// <summary>
    /// Subclass of Invoker for calling the Dictionary Service's Expand method.
    /// </summary>
    internal class ExpandInvoker : Invoker
    {
        // Parameters for the Expand method.
        private String SearchText { get; set; }
        private String IncludeTypes { get; set; }
        private int Offset { get; set; }
        private int MaxResults { get; set; }
        private DictionaryType Dictionary { get; set; }
        private Language Language { get; set; }

        /// <summary>
        /// Initialization.  Use Invoker.Create() with method set to ApiMethodType.Expand
        /// to instanatiate an ExpandInvoker object.
        /// </summary>
        /// <param name="request">The current request object.</param>
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

        /// <summary>
        /// Invokes the Expand() method.
        /// </summary>
        /// <returns>A data structure representing the results of
        /// the Expand search.</returns>
        public override IJsonizable Invoke()
        {
            return Service.Expand(SearchText, IncludeTypes, Offset, MaxResults, Dictionary, Language);
        }
    }
}
