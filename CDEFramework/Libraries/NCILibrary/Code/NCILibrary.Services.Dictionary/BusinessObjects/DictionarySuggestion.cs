using System;
using System.Runtime.Serialization;

namespace NCI.Services.Dictionary.BusinessObjects
{
    [DataContract()]
    public class DictionarySuggestion
    {
        // For serialization
        public DictionarySuggestion()
        {
        }

        public DictionarySuggestion(int id, String term)
        {
            this.ID = id.ToString();
            this.Term = term;
        }

        /// <summary>
        ///  The Term's ID
        /// </summary>
        [DataMember(Name = "id")]
        // TODO:  Change this from String to Int without breaking the dictionary app manager.
        public String ID { get; set; }

        /// <summary>
        /// The Term's name
        /// </summary>
        [DataMember(Name = "term")]
        public String Term { get; set; }
    }
}
