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

        [DataMember(Name = "meta")]
        public SearchReturnMeta Meta { get; set; }

        [DataMember(Name = "result")]
        public String[] Result { get; set; }
    }
}
