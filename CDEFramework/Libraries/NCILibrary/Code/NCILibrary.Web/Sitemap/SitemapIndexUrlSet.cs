using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Serialization;

namespace NCI.Web.Sitemap
{
    [XmlRoot("sitemapindex")]
    public class SitemapIndexUrlSet : IEnumerable<SitemapIndexUrl>
    {
        private List<SitemapIndexUrl> _sitemapCollection = new List<SitemapIndexUrl>();

        public SitemapIndexUrlSet() { }

        public SitemapIndexUrlSet(IEnumerable<SitemapIndexUrl> set)
        {
            this._sitemapCollection.AddRange(set);
        }

        public IEnumerator<SitemapIndexUrl> GetEnumerator()
        {
            return _sitemapCollection.GetEnumerator();
        }

        public void Add(SitemapIndexUrlSet fromSet)
        {
            this._sitemapCollection.AddRange(fromSet._sitemapCollection);
        }

        public void Add(object o)
        {
            if (o is SitemapIndexUrl)
            {
                this._sitemapCollection.Add((SitemapIndexUrl)o);
            }
            else
            {
                throw new Exception();
            }
        }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            foreach (SitemapIndexUrl s in this._sitemapCollection)
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
