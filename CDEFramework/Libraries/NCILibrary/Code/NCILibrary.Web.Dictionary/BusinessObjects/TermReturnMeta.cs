using System;
using System.Runtime.Serialization;

namespace NCI.Web.Dictionary.BusinessObjects
{
    /// <summary>
    /// Metadata about a call to GetTerm().
    /// </summary>
    public class TermReturnMeta : MetaCommon
    {
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
