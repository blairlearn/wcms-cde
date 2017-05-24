using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Web;
using CancerGov.ClinicalTrials.Basic.v2.Configuration;
using Common.Logging;

namespace CancerGov.ClinicalTrials.Basic.v2
{
    public class DynamicTrialListingMapping
    {   
        // Lock synchronization object
        private static object syncLock = new Object();

        private static readonly string MAP_CACHE_KEY = "LabelMapping";
        private static readonly string EVS_MAPPING_FILE = BasicClinicalTrialSearchAPISection.GetEvsMappingFilePath();
        private static readonly string OVERRIDE_MAPPING_FILE = BasicClinicalTrialSearchAPISection.GetMappingOverrideFilePath();
        private Dictionary<string, string> Mappings = new Dictionary<string, string>();
       
        private DynamicTrialListingMapping() { }

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
                        Dictionary<string, string> dictEVS = GetDictionary(EVS_MAPPING_FILE);
                        Dictionary<string, string> dictOverrides = GetDictionary(OVERRIDE_MAPPING_FILE);
                        
                        foreach(string key in dictOverrides.Keys)
                        {
                            if(dictEVS.ContainsKey(key))
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

                        // Store instance in cache for five minutes
                        HttpContext.Current.Cache.Add(MAP_CACHE_KEY, instance, null, DateTime.Now.AddMinutes(5), System.Web.Caching.Cache.NoSlidingExpiration, System.Web.Caching.CacheItemPriority.High, null);
                    }
                }
            }
        }

        private static Dictionary<string, string> GetDictionary(string filePath)
        {
            Dictionary<string, string> dict = new Dictionary<string,string>();
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

                            // Add mapping to dictionary if it isn't already present
                            if (!dict.ContainsKey(parts[0]))
                            {
                                dict.Add(parts[0], parts[1]);
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

        public string this[string code]
        {
            get{
                return Mappings[code];
            }
        }

        public bool MappingContainsKey(string key)
        {
            return Mappings.ContainsKey(key);
        }
    }
}
