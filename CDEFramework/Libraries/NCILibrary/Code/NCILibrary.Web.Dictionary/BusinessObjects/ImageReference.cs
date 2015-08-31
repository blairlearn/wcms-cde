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
        public string Filename { get; set; }

        public string AltText { get; set; }

        public string Caption { get; set; }
    }
}
