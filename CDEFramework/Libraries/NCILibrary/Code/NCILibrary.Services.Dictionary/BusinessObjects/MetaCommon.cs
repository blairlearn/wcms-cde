using System;
using System.Runtime.Serialization;

namespace NCI.Services.Dictionary.BusinessObjects
{
    /// <summary>
    /// Infrastructure.  Base class for all the return metadata classes.
    /// </summary>
    [DataContract()]
    abstract public class MetaCommon
    {
        /// <summary>
        /// Initialization.
        /// </summary>
        public MetaCommon()
        {
            // Set a default status message.
            Messages = new string[] { "OK" };
        }

        /// <summary>
        /// Human-readable message about the status of the call to GetTerm().
        /// May contain an error message.
        /// </summary>
        [DataMember(Name = "message")]
        public String[] Messages { get; set; }
    }
}
