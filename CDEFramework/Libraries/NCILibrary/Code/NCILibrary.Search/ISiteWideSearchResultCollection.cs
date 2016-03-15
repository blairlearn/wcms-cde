using System;
using System.Collections.Generic;
using System.Text;
using System.Collections;

namespace NCI.Search
{
    public interface ISiteWideSearchResultCollection : IEnumerable<ISiteWideSearchResult>
    {
        long ResultCount { get; set; }        
              
    }
}
