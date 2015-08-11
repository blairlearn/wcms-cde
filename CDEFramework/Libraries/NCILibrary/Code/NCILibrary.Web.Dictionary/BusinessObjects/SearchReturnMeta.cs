using System;
using System.Runtime.Serialization;

namespace NCI.Services.Dictionary.BusinessObjects
{
    /// <summary>
    /// Metadata about a call to GetTerm().
    /// </summary>
    [DataContract()]
    public class SearchReturnMeta : MetaCommon
    {
        [DataMember(Name = "offset")]
        public int Offset { get; set; }

        [DataMember(Name = "result_count")]
        public int ResultCount { get; set; }

        /// <summary>
        /// The Term's audience Patient or HealthProfessional
        /// </summary>
        [DataMember(Name = "audience")]
        public String Audience { get; set; }

        /// <summary>
        /// The Term's language
        /// </summary>
        [DataMember(Name = "language")]
        public String Language { get; set; }
    }
}
