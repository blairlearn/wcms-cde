using System;
using System.Runtime.Serialization;

namespace NCI.Web.Dictionary.BusinessObjects
{
    public class Definition
    {
        /// <summary>
        /// The Term's definition in plain text (no markup).
        /// </summary>
        public String Text { get; set; }

        /// <summary>
        /// The Term's definition in HTML format.
        /// </summary>
        public String Html { get; set; }
    }
}
