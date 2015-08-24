using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace NCI.Web.Dictionary.BusinessObjects
{
    [DataContract()]
    public class DictionaryExpansion
    {
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
        /// Dictionary Terms
        /// </summary>
        [DataMember(Name = "term")]
        public DictionaryTerm Term { get; set; }
    }
}
