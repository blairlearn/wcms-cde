using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Runtime.Serialization;

namespace NCI.Services.Dictionary.BusinessObjects
{
    /// <summary>
    /// Outermost data structure for returns from Expand().
    /// </summary>
    [DataContract()]
    public class SearchReturn
    {
        public SearchReturn()
        {
            // Force collection to be non-null.
            Result = new DictionaryExpansion[] { };
        }

        /// <summary>
        /// Metadata about the expansion search results.
        /// </summary>
        [DataMember(Name = "meta")]
        public SearchReturnMeta Meta { get; set; }

        /// <summary>
        /// Array of objects containg details of the individual terms which met the search criteria.
        /// </summary>
        [DataMember(Name = "result")]
        public DictionaryExpansion[] Result { get; set; }
    }
}
