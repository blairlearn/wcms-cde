using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NCI.Web.Sitemap
{
    public interface ISitemapUrlStore
    {
        SitemapUrlSet GetSitemapUrls();
    }
}