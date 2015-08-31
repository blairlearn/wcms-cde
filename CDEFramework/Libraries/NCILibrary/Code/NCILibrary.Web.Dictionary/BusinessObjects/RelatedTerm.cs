using System;
using System.Runtime.Serialization;

namespace NCI.Web.Dictionary.BusinessObjects
{
    /// <summary>
    /// represents a single related Term
    /// </summary>
    public class RelatedTerm
    {

        /// <summary>
        /// related Term's ID
        /// </summary>
        public int Termid { get; set; }

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
