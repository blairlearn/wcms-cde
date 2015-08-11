using System;
using System.Runtime.Serialization;

namespace NCI.Services.Dictionary.BusinessObjects
{
    /// <summary>
    /// Describes how a dictionary Term is pronounced.
    /// </summary>
    [DataContract()]
    public class Pronunciation
    {
        public Pronunciation()
        {
            // Guarantee non-null values.
            Key = string.Empty;
            Audio = string.Empty;
        }

        /// <summary>
        /// Pronunciation key.  Possibly empty.
        /// </summary>
        [DataMember( Name = "key" )]
        public String Key { get; set; }

        /// <summary>
        /// Possibly empty URL of an audio file containing the pronunciation. (Cancer Term and Genetics only)
        /// </summary>
        [DataMember(Name = "audio")]
        public String Audio { get; set; }
    }
}
