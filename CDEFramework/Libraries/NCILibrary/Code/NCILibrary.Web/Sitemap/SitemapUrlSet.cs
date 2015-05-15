using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.Xml.Serialization;

namespace NCI.Web.Sitemap
{
    [XmlType("urlset")]
    public class SitemapUrlSet : IEnumerable<SitemapUrl>
    {
        private List<SitemapUrl> _sitemapCollection = new List<SitemapUrl>();

        public SitemapUrlSet() { }

        public SitemapUrlSet(IEnumerable<SitemapUrl> set)
        {
            this._sitemapCollection.AddRange(set);
        }

        public IEnumerator<SitemapUrl> GetEnumerator()
        {
            return _sitemapCollection.GetEnumerator();
        }

        public void Add(SitemapUrlSet fromSet)
        {
            this._sitemapCollection.AddRange(fromSet._sitemapCollection);
        }

        public void Add(object o)
        {
            if (o is SitemapUrl)
            {
                this._sitemapCollection.Add((SitemapUrl)o);
            }
            else
            {
                throw new Exception();
            }
        }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            foreach (SitemapUrl s in this._sitemapCollection)
            {
                sb.Append(s.pageUrl);
                sb.Append("\n");
            }

            return sb.ToString();
        }

        #region IEnumerable Members

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return _sitemapCollection.GetEnumerator();
        }

        #endregion
    }
}