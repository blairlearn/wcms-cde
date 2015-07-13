using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace NCI.Services.Dictionary.BusinessObjects
{
    [DataContract()]
    public class Definition
    {
        /// <summary>
        /// The Term's definition in plain text (no markup).
        /// </summary>
        [DataMember(Name = "text")]
        public String Text { get; set; }

        /// <summary>
        /// The Term's definition in HTML format.
        /// </summary>
        [DataMember(Name = "html")]
        public String Html { get; set; }
    }
}
