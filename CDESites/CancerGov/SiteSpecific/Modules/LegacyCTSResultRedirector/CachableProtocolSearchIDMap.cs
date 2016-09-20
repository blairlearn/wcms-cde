using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Web;
using System.Web.Caching;
using Common.Logging;

namespace CancerGov.HttpModules
{
    /// <summary>
    /// Maps a collection of old Protocol Search IDs to new Protocol Search IDs. All lookups ignore case as well as
    /// any leading or trailing spaces.
    /// </summary>
    internal class CachableProtocolSearchIDMap
    {
        static ILog log = LogManager.GetLogger(typeof(CachableProtocolSearchIDMap));
        static string LEGACY_CTS_REDIRECTION_MAP_CACHE_KEY = "LEGACY_CTS_REDIRECTION_MAP_CACHE_KEY";        

        /// <summary>
        /// Returns a RedirectionMap created from the specified datafile.  If avaialble, a cached
        /// copy of the map is returned; otherwise, the data file is parsed and a new map created.
        /// </summary>
        /// <returns>A (possibly empty) RedirectionMap.</returns>
        public static CachableProtocolSearchIDMap GetMap()
        {
            log.Trace("Enter CachableProtocolSearchIDMap.GetMap().");

            CachableProtocolSearchIDMap map;
            Cache cache = HttpContext.Current.Cache;

            try
            {
                log.Debug("Load cache for legacy CTS redirect map.");


                //Try and get the map from the cache
                map = (CachableProtocolSearchIDMap)cache[LEGACY_CTS_REDIRECTION_MAP_CACHE_KEY];

                if (map == null)
                {
                    lock (LEGACY_CTS_REDIRECTION_MAP_CACHE_KEY)
                    {
                        // Check whether the cache was loaded while we waited for the lock.
                        map = (CachableProtocolSearchIDMap)cache[LEGACY_CTS_REDIRECTION_MAP_CACHE_KEY];

                        if (map == null)
                        {
                            //Actually try and load the file
                            // There was no cached redirection map.  Load it from the file system.
                            log.Debug("Cache miss. Loading Legacy CTS redirection map");

                            //Now load and store the map file.
                            map = LoadAndStoreMapFile();

                        }
                        else
                        {
                            log.Debug("Cached redirection map found on second chance retrieval.");
                        }
                    }
                }
                else
                {
                    // A cached redirection map was found. Return it.
                    log.Debug("Loading cached redirection map.");
                }
            }
            catch (Exception ex)
            {
                log.Error("Error while getting the redirection map.", ex);

                // Instead of letting the request die,
                // swallow the exception and return an empty map.
                map = new CachableProtocolSearchIDMap();
            }

            return map;
        }

        /// <summary>
        /// Stores an empty map in the cache in the event it did not exist or could not be loaded.  
        /// this way we do not keep trying to load this file every request.
        /// </summary>
        /// <returns>An empty CachableProtocolSearchIDMap</returns>
        private static CachableProtocolSearchIDMap StoreEmptyFile()
        {
            Cache cache = HttpContext.Current.Cache;
            CachableProtocolSearchIDMap map = new CachableProtocolSearchIDMap();

            //Let's make it every 10 min to check for new cache.
            cache.Add(LEGACY_CTS_REDIRECTION_MAP_CACHE_KEY, map, null, DateTime.Now.AddMinutes(10), Cache.NoSlidingExpiration, CacheItemPriority.AboveNormal, null);

            return map;
        }

