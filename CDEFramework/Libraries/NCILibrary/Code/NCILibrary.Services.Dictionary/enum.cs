using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace NCI.Services.Dictionary
{
    /// <summary>
    /// Enumeration of the known dictionary types.
    /// </summary>
    internal enum DictionaryType
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

    internal enum Language
    {
        Unknown = 0,
        English = 1,
        Spanish = 2
    }

    internal enum SearchType
    {
        Unknown = 0,
        Begins = 1,
        Contains = 2,
        Magic = 3
    }
}
