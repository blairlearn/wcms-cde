using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NCI.Web.Dictionary
{
    /// <summary>
    /// Enumeration of the known dictionary types.
    /// </summary>
    public enum DictionaryType
    {
        /// <summary>
        /// We don't know what dictionary this is.  Error condition.
        /// </summary>
        Unknown = 0,

        /// <summary>
        /// The source document didn't specify a dictionary.
        /// Distinct from "A dictioary was assigned, but we don't know what it is."
        /// </summary>
        NotSet = 1,

        /// <summary>
        /// Dictionary of Cancer Terms.
        /// </summary>
        term = 2,

        /// <summary>
        /// Drug dictionary.
        /// </summary>
        drug = 3,

        /// <summary>
        /// Genetics Dictionary
        /// </summary>
        genetic = 4
    }

    /// <summary>
    /// Allowed search types
    /// </summary>
    public enum SearchType
    {
        Unknown = 0,

        /// <summary>
        /// Search for terms beginning with
        /// </summary>
        Begins = 1,

        /// <summary>
        /// Search for terms containing
        /// </summary>
        Contains = 2,

        /// <summary>
        /// Search for terms beginning with, followed by terms containing
        /// </summary>
        Magic = 3
    }

    public enum AudienceType
    {
        Unknown = 0,
        Patient = 1,
        HealthProfessional = 2
    }
}
