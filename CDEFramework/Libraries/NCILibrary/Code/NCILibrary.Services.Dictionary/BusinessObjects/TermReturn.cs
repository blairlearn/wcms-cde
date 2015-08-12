using System.Runtime.Serialization;
using System;
using System.IO;

namespace NCI.Services.Dictionary.BusinessObjects
{
    /// <summary>
    /// Defines the overall data structure for returns from an individual
    /// Term lookup.
    /// </summary>
    [DataContract()]
    public class TermReturn
    {
        /// <summary>
        /// Metadata about the term being returned.
        /// </summary>
        [DataMember(Name = "meta")]
        public TermReturnMeta Meta { get; set; }

        /// <summary>
        /// String containg the JSON structure for a single term definition,
        /// as rendred by GateKeeper.
        /// </summary>
        /// <remarks>
        /// When the contents of Term are retrieved via a web service call, the default serialization
        /// renders value as a single string with leading and trailing quotation marks, causing it to be treated
        /// as an individual JSON values rather than a data structure.  Changing this behavior will likely
        /// require custom serialization and formatting.
        /// </remarks>
        [DataMember(Name = "term")]
        public String Term { get; set; }
    }
}
