using System;
using System.Data;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace NCI.Services.Dictionary
{
    /// <summary>
    /// Lightweight helper class to encapuslate return of search data and metadata
    /// in a single object. (i.e. No need for an out parameter on the search
    /// query layer.)
    /// </summary>
    internal class SuggestionResults
    {
        public SuggestionResults(DataTable data, int matchCount)
        {
            Data = data;
            MatchCount = matchCount;
        }

        public DataTable Data {get; private set;}
        public int MatchCount { get; private set; }
    }
}
