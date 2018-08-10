using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
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
    public class DynamicTrialListingMappingService : ITerminologyLookupService
    {   
        // Lock synchronization object
        private static object syncLock = new Object();

        private static readonly string MAP_CACHE_KEY = "LabelMapping";
        private static readonly string EVS_MAPPING_FILE = BasicClinicalTrialSearchAPISection.GetEvsMappingFilePath();
        private static readonly string OVERRIDE_MAPPING_FILE = BasicClinicalTrialSearchAPISection.GetOverrideMappingFilePath();
        private static readonly string TOKENS_MAPPING_FILE = BasicClinicalTrialSearchAPISection.GetTokenMappingFilePath();
        private DynamicTrialListingMapper _mapper = null;
       
        private DynamicTrialListingMappingService() { }

        /// <summary>
        /// Gets an instance of the DynamicTrialListingMapping
        /// </summary>
        public static DynamicTrialListingMappingService Instance
        {
            get
            {
                Initialize();
                return (DynamicTrialListingMappingService)HttpContext.Current.Cache[MAP_CACHE_KEY];
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
                        DynamicTrialListingMappingService instance = new DynamicTrialListingMappingService();

                        string evsMappingFilepath = MapFilepath(EVS_MAPPING_FILE);
                        string overrideMappingFilepath = MapFilepath(OVERRIDE_MAPPING_FILE);
                        string tokensMappingFilepath = MapFilepath(TOKENS_MAPPING_FILE);

                        instance._mapper = new DynamicTrialListingMapper(evsMappingFilepath, overrideMappingFilepath, tokensMappingFilepath);

                        HttpContext.Current.Cache.Add(MAP_CACHE_KEY, instance, null, DateTime.Now.AddMinutes(5), System.Web.Caching.Cache.NoSlidingExpiration, System.Web.Caching.CacheItemPriority.High, null);
                    }
                }
            }
        }

        private static string MapFilepath(string filePath)
        {
            if(string.IsNullOrEmpty(filePath))
            {
                LogManager.GetLogger(typeof(DynamicTrialListingMappingService)).ErrorFormat("Mapping file cannot be null or empty.");
                throw new ArgumentNullException(filePath);
            }

            if (File.Exists(HttpContext.Current.Server.MapPath(filePath)))
            {
                return HttpContext.Current.Server.MapPath(filePath);
            }
            else
            {
                // Throw exception if file doesn't exist
                LogManager.GetLogger(typeof(DynamicTrialListingMappingService)).ErrorFormat("Mapping file '{0}' not found.", filePath);
                throw new FileNotFoundException(filePath);
            }
        }

        /// <summary>
        /// Gets the title-cased term. (I.E. first letter of each word is upper case)
        /// </summary>
        /// <param name="value"></param>
        /// <returns>The display label</returns>
        public string GetTitleCase (string value)
        {
            return _mapper.GetTitleCase(value);
        }

        /// <summary>
        /// Gets the non-title-cased term.  This accounts for special initials, proper nouns and roman numerals though.
        /// </summary>
        /// <param name="value"></param>
        /// <returns>The display label</returns>
        public string Get(string value)
        {
            return _mapper.Get(value);
        }

        /// <summary>
        /// Checks to see if the lookup contains an entry for the ID(s)
        /// </summary>
        /// <param name="key">The ID(s) or labels to lookup</param>
        /// <param name="withOverrides">Whether or not to check the overrides mapping.</param>
        /// <returns>True or false based on the existance of the ID(s) in the lookup</returns>
        public bool MappingContainsKey(string code, bool isLabel = false)
        {
            return _mapper.MappingContainsKey(code);
        }
    }
}
