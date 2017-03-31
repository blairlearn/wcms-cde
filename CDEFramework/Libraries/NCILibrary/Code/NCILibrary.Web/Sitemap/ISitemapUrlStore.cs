using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NCI.Web.Sitemap
{
    public interface ISitemapUrlStore
    {
        IEnumerable<SitemapUrl> GetSitemapUrls();
        Task<IEnumerable<SitemapUrl>> GetSitemapUrlsAsync();
    }
}