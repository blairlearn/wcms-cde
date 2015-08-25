using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace NCI.Services.Dictionary.BusinessObjects
{
    [DataContract()]
    public class DictionaryExpansion
    {

        /// <summary>
        /// Needed for serialization
        /// </summary>
        public DictionaryExpansion()
        {
        }

        public DictionaryExpansion(int id, String termName, String termDetail)
        {
            this.ID = id;
            this.MatchedTerm = termName;
            this.TermDetail = termDetail;
        }

        /// <summary>
        ///  The Term's ID
        /// </summary>
        [DataMember(Name = "id")]
        public int ID { get; set; }

        /// <summary>
        /// The Term name (or alias) that matched the expansion
        /// </summary>
        [DataMember(Name = "matched")]
        public String MatchedTerm { get; set; }

        /// <summary>
        /// String containg the JSON structure for the matched term definition.
        /// </summary>
        [DataMember(Name = "term")]
        public String TermDetail { get; set; }
    }
}
