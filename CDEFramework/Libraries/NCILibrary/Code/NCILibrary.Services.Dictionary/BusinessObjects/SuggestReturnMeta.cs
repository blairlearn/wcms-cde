using System.Runtime.Serialization;

namespace NCI.Services.Dictionary.BusinessObjects
{
    [DataContract()]
    public class SuggestReturnMeta : MetaCommon, IJsonizable
    {
        /// <summary>
        /// The total number of terms matching the request
        /// </summary>
        [DataMember(Name = "result_count")]
        public int ResultCount { get; set; }


        public void Jsonize(Jsonizer builder)
        {
            builder.AddMember("result_count", ResultCount, true);
        }
    }
}
