using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace NCI.Services.Dictionary.BusinessObjects
{
    /// <summary>
    /// represents a single related Term
    /// </summary>
    public class RelatedTerm
    {

        /// <summary>
        /// related Term's ID
        /// </summary>
        public String Termid { get; set; }

        /// <summary>
        /// identifies thpublic Stringe dictionary {get;set;}
        /// </summary>
        public String Dictionary { get; set; }

        /// <summary>
        /// link text
        /// </summary>
        public String Text { get; set; }
    }
}
