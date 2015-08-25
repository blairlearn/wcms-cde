using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Runtime.Serialization;

namespace NCI.Services.Dictionary.BusinessObjects
{
    [DataContract()]
    public class ExpandReturnMeta : MetaCommon
    {
        [DataMember(Name = "offset")]
        public int Offset { get; set; }

        /// <summary>
        /// The total number of terms matching the request
        /// </summary>
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
