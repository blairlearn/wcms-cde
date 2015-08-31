using System.Runtime.Serialization;

namespace NCI.Web.Dictionary.BusinessObjects
{
    /// <summary>
    /// Outermost data structure for returns from ValidateSearchSuggest().
    /// </summary>
    public class SuggestReturn
    {
        public SuggestReturn()
        {
            // Force collection to be non-null.
            Result = new DictionarySuggestion[] { };
        }

        public SuggestReturnMeta Meta { get; set; }

        public DictionarySuggestion[] Result { get; set; }
    }
}
