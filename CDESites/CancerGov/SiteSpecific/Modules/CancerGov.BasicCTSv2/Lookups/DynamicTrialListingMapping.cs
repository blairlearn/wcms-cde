using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Web;
using CancerGov.ClinicalTrials.Basic.v2.Configuration;
using CancerGov.ClinicalTrials.Basic.v2.Lookups;
using Common.Logging;

namespace CancerGov.ClinicalTrials.Basic.v2
{
    /// <summary>
    /// Class represents a singleton that can be used to lookup a display label for
    /// one or more NCI Thesaurus codes.  This is used by the DynamicTrialListing pages
    /// and allows for overrides and other special rules.
    /// </summary>
    public class DynamicTrialListingMapping : ITerminologyLookupService
    {   
        // Lock synchronization object
        private static object syncLock = new Object();

        private static readonly string MAP_CACHE_KEY = "LabelMapping";
        private static readonly string EVS_MAPPING_FILE = BasicClinicalTrialSearchAPISection.GetEvsMappingFilePath();
        private static readonly string OVERRIDE_MAPPING_FILE = BasicClinicalTrialSearchAPISection.GetOverrideMappingFilePath();
        private static readonly string TOKENS_MAPPING_FILE = BasicClinicalTrialSearchAPISection.GetTokenMappingFilePath();
        private static readonly string STAGES_MAPPING_FILE = BasicClinicalTrialSearchAPISection.GetStagesMappingFilePath();
        private Dictionary<string, MappingItem> Mappings = new Dictionary<string, MappingItem>();
        private HashSet<string> Tokens = new HashSet<string>();
       
        private DynamicTrialListingMapping() { }

        /// <summary>
        /// Gets an instance of the DynamicTrialListingMapping
        /// </summary>
        public static DynamicTrialListingMapping Instance
        {
            get
            {
                Initialize();
                return (DynamicTrialListingMapping)HttpContext.Current.Cache[MAP_CACHE_KEY];
            }
        }

        private static void Initialize()
        {
            if (HttpContext.Current.Cache[MAP_CACHE_KEY] == null)
            {
                lock (syncLock)
                {
                    if (HttpContext.Current.Cache[MAP_CACHE_KEY] == null)
                    {
                        DynamicTrialListingMapping instance = new DynamicTrialListingMapping();

                        // Load up mapping files
                        Dictionary<string, MappingItem> dictEVS = GetDictionary(EVS_MAPPING_FILE, false);
                        Dictionary<string, MappingItem> dictOverrides = GetDictionary(OVERRIDE_MAPPING_FILE, true);
                        Dictionary<string, MappingItem> dictStages = GetDictionary(STAGES_MAPPING_FILE, true);
                        
                        foreach(string key in dictStages.Keys)
                        {
                            if(dictEVS.ContainsKey(key))
                            {
                                // Use stage mappings instead of EVS mappings
                                dictEVS[key] = dictStages[key];
                            }
                            else
                            {
                                // Add stage mappings if their key/value isn't present in EVS mappings
                                dictEVS.Add(key, dictStages[key]);
                            }
                        }

                        foreach (string key in dictOverrides.Keys)
                        {
                            if (dictEVS.ContainsKey(key))
                            {
                                // Use override mappings instead of EVS mappings
                                dictEVS[key] = dictOverrides[key];
                            }
                            else
                            {
                                // Add override mappings if their key/value isn't present in EVS mappings
                                dictEVS.Add(key, dictOverrides[key]);
                            }
                        }
                        
                        instance.Mappings = dictEVS;

                        instance.Tokens = GetTokens(TOKENS_MAPPING_FILE);

                        // Store instance in cache for five minutes
                        HttpContext.Current.Cache.Add(MAP_CACHE_KEY, instance, null, DateTime.Now.AddMinutes(5), System.Web.Caching.Cache.NoSlidingExpiration, System.Web.Caching.CacheItemPriority.High, null);
                    }
                }
            }
        }

        private static Dictionary<string, MappingItem> GetDictionary(string filePath, bool isOverride)
        {
            Dictionary<string, MappingItem> dict = new Dictionary<string, MappingItem>();
            try
            {
                if (File.Exists(HttpContext.Current.Server.MapPath(filePath)))
                {
                    // If file exists, use streamreader to load mappings into dictionary
                    using (StreamReader sr = new StreamReader(HttpContext.Current.Server.MapPath(filePath)))
                    {
                        string line;
                        while ((line = sr.ReadLine()) != null)
                        {
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
                            item.IsOverride = isOverride;

                            // Add mapping to dictionary if it isn't already present
                            if (!dict.ContainsKey(parts[0]))
                            {
                                dict.Add(parts[0], item);
                            }
                        }
                    }
                }
                else
                {
                    // Throw exception if file doesn't exist
                    LogManager.GetLogger(typeof(DynamicTrialListingMapping)).ErrorFormat("Mapping file '{0}' not found.", filePath);
                    throw new FileNotFoundException(filePath);
                }
            }
            catch (Exception ex)
            {
                LogManager.GetLogger(typeof(DynamicTrialListingMapping)).ErrorFormat("Error while getting the mapping file.", ex);
            }

            return dict;
        }

