using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace NCI.Web.Sitemap
{
    public enum sitemapChangeFreq { always, hourly, daily, weekly, monthly, yearly, never };

    [XmlRoot("url")]
    [XmlType("url")]
    public class SitemapUrl
    {
        internal SitemapUrl()
        {
        }

        [XmlElement("pageurl")]
        public string pageUrl;

        [XmlIgnore]
        public DateTime lastMod;

        [XmlElement("lastmod")]
        public string lastModXml
        {
            get
            {
                return lastMod.ToShortDateString();
            }
            set { }
        }

        [XmlIgnore]
        public sitemapChangeFreq changeFreq;

        [XmlElement("changefreq")]
        public string changeFreqXml
        {
            get
            {
                return changeFreq.ToString();
            }
            set { }
        }

        [XmlElement("priority")]
        public double priority;

        public SitemapUrl(string url, DateTime mod, sitemapChangeFreq freq, double pri)
        {
            pageUrl = url;
            lastMod = mod;
            changeFreq = freq;
            priority = pri;
        }
    }
}