using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Web;
using Common.Logging;
using CancerGov.ClinicalTrials.Basic.v2.Lookups;

namespace CancerGov.ClinicalTrials.Basic.v2
{
    /// <summary>
    /// Class represents a singleton that can be used to lookup a CDRID for
    /// one user-friendly URL name.  This is used by the Dictionary pages.
    /// </summary>
    public class DynamicTrialListingFriendlyNameMapping

    {
        // Lock synchronization object
        private static object syncLock = new Object();

        private Dictionary<string, MappingItem> _mapping = new Dictionary<string, MappingItem>();

        private DynamicTrialListingFriendlyNameMapping(Dictionary<string, MappingItem> mapping)
        {
            _mapping = mapping;
        }

        public static DynamicTrialListingFriendlyNameMapping GetMapping(string evsMappingFilepath, string overrideMappingFilepath, bool withOverrides)
        {
            if (string.IsNullOrEmpty(evsMappingFilepath) || string.IsNullOrEmpty(overrideMappingFilepath))
            {
                throw new ArgumentException("The dictionary mapping filepaths must not be empty or null.");
            }

            if (HttpContext.Current.Cache[evsMappingFilepath] == null)
            {
                lock (syncLock)
                {
                    if (HttpContext.Current.Cache[evsMappingFilepath] == null)
                    {
                        DynamicTrialListingFriendlyNameMapping instance = LoadDictionaryMappingFromFiles(evsMappingFilepath, overrideMappingFilepath, false);

                        // Store instance in cache for five minutes
                        HttpContext.Current.Cache.Add(evsMappingFilepath, instance, null, DateTime.Now.AddMinutes(5), System.Web.Caching.Cache.NoSlidingExpiration, System.Web.Caching.CacheItemPriority.High, null);
                    }
                }
            }

            if (HttpContext.Current.Cache[overrideMappingFilepath] == null)
            {
                lock (syncLock)
                {
                    if (HttpContext.Current.Cache[overrideMappingFilepath] == null)
                    {
                        DynamicTrialListingFriendlyNameMapping instance = LoadDictionaryMappingFromFiles(evsMappingFilepath, overrideMappingFilepath, true);

                        // Store instance in cache for five minutes
                        HttpContext.Current.Cache.Add(overrideMappingFilepath, instance, null, DateTime.Now.AddMinutes(5), System.Web.Caching.Cache.NoSlidingExpiration, System.Web.Caching.CacheItemPriority.High, null);
                    }
                }
            }

            return withOverrides ? (DynamicTrialListingFriendlyNameMapping)HttpContext.Current.Cache[overrideMappingFilepath] : (DynamicTrialListingFriendlyNameMapping)HttpContext.Current.Cache[evsMappingFilepath];
        }

