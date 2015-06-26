using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace NCI.Services.Dictionary.BusinessObjects
{
    /// <summary>
    /// Describes how a dictionary term is pronounced.
    /// </summary>
    public class Pronunciation
    {
        /// <summary>
        /// Pronunciation key.  Possibly empty.
        /// </summary>
        public String key { get; set; }

        /// <summary>
        /// Possibly empty URL of an audio file containing the pronunciation. (Cancer Term and Genetics only)
        /// </summary>
        public String audio { get; set; }
    }
}
