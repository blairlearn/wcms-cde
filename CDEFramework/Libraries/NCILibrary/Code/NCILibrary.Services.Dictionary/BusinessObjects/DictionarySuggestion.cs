using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace NCI.Services.Dictionary.BusinessObjects
{
    [DataContract()]
    public class DictionarySuggestion
    {
        /// <summary>
        ///  The Term's ID
        /// </summary>
        [DataMember(Name = "id")]
        public String ID { get; set; }

        /// <summary>
        /// The Term's name
        /// </summary>
        [DataMember(Name = "term")]
        public String Term { get; set; }
    }
}
