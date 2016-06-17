using System.Web;

using NCI.Services.Dictionary.BusinessObjects;

namespace NCI.Services.Dictionary.Handler
{
    /// <summary>
    /// Subclass of Invoker for calling the Dictionary Service's GetTerm method.
    /// </summary>
    internal class GetTermInvoker : Invoker
    {
        // Parameters for the GetTerm method.
        private int TermID { get; set; }
        private DictionaryType Dictionary { get; set; }
        private Language Language { get; set; }
        private AudienceType AudienceType { get; set; }

        /// <summary>
        /// Initialization.  Use Invoker.Create() with method set to ApiMethodType.GetTerm
        /// to instanatiate an GetTermInvoker object.
        /// </summary>
        /// <param name="request">The current request object.</param>
        public GetTermInvoker(HttpRequest request)
            : base(request)
        {
            TermID = GetTermID();
            Dictionary = GetDictionaryWithDefaults();
            Language = GetLanguage();
            AudienceType = GetAudienceWithDefaults();
        }

        /// <summary>
        /// Invokes the GetTermInvoker() method.
        /// </summary>
        /// <returns>A data structure representing the results of
        /// the GetTerm call.</returns>
        public override IJsonizable Invoke()
        {
            return Service.GetTerm(TermID, Dictionary, Language, AudienceType);
        }
    }
}
