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

    [DataContract]
    public enum Language
    {
        Unknown = 0,
        English = 1,
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
}
