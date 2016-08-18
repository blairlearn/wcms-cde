using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using NCI.Logging;
using Newtonsoft.Json;

namespace CancerGov.ClinicalTrials.Basic.v2
{
    /// <summary>
    /// Class to load and do heavy lifting on the json data
    /// </summary>
    class ZipCodeGeoLoader
    {
        static string zipFilePath = ConfigurationManager.AppSettings["ZipCodesJsonMap"].ToString();

        /// <summary>
        /// Deserialize the json zip/coord mapping file into a dictionary object
        /// </summary>
        /// <returns>ZipCodeDictionary zipCodes</returns>
        public static ZipCodeDictionary LoadDictionary()
        {
            try {
                using (StreamReader r = new StreamReader(zipFilePath))
                {
                    string json = r.ReadToEnd();
                    ZipCodeDictionary zipCodes = JsonConvert.DeserializeObject<ZipCodeDictionary>(json);
                    return zipCodes;
                }
            }
            catch(FileNotFoundException ex) 
            {
                Logger.LogError("CancerGov.ClinicalTrials.Basic.v2:ZipCodeGeoLoader.cs:LoadDictionary()", "Path " + zipFilePath + " not found.", NCIErrorLevel.Error, ex);
                return null;
            }
            catch (Exception ex)
            {
                Logger.LogError("CancerGov.ClinicalTrials.Basic.v2:ZipCodeGeoLoader.cs:LoadDictionary()", "Failed to read dictionary file on path " + zipFilePath, NCIErrorLevel.Error, ex);
                return null;
            }
        }
    }
}
