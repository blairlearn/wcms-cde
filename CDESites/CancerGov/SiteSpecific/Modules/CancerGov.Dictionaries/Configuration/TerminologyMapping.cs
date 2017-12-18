using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Web;
using Common.Logging;

namespace CancerGov.Dictionaries.Configuration
{
    /// <summary>
    /// Class represents a singleton that can be used to lookup a CDRID for
    /// one user-friendly URL name.  This is used by the Dictionary pages.
    /// </summary>
    public class TerminologyMapping
    {
        // Lock synchronization object
        private static object syncLock = new Object();

        private Dictionary<string, string> _mapping = new Dictionary<string, string>();

        private TerminologyMapping(Dictionary<string, string> mapping)
        {
            _mapping = mapping;
        }

        public static TerminologyMapping GetMappingForFile(string dictMappingFilepath)
        {
            if (string.IsNullOrEmpty(dictMappingFilepath))
            {
                throw new ArgumentException("The dictionary mapping filepath must not be empty or null.");
            }

            if (HttpContext.Current.Cache[dictMappingFilepath] == null)
            {
                lock (syncLock)
                {
                    if (HttpContext.Current.Cache[dictMappingFilepath] == null)
                    {
                        TerminologyMapping instance = LoadDictionaryMappingFromFile(dictMappingFilepath);

                        // Store instance in cache for five minutes
                        HttpContext.Current.Cache.Add(dictMappingFilepath, instance, null, DateTime.Now.AddMinutes(5), System.Web.Caching.Cache.NoSlidingExpiration, System.Web.Caching.CacheItemPriority.High, null);
                    }
                }
            }

            return (TerminologyMapping)HttpContext.Current.Cache[dictMappingFilepath];
        }

        private static TerminologyMapping LoadDictionaryMappingFromFile(string filePath)
        {
            Dictionary<string, string> dict = new Dictionary<string, string>();

            string realPath = "";
            try
            {
                realPath = HttpContext.Current.Server.MapPath(filePath);
            }
            catch (Exception ex)
            {
                LogManager.GetLogger(typeof(TerminologyMapping)).ErrorFormat("Error while getting the mapping file.", ex);
            }

            try
            {
                // If file exists, use streamreader to load mappings into dictionary
                using (StreamReader sr = new StreamReader(HttpContext.Current.Server.MapPath(filePath)))
                {
                    string line;
                    while ((line = sr.ReadLine()) != null)
                    {
                        // Lowercase entry  (for comparison to pretty name from URL parameters later)
                        line = line.ToLower();


                        string[] parts = line.Split('|');

                        // Add mapping to dictionary if it isn't already present
                        if (!dict.ContainsKey(parts[0]))
                        {
                            dict.Add(parts[0], parts[1]);
                        }
                    }
                }
            }
            catch
            {
                // Throw exception if file doesn't exist
                LogManager.GetLogger(typeof(TerminologyMapping)).ErrorFormat("Mapping file '{0}' could not be read.", filePath);
                throw new FileNotFoundException(filePath);
            }

            return new TerminologyMapping(dict);
        }

        /// <summary>
        /// Gets the CDRID that maps to the given pretty name.
        /// </summary>
        /// <param name="value"></param>
        /// <returns>The CDRID</returns>
        public string GetFriendlyNameFromCDRID(string value)
        {
            value = value.ToLower();
            return _mapping[value];
        }

        /// <summary>
        /// Gets the CDRID that maps to the given pretty name.
        /// </summary>
        /// <param name="value"></param>
        /// <returns>The CDRID</returns>
        public string GetCDRIDFromFriendlyName(string value)
        {
            value = value.ToLower();
            return _mapping.FirstOrDefault(x => x.Value == value).Key;
        }

        /// <summary>
        /// Checks to see if the lookup contains an entry for the CDRID
        /// </summary>
        /// <param name="key">The CDRID to lookup</param>
        /// <returns>True or false based on the existance of the CDRID in the lookup</returns>
        public bool MappingContainsCDRID(string key)
        {
            key = key.ToLower();
            return _mapping.ContainsKey(key);
        }

        /// <summary>
        /// Checks to see if the lookup contains an entry for the pretty name
        /// </summary>
        /// <param name="value">The pretty name to lookup</param>
        /// <returns>True or false based on the existance of the pretty name in the lookup</returns>
        public bool MappingContainsFriendlyName(string value)
        {
            value = value.ToLower();
            string myKey = _mapping.FirstOrDefault(x => x.Value == value).Key;
            if (!string.IsNullOrEmpty(myKey))
            {
                return true;
            }

            return false;
        }
    }
}
