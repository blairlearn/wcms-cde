using System;
using System.Runtime.Serialization;

namespace NCI.Services.Dictionary.BusinessObjects
{
    /// <summary>
    /// represents an external link
    /// </summary>
    [DataContract()]
    public class RelatedExternalLink
    {
        /// <summary>
        /// the external link
        /// </summary>
        [DataMember(Name = "url")]
        public String Url { get; set; }

        /// <summary>
        /// language to include the link with.
        /// </summary>
        [DataMember(Name = "language")]
        public String Language { get; set; }

        /// <summary>
        /// link text
        /// </summary>
        [DataMember(Name = "text")]
        public String Text { get; set; }
    }
}
