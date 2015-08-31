using System;
using System.Runtime.Serialization;

namespace NCI.Web.Dictionary.BusinessObjects
{
    /// <summary>
    /// represents an external link
    /// </summary>
    public class RelatedExternalLink
    {
        /// <summary>
        /// the external link
        /// </summary>
        public String Url { get; set; }

        /// <summary>
        /// link text
        /// </summary>
        public String Text { get; set; }
    }
}
