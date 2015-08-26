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
        // We don't know what dictionary this is.  Error condition.
        Unknown = 0,

        // Dictionary of Cancer Terms
        term = 1,

        // Drug Dictionary
        drug = 2,

        // Dictionary of Genetics Terms
        genetic =3
    }

    [DataContract()]
    public enum Language
    {
        [EnumMember(Value = "unknown")]
        Unknown = 0,

        [EnumMember(Value = "en")]
        English = 1,

        [EnumMember(Value = "es")]
        Spanish = 2
    }

    [DataContract]
    public enum SearchType
    {
        Unknown = 0,
        Begins = 1,
        Contains = 2,
        Magic = 3
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
