using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CancerGov.ClinicalTrials.Basic.v2
{
    public static class LegacyIDLookup
    {
        /// <summary>
        /// LegacyCancerTypeDictionary lookup dictionary.
        /// </summary>
        private static LegacyCancerTypeDictionary cancerTypeDictionary;

    
        /// <summary>
        /// Used by WatchTemplateDirectory() to watch for changes to zip_code.json
        /// </summary>
        //private static FileSystemWatcher zipCodeFileWatcher;

        /// <summary>
        /// Static constructor - initializes lookup objects
        /// </summary>
        static LegacyIDLookup()
        {
            cancerTypeDictionary = LegacyIDLoader.LoadCancerTypeDictionary();
            //WatchDictionaryFile();
        }
}
}
