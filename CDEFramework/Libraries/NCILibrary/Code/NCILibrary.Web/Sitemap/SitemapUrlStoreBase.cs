using System;
using System.Configuration.Provider;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NCI.Web.Sitemap
{
    /// <summary>
    /// Base class for a SitemapUrl store for providing Sitemap.xml entries
    /// </summary>
    public abstract class SitemapUrlStoreBase : ProviderBase, ISitemapUrlStore
    {
        /// <summary>
        /// Gets a set of Sitemap Urls
        /// </summary>
        /// <remarks>For performance reasons you should implement this method with
        /// a yield return SitemapUrl in order to "stream" the SitemapUrls to the handler so
        /// it can start outputting them to the response as soon as possible.
        /// </remarks>
        /// <returns></returns>
        public abstract IEnumerable<SitemapUrl> GetSitemapUrls();
        
        /// <summary>
        /// Gets a set of sitemap Urls asynchronously
        /// </summary>
        /// <remarks>For performance reasons you should implement this method with
        /// a yield return SitemapUrl in order to "stream" the SitemapUrls to the handler so
        /// it can start outputting them to the response as soon as possible.
        /// </remarks>
        /// <returns></returns>
        public abstract Task<IEnumerable<SitemapUrl>> GetSitemapUrlsAsync();
    }
}