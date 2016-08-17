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
        /// Static constructor - initializes ZipCodeDictionary object
        /// </summary>
        static ZipCodeGeoLookup()
        {
            zipCodeDictionary = ZipCodeGeoLoader.LoadDictionary();
            ReloadDictionary();
        }

        /// <summary>
        /// Check against the dictionary of zipcodes and return a ZipCodeGeoEntry object 
        /// if a match is found.
        /// </summary>
        /// <param name="zipCodeEntry">5-digit zip code string</param>
        /// <returns>ZipCodeGeoEntry or null if no match</returns>
        public static ZipCodeGeoEntry GetZipCodeGeoEntry(string zipCodeEntry)
        {
            ZipCodeDictionary zipDict = zipCodeDictionary;
            GenerateData();

            if(zipDict.ContainsKey(zipCodeEntry))
            {
                return zipDict[zipCodeEntry];
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        static void GenerateData()
        {
            Object lockObject = new Object();
            if(zipCodeDictionary == null)
            {
                lock(lockObject)
                {
                    if(zipCodeDictionary == null)
                    {
                        zipCodeDictionary = ZipCodeGeoLoader.LoadDictionary();
                    }
                }
            }
        }

        /// <summary>
        /// Watch for and handle changes to the zip codes JSON file usied for the search params mapping.
        /// </summary>
        static void ReloadDictionary()
        {
            String zipFilePath = ConfigurationManager.AppSettings["ZipCodesJsonMap"].ToString();
            zipCodeFileWatcher = new FileSystemWatcher((Path.GetDirectoryName(zipFilePath)));
            zipCodeFileWatcher.NotifyFilter = NotifyFilters.LastWrite | NotifyFilters.CreationTime | NotifyFilters.Size | NotifyFilters.LastAccess | NotifyFilters.Attributes;
            zipCodeFileWatcher.Filter = "*.json";
            zipCodeFileWatcher.EnableRaisingEvents = true;
            zipCodeFileWatcher.Created += new FileSystemEventHandler(OnChange);
            zipCodeFileWatcher.Changed += new FileSystemEventHandler(OnChange);
            zipCodeFileWatcher.Deleted += new FileSystemEventHandler(OnRemove);
            zipCodeFileWatcher.Renamed += new RenamedEventHandler(OnRename);
        }

        /// <summary>
        /// Event handler for .json file in the Configuration\files directory being modified or created.
        /// Load the dictionary again upon file update.
        /// </summary>
        /// <param name="src">event source (not used)</param>
        /// <param name="e">event arguments (not used)</param>
        private static void OnChange(object src, FileSystemEventArgs e)
        {
            zipCodeDictionary = ZipCodeGeoLoader.LoadDictionary();
        }

        /// <summary>
        /// Event handler for .json file in the Configuration\files directory being deleted.
        /// </summary>
        /// <param name="src">event source (not used)</param>
        /// <param name="e">event arguments (not used)</param>
        private static void OnRemove(object src, FileSystemEventArgs e) { }

        /// <summary>
        /// Event handler for .json file in the Configuration\files directory being renamed.
        /// </summary>
        /// <param name="src">event source (not used)</param>
        /// <param name="e">event arguments (not used)</param>
        private static void OnRename(object source, RenamedEventArgs e) { }

    }
}
