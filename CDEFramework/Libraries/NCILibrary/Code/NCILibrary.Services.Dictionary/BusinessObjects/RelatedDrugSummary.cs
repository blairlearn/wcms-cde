using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace NCI.Services.Dictionary.BusinessObjects
{
    /// <summary>
    /// Represents a link to a Drug Info Summary
    /// </summary>
    public class RelatedDrugSummary
    {
        /// <summary>
        /// the summary link
        /// </summary>
        public String Url { get; set; }

        /// <summary>
        /// language to include the link with.
        /// </summary>
        public String Language { get; set; }

        /// <summary>
        /// link text
        /// </summary>
        public String Text { get; set; }
    }
}
