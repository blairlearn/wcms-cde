using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace NCI.Services.Dictionary
{
    /// <summary>
    /// Enumeration of the known dictionary types.
    /// </summary>
    [DataContract]
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

    [DataContract()]
    public enum Language
    {
        /// <summary>
        /// The language wasn't set to a recognized value.
        /// </summary>
        [EnumMember(Value = "unknown")]
        Unknown = 0,

        [EnumMember(Value = "en")]
        English = 1,

        [EnumMember(Value = "es")]
        Spanish = 2
    }

    /// <summary>
    /// Allowed search types
    /// </summary>
    [DataContract]
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
        Magic = 3,

        /// <summary>
        /// Search for terms exact match
        /// </summary>
        Exact = 4
    }

    [DataContract]
    public enum AudienceType
    {
        Unknown = 0,
        Patient = 1,
        HealthProfessional = 2
    }

    /// <summary>
    /// List of service methods
    /// </summary>
    internal enum ApiMethodType
    {
        Unknown = 0,
        GetTerm = 1,
        Search = 2,
        SearchSuggest = 3,
        Expand = 4
    }



}
