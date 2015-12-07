using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Runtime.Serialization;
using System.Text;

namespace NCI.Services.Dictionary.BusinessObjects
{
    /// <summary>
    /// Outermost data structure for returns from Expand().
    /// </summary>
    [DataContract()]
    public class SearchReturn : IJsonizable
    {
        public SearchReturn()
        {
            // Force collection to be non-null.
            Result = new DictionarySearchResultEntry[] { };
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
        public DictionarySearchResultEntry[] Result { get; set; }

        /// <summary>
        /// Hook for storing data members by calling the various AddMember() overloads.
        /// </summary>
        /// <param name="builder">The Jsonizer instance to use for storing data members.</param>
        public void Jsonize(Jsonizer builder)
        {
            builder.AddMember("meta", Meta, false);
            builder.AddMember("result", Result, true);
        }
    }
}
