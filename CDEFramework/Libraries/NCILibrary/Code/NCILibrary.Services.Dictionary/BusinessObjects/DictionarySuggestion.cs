using System;
using System.Runtime.Serialization;

namespace NCI.Services.Dictionary.BusinessObjects
{
    /// <summary>
    /// Data structure containing a single suggested term from the Dictionary
    /// Service's SearchSuggest method.
    /// </summary>
    [DataContract()]
    public class DictionarySuggestion : IJsonizable
    {
        // Required for serialization
        public DictionarySuggestion()
        {
        }

        /// <summary>
        /// Initialization
        /// </summary>
        /// <param name="id">The term's ID</param>
        /// <param name="term">Name of a term.</param>
        public DictionarySuggestion(int id, String term)
        {
            this.ID = id;
            this.Term = term;
        }

        /// <summary>
        ///  The Term's ID
        /// </summary>
        [DataMember(Name = "id")]
        public int ID { get; set; }

        /// <summary>
        /// The Term's name
        /// </summary>
        [DataMember(Name = "term")]
        public String Term { get; set; }


        /// <summary>
        /// Hook for storing data members by calling the various AddMember() overloads.
        /// </summary>
        /// <param name="builder">The Jsonizer instance to use for storing data members.</param>
        public void Jsonize(Jsonizer builder)
        {
            builder.AddMember("id", ID, false);
            builder.AddMember("term", Term, true);
        }
    }
}
