using System.Runtime.Serialization;

namespace NCI.Web.Dictionary.BusinessObjects
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
