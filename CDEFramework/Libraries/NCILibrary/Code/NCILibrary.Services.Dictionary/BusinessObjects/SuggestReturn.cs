using System.Runtime.Serialization;
using System.Text;

namespace NCI.Services.Dictionary.BusinessObjects
{
    /// <summary>
    /// Outermost data structure for returns from ValidateSearchSuggest().
    /// </summary>
    [DataContract()]
    public class SuggestReturn : IJsonizable
    {
        public SuggestReturn()
        {
            // Force collection to be non-null.
            Result = new DictionarySuggestion[] { };
        }

        [DataMember(Name = "meta")]
        public SuggestReturnMeta Meta { get; set; }

        [DataMember(Name = "result")]
        public DictionarySuggestion[] Result { get; set; }

        /// <summary>
        /// Hook for storing data members by calling the various AddMember() overloads.
        /// </summary>
        /// <param name="builder">The Jsonizer instance to use for storing data members.</param>
        public void Jsonize(Jsonizer builder)
        {
            builder.AddMember("meta", Meta, false);
            builder.AddMember("result", Result, true);
        }
    }
}
