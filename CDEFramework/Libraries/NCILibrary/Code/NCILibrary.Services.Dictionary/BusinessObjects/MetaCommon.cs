using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace NCI.Services.Dictionary.BusinessObjects
{
    /// <summary>
    /// Infrastructure.  Base class for all the return metadata classes.
    /// </summary>
    abstract public class MetaCommon
    {
        public MetaCommon()
        {
            // Set a default status message.
            Messages = new string[] { "OK" };
        }

        /// <summary>
        /// Human-readable message about the status of the call to GetTerm().
        /// May contain an error message.
        /// </summary>
        public String[] Messages { get; set; }
    }
}
