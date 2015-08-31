using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Runtime.Serialization;

namespace NCI.Web.Dictionary.BusinessObjects
{
    [DataContract()]
    public class SearchReturnMeta : MetaCommon
    {
        public int Offset { get; set; }

        /// <summary>
        /// The total number of terms matching the request
        /// </summary>
        public int ResultCount { get; set; }

        /// <summary>
        /// The Term's audience Patient or HealthProfessional
        /// </summary>
        public String Audience { get; set; }

        /// <summary>
        /// The Term's language
        /// </summary>
        public String Language { get; set; }
    }
}
