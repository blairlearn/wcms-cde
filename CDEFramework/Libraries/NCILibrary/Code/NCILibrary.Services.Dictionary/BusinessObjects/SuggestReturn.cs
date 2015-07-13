using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace NCI.Services.Dictionary.BusinessObjects
{
    [DataContract()]
    public class SuggestReturn
    {
        public SuggestReturn()
        {
            // Force collection to be non-null.
            Result = new DictionarySuggestion[] { };
        }

        [DataMember(Name = "meta")]
        public SuggestReturnMeta Meta { get; set; }

        [DataMember(Name = "result")]
        public DictionarySuggestion[] Result { get; set; }
    }
}
