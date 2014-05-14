using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NCI.Search.Endeca;

namespace CancerGov.Modules.Search.Endeca
{
    public class SiteWideSearchResults : List<EndecaResult>
    {
        private long _totalNumResults = 0;
        private string _didYouMeanText = string.Empty;

        /// <summary>
        /// Gets the total number of results for the endeca search this collection represents.
        /// </summary>
        public long TotalNumResults
        {
            get { return _totalNumResults; }
        }        

        /// <summary>
        /// Gets and sets any Did you mean text.
        /// </summary>
        public string DidYouMeanText
        {
            get { return _didYouMeanText; }
            set { _didYouMeanText = value; }
        }

        public SiteWideSearchResults(long totalNumResults)
        {
            _totalNumResults = totalNumResults;
        }
    }
}
