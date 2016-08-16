using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NCI.Logging;
using NCI.Util;

using Newtonsoft.Json;

namespace CancerGov.ClinicalTrials.Basic.v2
{
    /*
     * This will be our manager class
     */
    /**

     * TODO: 
     *  - FIX INVALID ZIP LOGIC
     *  - Debug RenamedEventHandler
     *  - Comment "OnRemove" 
     *  - Add null check on initial load
     */

    public static class ZipCodeGeoLookup
    {

        /// <summary>
        /// ZipCodeDictionary field that will be used for Loader/Reloader
        /// </summary>
        private static ZipCodeDictionary zipCodeDictionary;

        /// <summary>
        /// Used by WatchTemplateDirectory() to watch for changes to zip_code.json
        /// </summary>
        private static FileSystemWatcher zipCodeFileWatcher;

        /// <summary>
        /// Static constructor
        /// </summary>
        static ZipCodeGeoLookup()
        {
            zipCodeDictionary = ZipCodeGeoLoader.LoadDictionary();
        }

        
        public static ZipCodeGeoEntry GetZipCodeGeoEntry(string zipcodeEntry)
        {
            ZipCodeDictionary zipDict = zipCodeDictionary;

            ReloadDictionary(zipDict);

            if(zipDict.ContainsKey(zipcodeEntry))
            {
                return zipDict[zipcodeEntry];
            }
            else
            {
                return null;
            }
        }

        static void ReloadDictionary(ZipCodeDictionary zips)
        {
            String zipFilePath = ConfigurationManager.AppSettings["ZipCodesJsonMap"].ToString();
            zipCodeFileWatcher = new FileSystemWatcher((Path.GetDirectoryName(zipFilePath)));
            zipCodeFileWatcher.NotifyFilter = NotifyFilters.LastWrite | NotifyFilters.CreationTime | NotifyFilters.Size | NotifyFilters.LastAccess | NotifyFilters.Attributes;
            zipCodeFileWatcher.Filter = "*.json";
            zipCodeFileWatcher.EnableRaisingEvents = true;
            zipCodeFileWatcher.Created += new FileSystemEventHandler(OnChange);
            zipCodeFileWatcher.Changed += new FileSystemEventHandler(OnChange);
            zipCodeFileWatcher.Deleted += new FileSystemEventHandler(OnRemove);
            zipCodeFileWatcher.Renamed += new RenamedEventHandler(OnRemove);

            /*
            (if zips == null) 
            {
                zips.Add
            }
             */

            //return zipCodeDict;
        }

                    

        /// <summary>
        /// Event handler for .json file in the Configuration\files directory being modified or created.
        /// </summary>
        /// <param name="src">event source (not used)</param>
        /// <param name="e">event arguments (not used)</param>
        private static void OnChange(object src, FileSystemEventArgs e)
        {
            zipCodeDictionary = ZipCodeGeoLoader.LoadDictionary();
        }

        /// <summary>
        /// Event handler for .json file in the Configuration\files directory being renamed or deleted.
        /// </summary>
        /// <param name="src">event source (not used)</param>
        /// <param name="e">event arguments (not used)</param>
        private static void OnRemove(object src, FileSystemEventArgs e)
        {

        }
    }
}
