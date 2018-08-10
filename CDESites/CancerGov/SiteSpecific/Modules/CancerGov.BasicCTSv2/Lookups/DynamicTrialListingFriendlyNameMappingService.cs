using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.IO;
using System.Web;
using Common.Logging;
using CancerGov.ClinicalTrials.Basic.v2.Lookups;

namespace CancerGov.ClinicalTrials.Basic.v2
{
    /// <summary>
    /// Class represents a singleton that can be used to lookup a C-code for
    /// user-friendly URL name.  This is used by the Dynamic Listing Pages.
    /// </summary>
    public class DynamicTrialListingFriendlyNameMappingService

    {
        // Lock synchronization object
        private static object syncLock = new Object();

        private DynamicTrialListingFriendlyNameMapper _mapper = null;

        private DynamicTrialListingFriendlyNameMappingService() { }

        public static DynamicTrialListingFriendlyNameMappingService GetMapping(string friendlyNameMappingFile, string overrideFriendlyNameMappingFile, bool withOverrides)
        {
            if (string.IsNullOrEmpty(friendlyNameMappingFile) || string.IsNullOrEmpty(overrideFriendlyNameMappingFile))
            {
                LogManager.GetLogger(typeof(DynamicTrialListingFriendlyNameMappingService)).ErrorFormat("Mapping file cannot be null or empty.");
                throw new ArgumentException("The dictionary mapping filepaths must not be empty or null.");
            }

            if (HttpContext.Current.Cache[friendlyNameMappingFile] == null)
            {
                lock (syncLock)
                {
                    if (HttpContext.Current.Cache[friendlyNameMappingFile] == null)
                    {
                        DynamicTrialListingFriendlyNameMappingService instance = new DynamicTrialListingFriendlyNameMappingService();

                        string friendlyNameMappingFilepath = MapFilepath(friendlyNameMappingFile);
                        string overrideFriendlyNameMappingFilepath = MapFilepath(overrideFriendlyNameMappingFile);

                        instance._mapper = new DynamicTrialListingFriendlyNameMapper(friendlyNameMappingFilepath, overrideFriendlyNameMappingFilepath, false);

                        // Store instance in cache for five minutes
                        HttpContext.Current.Cache.Add(friendlyNameMappingFile, instance, null, DateTime.Now.AddMinutes(5), System.Web.Caching.Cache.NoSlidingExpiration, System.Web.Caching.CacheItemPriority.High, null);
                    }
                }
            }

            if (HttpContext.Current.Cache[overrideFriendlyNameMappingFile] == null)
            {
                lock (syncLock)
                {
                    if (HttpContext.Current.Cache[overrideFriendlyNameMappingFile] == null)
                    {
                        DynamicTrialListingFriendlyNameMappingService instance = new DynamicTrialListingFriendlyNameMappingService();

                        string friendlyNameMappingFilepath = MapFilepath(friendlyNameMappingFile);
                        string overrideFriendlyNameMappingFilepath = MapFilepath(overrideFriendlyNameMappingFile);

                        instance._mapper = new DynamicTrialListingFriendlyNameMapper(friendlyNameMappingFilepath, overrideFriendlyNameMappingFilepath, true);

                        // Store instance in cache for five minutes
                        HttpContext.Current.Cache.Add(overrideFriendlyNameMappingFile, instance, null, DateTime.Now.AddMinutes(5), System.Web.Caching.Cache.NoSlidingExpiration, System.Web.Caching.CacheItemPriority.High, null);
                    }
                }
            }

            return withOverrides ? (DynamicTrialListingFriendlyNameMappingService)HttpContext.Current.Cache[overrideFriendlyNameMappingFile] : (DynamicTrialListingFriendlyNameMappingService)HttpContext.Current.Cache[friendlyNameMappingFile];
        }

        private static string MapFilepath(string filePath)
        {
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
        /// Gets the friendly name that maps to the given C-code(s).
        /// </summary>
        /// <param name="value"></param>
        /// <returns>The friendly name</returns>
        public string GetFriendlyNameFromCode(string value, bool hasExactMatch)
        {
            return _mapper.GetFriendlyNameFromCode(value, hasExactMatch);
        }

        /// <summary>
        /// Gets the C-code (or combination of) that maps to the given pretty name.
        /// </summary>
        /// <param name="value"></param>
        /// <returns>The C-code(s)</returns>
        public string GetCodeFromFriendlyName(string value)
        {
            return _mapper.GetCodeFromFriendlyName(value);
        }

        /// <summary>
        /// Checks to see if the lookup contains an entry for the C-code(s), whether an exact match or contained in keys that all have the same friendly name.
        /// </summary>
        /// <param name="key">The C-code(s) to lookup</param>
        /// <returns>True or false based on the existance of the C-code(s) in the lookup</returns>
        public bool MappingContainsCode(string code, bool needsExactMatch)
        {
            return _mapper.MappingContainsCode(code, needsExactMatch);
        }

        /// <summary>
        /// Checks to see if the lookup contains an entry for the pretty name
        /// </summary>
        /// <param name="value">The pretty name to lookup</param>
        /// <returns>True or false based on the existance of the pretty name in the lookup</returns>
        public bool MappingContainsFriendlyName(string value)
        {
            return _mapper.MappingContainsFriendlyName(value);
        }
    }
}

