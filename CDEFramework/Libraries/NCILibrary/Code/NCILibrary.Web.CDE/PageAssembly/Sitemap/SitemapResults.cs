using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Common.Logging;

using NCI.Web.CDE.Configuration;
using NCI.Web.Sitemap;
using NCI.Logging;
using System.Threading.Tasks;

namespace NCI.Web.CDE.PageAssembly.Sitemap
{
    /// <summary>
    /// Class that represents the results of a sitemap load.
    /// </summary>
    internal class SitemapResults
    {
        private List<string> _errorMessages = new List<string>();
        private List<SitemapUrl> _sitemapUrls = new List<SitemapUrl>();
        private readonly bool _isAsync = false;

        /// <summary>
        /// Gets the total number of Errors occurred in this load.
        /// </summary>
        public int ErrorCount { get; set; }

        /// <summary>
        /// Gets the sitemap urls loaded
        /// </summary>
        public IEnumerable<SitemapUrl> SitemapUrls
        {
            get
            {
                return _sitemapUrls;
            }
        }

        /// <summary>
        /// Gets a collection of error messages that occurred during this load.
        /// </summary>
        public string[] ErrorMessages
        {
            get
            {
                return _errorMessages.ToArray();
            }
        }

        /// <summary>
        /// Creates a new instance of an internal sitemap results
        /// </summary>
        public SitemapResults(bool isAsync)
        {
            this.ErrorCount = 0;
            this._isAsync = isAsync;
        }

        /// <summary>
        /// Adds an error to the list of errors
        /// </summary>
        /// <param name="errorMessage">The error message</param>
        public void AddError(string errorMessage)
        {
            //Lock
            this.ErrorCount++;
            this._errorMessages.Add(errorMessage);
        }

        /// <summary>
        /// Adds a Sitemap Url to the results
        /// </summary>
        /// <param name="url"></param>
        public void AddSitemapUrl(SitemapUrl url)
        {
            //LOCK
            this._sitemapUrls.Add(url);
        }

    }
}
