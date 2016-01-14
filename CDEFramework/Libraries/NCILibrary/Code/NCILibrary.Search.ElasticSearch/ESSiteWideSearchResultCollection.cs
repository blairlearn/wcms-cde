using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NCI.Search
{
    public class ESSiteWideSearchResultCollection : ISiteWideSearchResultCollection
    {
        private IEnumerable<ESSiteWideSearchResult> searchResults;

        public long ResultCount { get; set; }

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