        /// <summary>
        /// Attempts to Load the map file and store it in the cache if it was successfully loaded.
        /// </summary>
        /// <returns></returns>
        private static CachableProtocolSearchIDMap LoadAndStoreMapFile()
        {
            Cache cache = HttpContext.Current.Cache;
            String datafile = ConfigurationManager.AppSettings["LegacyCTSResultRedirectMap"];

            if (string.IsNullOrEmpty(datafile))
            {
                log.Warn("LegacyCTSResultRedirectMap AppSetting is not set");
                return StoreEmptyFile(); //Return empty map
            }

            string fullPath = string.Empty;

            try
            {

                //The path should be something like ~/PublishedContent/adasd, which needs to be
                //mapped to a full file path.
                fullPath = HttpContext.Current.Server.MapPath(datafile);

                CacheItemRemovedCallback onRemove = new CacheItemRemovedCallback(RemovedItemCallback);
                CacheDependency fileDependency = new CacheDependency(fullPath);

                CachableProtocolSearchIDMap map = LoadMapFromFile(fullPath);
                cache.Add(LEGACY_CTS_REDIRECTION_MAP_CACHE_KEY, map, fileDependency, Cache.NoAbsoluteExpiration, Cache.NoSlidingExpiration, CacheItemPriority.AboveNormal, onRemove);

                return map;
            }
            catch (Exception ex)
            {
                //Log Warning that we could not load item
                log.WarnFormat("Error loading LegacyCTSResultRedirectMap, {0}", ex, datafile);

                return StoreEmptyFile(); //Return empty map.
            }

        }



        /// <summary>
        /// Private helper method to respond when a cached item is removed.
        /// </summary>
        /// <param name="key">The cache key for the item.</param>
        /// <param name="item">The actual object which is being dropped from the cache.</param>
        /// <param name="reason">Enum explaining why the item was removed.</param>
        private static void RemovedItemCallback(String key, Object item, CacheItemRemovedReason reason)
        {
            log.TraceFormat("'{0}' removed from cache because '{1}'.", key, reason);
        }

        /// <summary>
        /// Private helper method to encapsulate the logic for loading and parsing the datafile.
        /// </summary>
        /// <param name="fullPath">Location of a data file creating comma-separated pairs of
        /// old URLs and new URLs for use as redirection targets.</param>
        /// <returns></returns>
        private static CachableProtocolSearchIDMap LoadMapFromFile(String fullPath)
        {
            log.Trace("Enter LoadMapFromFile().");

            char[] separators = new char[1];
            separators[0] = ',';

            CachableProtocolSearchIDMap map = new CachableProtocolSearchIDMap();

            //Removing Try/Catch as the wrapping class should handle any errors.

            if (!File.Exists(fullPath))
            {
                log.ErrorFormat("LegacyCTSResultRedirectMap file full path '{0}' not found.", fullPath);
                throw new FileNotFoundException(fullPath);
            }

            String[] listOfIDPairs = File.ReadAllLines(fullPath);
            foreach (String idPair in listOfIDPairs)
            {
                String[] ids = idPair.Trim().Split(separators);
                if (ids.Length >= 2)
                    map.Add(ids[0], ids[1]);
                if (ids.Length != 2)
                {
                    // We can recover from this problem. No exception needed.
                    log.WarnFormat("Expected only two protocol search IDs, found {0} in '{1}'.", ids.Length, idPair);
                }
            }
            
            return map;
        }


        // The RedirectionMap is mainly a thin wrapper around Dictionary<string, string>.
        // These are the Dictionary methods we expose.
        private Dictionary<string, string> map;

        public CachableProtocolSearchIDMap()
        {
            map = new Dictionary<string, string>();
        }

        public void Add(String oldProtocolSearchID, String newProtocolSearchID)
        {
            oldProtocolSearchID = oldProtocolSearchID.Trim().ToLowerInvariant();
            newProtocolSearchID = newProtocolSearchID.Trim().ToLowerInvariant();
            map.Add(oldProtocolSearchID, newProtocolSearchID);
        }

        public bool Contains(String protocolSearchID)
        {
            protocolSearchID = protocolSearchID.Trim().ToLowerInvariant();
            return map.ContainsKey(protocolSearchID);
        }

        public String this[String protocolSearchID]
        {
            get
            {
                protocolSearchID = protocolSearchID.Trim().ToLowerInvariant();
                return map[protocolSearchID];
            }
        }
    }
}
