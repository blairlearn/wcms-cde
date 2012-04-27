using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CancerGov.Modules.Search.Endeca
{
    public class BestBetListItem
    {
        private string _url = string.Empty;
        private string _title = string.Empty;
        private string _description = string.Empty;

        /// <summary>
        /// Gets the URL of this list item
        /// </summary>
        public string Url
        {
            get { return _url; }
        }

        /// <summary>
        /// Gets the title of this list item
        /// </summary>
        public string Title
        {
            get { return _title; }
        }

        /// <summary>
        /// Gets the description of this list item
        /// </summary>
        public string Description
        {
            get { return _description; }
        }

        public BestBetListItem(string url, string title, string description)
        {
            _url = url;
            _title = title;
            _description = description;
        }

    }
}