        private static DynamicTrialListingFriendlyNameMapping LoadDictionaryMappingFromFiles(string evsFilePath, string overrideFilePath, bool withOverrides)
        {
            Dictionary<string, MappingItem> dict = new Dictionary<string, MappingItem>();

            bool fileExists = false;

            fileExists = File.Exists(HttpContext.Current.Server.MapPath(evsFilePath));

            if(fileExists)
            {
                try
                {
                    // If file exists, use streamreader to load EVS mappings into dictionary
                    using (StreamReader sr = new StreamReader(HttpContext.Current.Server.MapPath(evsFilePath)))
                    {
                        string line;
                        while ((line = sr.ReadLine()) != null)
                        {
                            line = line.ToLower();

                            string[] parts = line.Split('|');
                            // Lowercase c-codes (for comparison to codes from URL parameters later)
                            parts[0] = parts[0].ToLower();

                            // Sort c-codes alphabetically/numerically if there are multiple 
                            if (parts[0].Contains(","))
                            {
                                string[] split = parts[0].Split(',');
                                Array.Sort(split);
                                string newKey = string.Join(",", split);
                                parts[0] = newKey;
                            }

                            // Create MappingItem object for mapping
                            MappingItem item = new MappingItem();
                            item.Codes = parts[0].Split(',').ToList();
                            item.Text = parts[1];
                            item.IsOverride = false;

                            // Add mapping to dictionary if it isn't already present
                            if (!dict.ContainsKey(parts[0]))
                            {
                                dict.Add(parts[0], item);
                            }
                        }
                    }
                }
                catch
                {
                    // Log an error if the file exists but cannot be read.
                    // Do not make the page error out - we want the dictionaries to still work,
                    // even if there is something wrong with the friendly name mapping file.
                    LogManager.GetLogger(typeof(DynamicTrialListingFriendlyNameMapping)).ErrorFormat("Mapping file '{0}' could not be read.", evsFilePath);
                }
            }
            else
            {
                // Log an error if the file does not exist.
                // Do not make the page error out - we want the dictionaries to still work,
                // even if there is something wrong with the friendly name mapping file.
                LogManager.GetLogger(typeof(DynamicTrialListingFriendlyNameMapping)).ErrorFormat("Error while getting the mapping file located at '{0}'.", evsFilePath);
            }

            fileExists = File.Exists(HttpContext.Current.Server.MapPath(overrideFilePath));

            if (fileExists && withOverrides)
            {
                try
                {
                    // If file exists and we need overrides, use streamreader to load override mappings into dictionary
                    using (StreamReader sr = new StreamReader(HttpContext.Current.Server.MapPath(overrideFilePath)))
                    {
                        string line;
                        while ((line = sr.ReadLine()) != null)
                        {
                            line = line.ToLower();

                            string[] parts = line.Split('|');
                            // Lowercase c-codes (for comparison to codes from URL parameters later)
                            parts[0] = parts[0].ToLower();

                            // Sort c-codes alphabetically/numerically if there are multiple 
                            if (parts[0].Contains(","))
                            {
                                string[] split = parts[0].Split(',');
                                Array.Sort(split);
                                string newKey = string.Join(",", split);
                                parts[0] = newKey;
                            }

                            // Create MappingItem object for mapping
                            MappingItem item = new MappingItem();
                            item.Codes = parts[0].Split(',').ToList();
                            item.Text = parts[1];
                            item.IsOverride = true;
                            
                            if (!dict.ContainsKey(parts[0]))
                            {
                                // Add override mapping to dictionary if it isn't already present
                                dict.Add(parts[0], item);
                            }
                            else
                            {
                                // If mapping for key is already present, overwrite the entry with the override mapping
                                dict[parts[0]] = item;
                            }
                        }
                    }
                }
                catch
                {
                    // Log an error if the file exists but cannot be read.
                    // Do not make the page error out - we want the dictionaries to still work,
                    // even if there is something wrong with the friendly name mapping file.
                    LogManager.GetLogger(typeof(DynamicTrialListingFriendlyNameMapping)).ErrorFormat("Mapping file '{0}' could not be read.", overrideFilePath);
                }
            }
            else
            {
                // Log an error if the file does not exist.
                // Do not make the page error out - we want the dictionaries to still work,
                // even if there is something wrong with the friendly name mapping file.
                LogManager.GetLogger(typeof(DynamicTrialListingFriendlyNameMapping)).ErrorFormat("Error while getting the mapping file located at '{0}'.", overrideFilePath);
            }


            return new DynamicTrialListingFriendlyNameMapping(dict);
        }

        /// <summary>
        /// Gets the CDRID that maps to the given pretty name.
        /// </summary>
        /// <param name="value"></param>
        /// <returns>The CDRID</returns>
        public string GetFriendlyNameFromCode(string value)
        {
            value = value.ToLower();

            //if(_mapping.ContainsKey(value))
            //{
                // If an exact match for the given codes is found, return the exact match.
                return _mapping[value].Text;
            /*}
            else
            {
                // If no exact match for the given codes is found, remove all of the overrides from the mapping.
                foreach (var item in _mapping.Where(m => m.Value.IsOverride).ToList())
                {
                    _mapping.Remove(item.Key);
                }

                // Check this mapping for any keys that contain the given value(s).
                List<string> splitIDs = value.Split(new char[] { ',' }).ToList();
                List<string> splitIDFriendlyNames = new List<string>();

                foreach (string ID in splitIDs)
                {
                    if (_mapping.ContainsKey(ID))
                    {
                        splitIDFriendlyNames.Add(_mapping[ID].Text);
                    }
                    else
                    {

                    }
                }

                // If there is one match, return it.
                // If there are multiple matches, compare the strings (friendly names) for each match. If they are the same, return the first match.
            }*/
        }

        /// <summary>
        /// Gets the CDRID that maps to the given pretty name.
        /// </summary>
        /// <param name="value"></param>
        /// <returns>The CDRID</returns>
        public string GetCodeFromFriendlyName(string value)
        {
            value = value.ToLower();
            return _mapping.FirstOrDefault(x => x.Value.Text == value).Key;
        }

        /// <summary>
        /// Checks to see if the lookup contains an entry for the CDRID
        /// </summary>
        /// <param name="key">The CDRID to lookup</param>
        /// <returns>True or false based on the existance of the CDRID in the lookup</returns>
        public bool MappingContainsCode(string key)
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
            string myKey = _mapping.FirstOrDefault(x => x.Value.Text == value).Key;
            if (!string.IsNullOrEmpty(myKey))
            {
                return true;
            }

            return false;
        }
    }
}

