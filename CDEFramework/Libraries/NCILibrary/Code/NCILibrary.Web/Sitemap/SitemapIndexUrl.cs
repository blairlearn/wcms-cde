using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Serialization;

namespace NCI.Web.Sitemap
{
    [XmlType("sitemap")]
    public class SitemapIndexUrl
    {
        internal SitemapIndexUrl() { }

        [XmlElement("loc")]
        public string pageUrl;

        [XmlElement("lastmod")]
        public DateTime lastMod;

        public SitemapIndexUrl(string url, DateTime mod)
        {
            pageUrl = url;
            lastMod = mod;
        }
    }
}
