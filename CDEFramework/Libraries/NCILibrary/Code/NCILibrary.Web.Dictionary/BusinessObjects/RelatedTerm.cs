using System;
using System.Runtime.Serialization;

namespace NCI.Services.Dictionary.BusinessObjects
{
    /// <summary>
    /// represents a single related Term
    /// </summary>
    [DataContract()]
    public class RelatedTerm
    {

        /// <summary>
        /// related Term's ID
        /// </summary>
        [DataMember(Name = "id")]
        public String Termid { get; set; }

        /// <summary>
        /// identifies thpublic Stringe dictionary {get;set;}
        /// </summary>
        [DataMember(Name = "dictionary")]
        public String Dictionary { get; set; }

        /// <summary>
        /// link text
        /// </summary>
        [DataMember(Name = "text")]
        public String Text { get; set; }
    }
}
