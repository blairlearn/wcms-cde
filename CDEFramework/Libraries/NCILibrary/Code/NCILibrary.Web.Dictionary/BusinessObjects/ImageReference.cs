using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace NCI.Web.Dictionary.BusinessObjects
{
    //This particular class needs Data Contract since the data from the database has different names than these
    [DataContract()]
    public class ImageReference
    {
        [DataMember(Name = "ref")]
        public string Filename { get; set; }

        [DataMember(Name = "alt")]
        public string AltText { get; set; }

        [DataMember(Name = "caption")]
        public string Caption { get; set; }
    }
}