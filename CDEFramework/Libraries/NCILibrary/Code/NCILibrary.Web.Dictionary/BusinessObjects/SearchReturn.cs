using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Runtime.Serialization;

namespace NCI.Web.Dictionary.BusinessObjects
{
    /// <summary>
    /// Outermost data structure for returns from Expand().
    /// </summary>
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
        public SearchReturnMeta Meta { get; set; }

        /// <summary>
        /// Array of DictionaryExpansion objects containg details of the individual terms which met the search criteria.
        /// </summary>
        public DictionaryExpansion[] Result { get; set; }
    }
}
