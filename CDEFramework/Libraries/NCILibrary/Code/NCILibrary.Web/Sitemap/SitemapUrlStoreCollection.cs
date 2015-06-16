using System;
using System.Collections.Generic;
using System.Configuration.Provider;
using System.Globalization;
using System.Runtime.CompilerServices;
using System.Linq;
using System.Text;

namespace NCI.Web.Sitemap
{
    /// <summary>
    /// Collection of SitemapUrlStores
    /// </summary>
    public class SitemapUrlStoreCollection : ProviderCollection
    {
        public override void Add(ProviderBase provider)
        {
            if (provider == null)
            {
                throw new ArgumentNullException("provider");
            }

            if (!(provider is SitemapUrlStoreBase))
            {
                throw new ArgumentException("UrlStore must be of type SitemapUrlStoreBase");
            }

            base.Add(provider);
        }

        new public SitemapUrlStoreBase this[string name]
        {
            get
            {
                return (SitemapUrlStoreBase)base[name];
            }
        }

        public void CopyTo(SitemapUrlStoreBase[] array, int index)
        {
            base.CopyTo(array, index);
        }

    }
}