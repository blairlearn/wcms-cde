using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace NCI.Web.Dictionary.BusinessObjects
{
    [DataContract()]
    public class ImageReference
    {
        [DataMember(Name = "filename")]
        public string Filename { get; set; }

        [DataMember(Name = "alt")]
        public string AltText { get; set; }

        [DataMember(Name = "caption")]
        public string Caption { get; set; }
    }
}
