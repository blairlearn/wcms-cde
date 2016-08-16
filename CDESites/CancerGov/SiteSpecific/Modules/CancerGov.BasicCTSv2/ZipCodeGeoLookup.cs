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
        private static ZipCodeDictionary zipCodeDict;

        /// <summary>
        /// Used by WatchTemplateDirectory() to watch for changes to zip_code.json
        /// </summary>
        private static FileSystemWatcher zipCodeFileWatcher;

        /// <summary>
        /// Static constructor
        /// </summary>
        static ZipCodeGeoLookup()
        {
            zipCodeDict = ZipCodeGeoLoader.LoadDictionary();
        }

        
        public static ZipCodeGeoEntry GetZipCodeGeoEntry(string zipcode)
        {
            ZipCodeDictionary zips = zipCodeDict;


            zips = ReloadDictionary(zips);
            if(zips.ContainsKey(zipcode))
            {
                return zips[zipcode];
            }
            else
            {
                return null;
            }
        }

        static ZipCodeDictionary ReloadDictionary(ZipCodeDictionary zips)
        {
            String zipFilePath = ConfigurationSettings.AppSettings["ZipCodesJsonMap"].ToString();
            zipCodeFileWatcher = new FileSystemWatcher((Path.GetDirectoryName(zipFilePath)));
            zipCodeFileWatcher.NotifyFilter = NotifyFilters.LastWrite | NotifyFilters.CreationTime | NotifyFilters.Size | NotifyFilters.LastAccess | NotifyFilters.Attributes;
            zipCodeFileWatcher.Filter = "*.json";
            zipCodeFileWatcher.EnableRaisingEvents = true;
            zipCodeFileWatcher.Changed += new FileSystemEventHandler(OnChange);
            //zipCodeFileWatcher.Created += new FileSystemEventHandler(OnChange);
            //zipCodeFileWatcher.Deleted += new FileSystemEventHandler(OnChange);
            //zipCodeFileWatcher.Renamed += new RenamedEventHandler(OnChange);

                            

            try
            {
                using (StreamReader r = new StreamReader(zipFilePath))
                {
                    string json = r.ReadToEnd();
                    ZipCodeDictionary zipCodes = JsonConvert.DeserializeObject<ZipCodeDictionary>(json);
                    return zipCodes;
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }

                    

        /// <summary>
        /// Event handler for json files in the Configuration\files directory being modified, created, or deleted.
        /// </summary>
        /// <param name="src">event source (not used)</param>
        /// <param name="e">event arguments (not used)</param>
        private static void OnChange(object src, FileSystemEventArgs e)
        {
            ZipCodeGeoLoader.LoadDictionary();
        }

    }
}
