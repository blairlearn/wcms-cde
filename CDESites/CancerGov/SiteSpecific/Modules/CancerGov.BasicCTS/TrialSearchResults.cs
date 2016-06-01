using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CancerGov.ClinicalTrials.Basic
{
    public class TrialSearchResults
    {

        public TrialSearchResult[] Results { get; private set; }
        public long TotalResults { get; private set; }

        public TrialSearchResults(long totalResults, IEnumerable<TrialSearchResult> results)
        {
            Results = results.ToArray();
            TotalResults = totalResults;
        }


    }
}
