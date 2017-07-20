using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NCILibrary.Search.Client
{
    public class SiteWideSearchResults
    {

        public SiteWideSearchResult[] Results { get; private set; }
        public long TotalResults { get; private set; }

        public SiteWideSearchResults(long totalResults, IEnumerable<SiteWideSearchResult> results)
        {
            Results = results.ToArray();
            TotalResults = totalResults;
        }


    }
}