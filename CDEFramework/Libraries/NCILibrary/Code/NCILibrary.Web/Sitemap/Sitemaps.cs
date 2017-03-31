using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NCI.Web.Sitemap
{
    public static class Sitemaps
    {
        private static SitemapUrlStoreCollection s_Stores;
        private static object s_lock = new object();
        private static bool s_Initialized = false;
        private static Exception s_InitializeException = null;

        public static SitemapUrlStoreCollection Stores { get { Initialize(); return s_Stores; } }

        /// <summary>
        /// Gets a set of Sitemap Urls
        /// </summary>
        /// <returns></returns>
        public static IEnumerable<SitemapUrl> GetSitemap()
        {
            Initialize();

            //Loop over each store
            foreach (SitemapUrlStoreBase S in s_Stores)
            {
                //Loop over each URL in the store.
                foreach (SitemapUrl url in S.GetSitemapUrls())
                {
                    yield return url;
                }                
            }
        }

        /// <summary>
        /// Gets a set of sitemap Urls asynchronously
        /// </summary>
        /// <returns></returns>
        public static async Task<IEnumerable<SitemapUrl>> GetSitemapAsync()
        {
            Initialize();

            SitemapUrlSet sitemapSet = new SitemapUrlSet();

            foreach (SitemapUrlStoreBase S in s_Stores)
            {
                sitemapSet.Add(S.GetSitemapUrlsAsync());
            }

            return sitemapSet;
        }


        public static void Initialize()
        {
            // If it's already initalized, just return
            if (s_Initialized)
            {
                return;
            }

            // If we previously tried to initialize and it failed, throw the same exception
            if (s_InitializeException != null)
                throw s_InitializeException;

            lock (s_lock)
            {
                // A competing thread may have already initialized while waiting for the lock
                if (s_Initialized)
                {
                    return;
                }
                if (s_InitializeException != null)
                    throw s_InitializeException;

                try
                {
                    LoadStore();
                }
                catch (Exception e)
                {
                    s_InitializeException = e;
                    throw;
                }

                // update this state only after the whole method completes to preserve the behavior where
                // the system is uninitialized if any exceptions were thrown.
                s_Initialized = true;
            }
        }

        public static void LoadStore()
        {
            s_Stores = new SitemapUrlStoreCollection();
            SitemapProviderConfiguration config = (SitemapProviderConfiguration)ConfigurationManager.GetSection("Sitemap");
            Type providerType = typeof(SitemapUrlStoreBase);

            // Only use the first Provider.
            foreach (ProviderSettings settings in config.SitemapStores)
            {
                Type settingsType = Type.GetType(settings.Type, true, true);

                if (settingsType == null)
                    throw new ConfigurationErrorsException(String.Format("Could not find type: {0}", settings.Type));
                if (!providerType.IsAssignableFrom(settingsType))
                    throw new ConfigurationErrorsException(String.Format("SitemapStore '{0}' must subclass from '{1}'", settings.Name, providerType));

                SitemapUrlStoreBase store = Activator.CreateInstance(settingsType) as SitemapUrlStoreBase;

                if (store != null)
                {
                    store.Initialize(settings.Name, settings.Parameters);
                }

                s_Stores.Add(store);
            }

            if (s_Stores.Count == 0)
                throw new ConfigurationErrorsException(string.Format("No SitemapUrlStoreBase found"));
        }
    }
}