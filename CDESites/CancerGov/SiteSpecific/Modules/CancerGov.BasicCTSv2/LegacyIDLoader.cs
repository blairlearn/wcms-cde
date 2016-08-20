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
    /// Class to load and do heavy lifting on loading lookup data.
    /// </summary>
    class LegacyIDLoader
    {
        //static string cancerTypeFilePath = ConfigurationManager.AppSettings["LegacyCancerTypeMap"].ToString();
        static string cancerTypeFilePath = "C:\\src\\WCMTeam\\cde\\siteContent\\CancerGov\\Files\\Configuration\\data\\cts-cdrid-to-ccid-map.txt";

        /// <summary>
        /// Retrieves pipe 
        /// </summary>
        /// <returns>ZipCodeDictionary zipCodes</returns>
        public static LegacyCancerTypeDictionary LoadCancerTypeDictionary()
        {
            try {
                //using (StreamReader r = new StreamReader(cancerTypeFilePath))
                //{
                //    string json = r.ReadToEnd();
                //    ZipCodeDictionary zipCodes = JsonConvert.DeserializeObject<ZipCodeDictionary>(json);
                //    return zipCodes;
                //}
                return null;
            }
            catch(FileNotFoundException ex) 
            {
                Logger.LogError("CancerGov.ClinicalTrials.Basic.v2:LegacyIDLoader:LoadCancerTypeDictionary()", "Path " + cancerTypeFilePath + " not found.", NCIErrorLevel.Error, ex);
                return null;
            }
            catch (Exception ex)
            {
                Logger.LogError("CancerGov.ClinicalTrials.Basic.v2:LegacyIDLoader:LoadCancerTypeDictionary()", "Failed to read dictionary file on path " + cancerTypeFilePath, NCIErrorLevel.Error, ex);
                return null;
            }
        }
    }
}
