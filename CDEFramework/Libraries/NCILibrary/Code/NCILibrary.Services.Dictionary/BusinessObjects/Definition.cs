using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace NCI.Services.Dictionary.BusinessObjects
{
    public class Definition
    {
        /// <summary>
        /// The term's definition in plain text (no markup).
        /// </summary>
        public String Text { get; set; }

        /// <summary>
        /// The term's definition in HTML format.
        /// </summary>
        public String Html { get; set; }
    }
}
