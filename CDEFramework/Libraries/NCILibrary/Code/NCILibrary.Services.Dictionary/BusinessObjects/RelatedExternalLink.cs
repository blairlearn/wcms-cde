using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace NCI.Services.Dictionary.BusinessObjects
{
    /// <summary>
    /// represents an external link
    /// </summary>
    public class RelatedExternalLink
    {
        /// <summary>
        /// the external link
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
