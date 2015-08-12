using System;
using System.Runtime.Serialization;

namespace NCI.Services.Dictionary.BusinessObjects
{
    [DataContract()]
    public class SearchReturn
    {
        public SearchReturn()
        {
            // Force collection to be non-null.
            Result = new String[] { };
        }

        /// <summary>
        /// Metadata about the search results.
        /// </summary>
        [DataMember(Name = "meta")]
        public SearchReturnMeta Meta { get; set; }

        /// <summary>
        /// Array of strings containg the JSON structure for zero or more term definitions.
        /// </summary>
        /// <remarks>
        /// When the contents of Result are retrieved via a web service call, the default serialization
        /// renders the strings with leading and trailing quotation marks, causing them to appear as individual
        /// JSON values rather than as data structures.  Changing this behavior will likely require custom
        /// serialization and formatting.
        /// </remarks>
        [DataMember(Name = "result")]
        public String[] Result { get; set; }
    }
}
