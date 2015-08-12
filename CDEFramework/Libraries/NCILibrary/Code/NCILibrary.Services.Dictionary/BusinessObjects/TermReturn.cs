using System.Runtime.Serialization;
using System;
using System.IO;

namespace NCI.Services.Dictionary.BusinessObjects
{
    /// <summary>
    /// Defines the overall data structure for returns from an individual
    /// Term lookup.
    /// </summary>
    [DataContract()]
    public class TermReturn
    {
        [DataMember(Name = "meta")]
        public TermReturnMeta Meta { get; set; }

        [DataMember(Name = "term")]
        public String Term { get; set; }
    }
}
