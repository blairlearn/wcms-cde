using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NCI.Search
{
    /// <summary>
    /// This class is a collection of all the Search results
    /// </summary>
    public class ESSiteWideSearchResultCollection : ISiteWideSearchResultCollection
    {
        private IEnumerable<ESSiteWideSearchResult> searchResults;

        /// <summary>
        /// Number of results returned
        /// </summary>
        public long ResultCount { get; set; }

        /// <summary>
        /// List of all the Search results
        /// </summary>
        List<ESSiteWideSearchResult> SearchResults { get; set; }
        
        public List<ESSiteWideSearchResult> ESSearchCollection()
        {
            SearchResults = (List<ESSiteWideSearchResult>)searchResults;
            return SearchResults;
        }

        public ESSiteWideSearchResultCollection(IEnumerable<ESSiteWideSearchResult> results)
        {
            this.searchResults = results;
        }


        #region IEnumerable<ESSearchResult> Members
        /// <summary>
        /// Get Enumerator object
        /// </summary>
        /// <returns>Enumerable list</returns>


        public IEnumerator<ISiteWideSearchResult> GetEnumerator()
        {

            return (IEnumerator<ISiteWideSearchResult>)searchResults.GetEnumerator();
        }

        #endregion
               

        IEnumerator IEnumerable.GetEnumerator()
        {
            return ((IEnumerable)(searchResults)).GetEnumerator();
        }
    }

}
