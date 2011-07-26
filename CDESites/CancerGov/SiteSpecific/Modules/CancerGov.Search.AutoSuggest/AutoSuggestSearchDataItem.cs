using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CancerGov.Search.AutoSuggest
{
    /// <summary>
    /// Business layer that contains items specific the the AutoSuggestSearch query.
    /// This class can only be instantiated by passing all known values in the
    /// constructor.
    public class AutoSuggestSearchDataItem
    {
        public int    TermID { get { return termID; } }
        public string TermName { get { return termName; } }
        public string OLTermName { get { return olTermName; } }
        public string TermPronunciation { get { return termPronunciation; } }

        public List<AutoSuggestSearchDataItem> PreviousNeighbors
        {
            get { return previousNeighbors; }
        }
        public List<AutoSuggestSearchDataItem> NextNeighbors
        {
            get { return nextNeighbors; }
        }

        /// <summary>
        /// Constructor requires all data fields to be passed
        /// </summary>
        /// <param name="termID"></param>
        /// <param name="termName"></param>
        /// <param name="spanishTermName"></param>
        /// <param name="termPronunciation"></param>
        public AutoSuggestSearchDataItem(
            int termID,
            string termName,
            string olTermName,
            string termPronunciation
        )
        {
            this.termID = termID;
            this.termName = termName;
            this.olTermName = olTermName;
            this.termPronunciation = termPronunciation;
            previousNeighbors = new List<AutoSuggestSearchDataItem>();
            nextNeighbors = new List<AutoSuggestSearchDataItem>();
        }

        private int termID;
        private string termName;
        private string olTermName;
        private string termPronunciation;
        private List<AutoSuggestSearchDataItem> previousNeighbors;
        private List<AutoSuggestSearchDataItem> nextNeighbors;

    }
}
