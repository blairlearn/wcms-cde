using System;
using System.Runtime.Serialization;

namespace NCI.Web.Dictionary.BusinessObjects
{
    /// <summary>
    /// Represents a link to a Drug Info Summary
    /// </summary>
    public class RelatedDrugSummary
    {
        /// <summary>
        /// the summary link
        /// </summary>
        public String url { get; set; }

        /// <summary>
        /// link text
        /// </summary>
        public String Text { get; set; }
    }
}
