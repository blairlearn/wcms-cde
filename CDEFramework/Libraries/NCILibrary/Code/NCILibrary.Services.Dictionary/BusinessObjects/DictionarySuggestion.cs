using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace NCI.Services.Dictionary.BusinessObjects
{
    public class DictionarySuggestion
    {
        /// <summary>
        ///  The term's ID
        /// </summary>
        public String id { get; set; }

        /// <summary>
        /// The term's name
        /// </summary>
        public String term { get; set; }
    }
}
