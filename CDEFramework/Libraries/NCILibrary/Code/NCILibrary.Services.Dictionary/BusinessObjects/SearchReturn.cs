using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace NCI.Services.Dictionary.BusinessObjects
{
    [DataContract()]
    public class SearchReturn
    {
        public SearchReturn()
        {
            // Force collection to be non-null.
            Result = new DictionaryTerm[] { };
        }

        [DataMember(Name = "meta")]
        public SearchReturnMeta Meta { get; set; }

        [DataMember(Name = "result")]
        public DictionaryTerm[] Result { get; set; }
    }
}
