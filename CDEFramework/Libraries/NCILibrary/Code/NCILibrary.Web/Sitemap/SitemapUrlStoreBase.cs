using System;
using System.Configuration.Provider;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NCI.Web.Sitemap
{
    public abstract class SitemapUrlStoreBase : ProviderBase, ISitemapUrlStore
    {
        public abstract SitemapUrlSet GetSitemapUrls();
    }
}