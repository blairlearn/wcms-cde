using System;
using System.Web;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Xml;
using Common.Logging;

namespace NCI.Web.Sitemap
{
    public static class Sitemaps
    {
        private static SitemapIndexProviderConfiguration s_Config;
        private static SitemapUrlStoreCollection s_Stores;
        private static object s_lock = new object();
        private static bool s_Initialized = false;
        private static Exception s_InitializeException = null;

        public static SitemapUrlStoreCollection Stores { get { Initialize(); return s_Stores; } }
        static ILog log = LogManager.GetLogger(typeof(SitemapIndexHandler));

        public static SitemapUrlSet GetSitemap(string sitemapName)
        {
            Initialize();

            SitemapUrlSet sitemapSet = new SitemapUrlSet();

            SitemapUrlStoreBase S = s_Stores[sitemapName.ToLower()];
            sitemapSet.Add(S.GetSitemapUrls(S.Name));

            return sitemapSet;
        }

        public static SitemapIndexUrlSet GetSitemapIndex()
        {
            List<SitemapIndexUrl> sitemapIndexUrls = new List<SitemapIndexUrl>();
            String path;
            SitemapIndexSection section = (SitemapIndexSection)ConfigurationManager.GetSection("SitemapIndex");
            SitemapIndexProviderConfiguration config = section.Sitemaps;

            // Find all Sitemap elements within the Sitemaps collection and add their name (which is their URL) to the SitemapIndex
            foreach (SitemapProviderConfiguration element in config)
            {
                try
                {
                    path = ConfigurationManager.AppSettings["RootUrl"] + "/sitemaps/" + element.Name.ToLower();
                    sitemapIndexUrls.Add(new SitemapIndexUrl(path, DateTime.Now));
                }

                // If the sitemap index URL for this element can't be added, skip to the next element in the config.
                catch (XmlException ex)
                {
                    log.Error("A sitemap index URL has failed parsing in Sitemaps:GetSitemapIndex().\nFile: " + element.Name + "\nEnvironment: " + System.Environment.MachineName + "\nRequest Host: " + HttpContext.Current.Request.Url.Host + "\n" + ex.ToString() + "\n");
                    continue;
                }
            }

            return new SitemapIndexUrlSet(sitemapIndexUrls);
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
            SitemapIndexSection section = (SitemapIndexSection)ConfigurationManager.GetSection("SitemapIndex");
            SitemapIndexProviderConfiguration s_Config = section.Sitemaps;
            s_Stores = new SitemapUrlStoreCollection();
 
            Type providerType = typeof(SitemapUrlStoreBase);

            foreach (SitemapProviderConfiguration element in s_Config)
            {
                // Only use the first Provider.
                foreach (ProviderSettings settings in element.SitemapStores)
                {
                    Type settingsType = Type.GetType(settings.Type, true, true);

                    if (settingsType == null)
                        throw new ConfigurationErrorsException(String.Format("Could not find type: {0}", settings.Type));
                    if (!providerType.IsAssignableFrom(settingsType))
                        throw new ConfigurationErrorsException(String.Format("SitemapStore '{0}' must subclass from '{1}'", settings.Name, providerType));

                    SitemapUrlStoreBase store = Activator.CreateInstance(settingsType) as SitemapUrlStoreBase;

                    if (store != null)
                    {
                        store.Initialize(settings.Name.ToLower(), settings.Parameters);
                    }

                    s_Stores.Add(store);
                }
            }

            if (s_Stores.Count == 0)
                throw new ConfigurationErrorsException(string.Format("No SitemapUrlStoreBase found"));
        }
    }
}