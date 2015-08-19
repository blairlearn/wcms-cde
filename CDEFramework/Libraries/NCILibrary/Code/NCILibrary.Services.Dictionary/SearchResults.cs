using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

namespace NCI.Services.Dictionary
{
    internal class SearchResults
    {
        public SearchResults(DataTable data, int matchCount)
        {
            Data = data;
            MatchCount = matchCount;
        }

        public DataTable Data {get; private set;}
        public int MatchCount { get; private set; }
    }
}
