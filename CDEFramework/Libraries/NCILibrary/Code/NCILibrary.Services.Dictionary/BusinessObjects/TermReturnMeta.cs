using System;
using System.Runtime.Serialization;

namespace NCI.Services.Dictionary.BusinessObjects
{
    /// <summary>
    /// Metadata about a call to GetTerm().
    /// </summary>
    [DataContract()]
    public class TermReturnMeta : MetaCommon, IJsonizable
    {
        /// <summary>
        /// The Term's audience Patient or HealthProfessional
        /// </summary>
        [DataMember(Name = "audience")]
        public String Audience { get; set; }

        /// <summary>
        /// The Term's language
        /// </summary>
        [DataMember(Name = "language")]
        public String Language { get; set; }


        public void Jsonize(Jsonizer builder)
        {
            builder.AddMember("audience", Audience, false);
            builder.AddMember("language", Language, true);
        }
    }
}
