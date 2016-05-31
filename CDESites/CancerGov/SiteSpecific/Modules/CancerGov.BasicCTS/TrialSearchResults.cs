using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CancerGov.ClinicalTrials.Basic
{
    public class TrialSearchResults : IEnumerable<TrialSearchResult>
    {
        private List<TrialSearchResult> _results = new List<TrialSearchResult>();

        #region IEnumerable<TrialSearchResult> Members

        public IEnumerator<TrialSearchResult> GetEnumerator()
        {
            return _results.GetEnumerator();
        }

        #endregion

        #region IEnumerable Members

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return _results.GetEnumerator();
        }

        #endregion


    }
}
