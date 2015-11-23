using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using NCI.Search;

namespace NCI.Search
{
    public class ESSiteWideSearchResult : ISiteWideSearchResult
    {
        public string Title { get; set; }
        public string Url { get; set; }
        public string Description { get; set; }

        public string WrappedUrl { get; set; }

        public string ContentType { get; set; }

        public ESSiteWideSearchResult()
        {
        }

        public ESSiteWideSearchResult(string title, string url, string description)
        {
            this.Title = title;
            this.Url = url;
            this.Description = description;
        }
    }
}