        private static HashSet<string> GetTokens(string filePath)
        {
            HashSet<string> tokensSet = new HashSet<string>();
            try
            {
                if (File.Exists(HttpContext.Current.Server.MapPath(filePath)))
                {
                    // If file exists, use streamreader to load mappings into dictionary
                    using (StreamReader sr = new StreamReader(HttpContext.Current.Server.MapPath(filePath)))
                    {
                        string line;
                        while ((line = sr.ReadLine()) != null)
                        {
                            tokensSet.Add(line);
                        }
                    }
                }
                else
                {
                    // Throw exception if file doesn't exist
                    LogManager.GetLogger(typeof(DynamicTrialListingMapping)).ErrorFormat("Mapping file '{0}' not found.", filePath);
                    throw new FileNotFoundException(filePath);
                }
            }
            catch (Exception ex)
            {
                LogManager.GetLogger(typeof(DynamicTrialListingMapping)).ErrorFormat("Error while getting the mapping file.", ex);
            }

            return tokensSet;
        }

        /// <summary>
        /// Gets the title-cased term. (I.E. first letter of each word is upper case)
        /// </summary>
        /// <param name="value"></param>
        /// <returns>The display label</returns>
        public string GetTitleCase (string value)
        {
            return Mappings[value].Text;
        }

        /// <summary>
        /// Gets the non-title-cased term.  This accounts for special initials, proper nouns and roman numerals though.
        /// </summary>
        /// <param name="value"></param>
        /// <returns>The display label</returns>
        public string Get(string value)
        {
            string overrideText = Mappings[value].Text;

            // Split apart string on known values (space and dash) for comparison to tokens
            string[] split = overrideText.Split(new char[] {' ', '-'});

            // For use in formatter
            int i = 0;
            List<string> keepToken = new List<string>();

            foreach(string part in split)
            {
                if(Tokens.Contains(part))
                {
                    // If do-not-replace tokens contains this string, replace with value for formatter
                    // and add token to list for later replace
                    overrideText = overrideText.Replace(part, "{" + i.ToString() + "}");
                    keepToken.Add(part);
                    i++;
                }
            }

            overrideText = overrideText.ToLower();
            overrideText = String.Format(overrideText, keepToken.ToArray());

            return overrideText;
        }

        /// <summary>
        /// Checks to see if the lookup contains an entry for the ID(s)
        /// </summary>
        /// <param name="key">The ID(s) or labels to lookup</param>
        /// <param name="isLabel">This is not used by this implementation. Defaults to false.</param>
        /// <returns>True or false based on the existance of the ID(s) in the lookup</returns>
        public bool MappingContainsKey(string key, bool isLabel = false)
        {
            return Mappings.ContainsKey(key);
            /*
            if (Mappings.ContainsKey(key))
            {
                return true;
            }
            else
            {
                // Split given codes
                List<string> splitKeys = key.Split(new char[] { ',' }).ToList();

                // If there is only one code, check to see if any other key in the mapping contains that code
                if (splitKeys.Count == 1)
                {
                    List<string> labels = new List<string>();

                    // Find all of the keys in the mapping that contain the code, and make a list of the labels for comparison.
                    labels = Mappings.Where(m => m.Key.Contains(key)).Select(m => m.Value.Text).ToList();

                    if(labels.Count < 1)
                    {
                        // If no other keys in the mapping contain the given code, return false
                        return false;
                    }
                    else if(labels.Count == 1)
                    {
                        // If only one key in the mapping contains the given code, return true
                        return true;
                    }
                    else
                    {
                        // If multiple keys in the mapping contain the given code, compare the labels of those keys.
                        // If all of the labels are the same, return true.
                        // If not, return false.
                        return labels.All(l => l == labels.First());
                    }
                }
                // If there are multiple codes, ???
                else
                {
                    //
                    List<string> splitKeyLabels = new List<string>();

                    foreach (string ID in splitKeys)
                    {
                        if (Mappings.ContainsKey(ID))
                        {
                            splitKeyLabels.Add(GetTitleCase(ID));
                        }
                        else
                        {
                            Mappings.Keys.Where(m => m.Contains(ID));

                        }
                    }
                }
            }*/
        }
    }
}
