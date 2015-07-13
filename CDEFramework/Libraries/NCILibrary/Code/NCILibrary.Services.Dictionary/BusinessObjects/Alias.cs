using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace NCI.Services.Dictionary.BusinessObjects
{
    [DataContract()]
    public class Alias
    {
        /// <summary>
        /// The other name
        /// </summary>
        [DataMember(Name = "name")]
        public String Name { get; set; }

        /// <summary>
        /// The type other name
        /// </summary>
        [DataMember(Name = "type")]
        public String Type { get; set; }
    }
}
