using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;
using System.Collections.ObjectModel;
using NCI.DataManager;

namespace NCI.Web.CDE.Modules
{
    public class BlogSearchResult
    {

        public ICollection<SeriesPrevNextResult> Results { get; set; }
    
        public string SiteName { get; set; }

        public BlogSearchResult()
        {
            
            Results = null;
        }
    }
}
