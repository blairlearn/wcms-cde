using System;
using System.Configuration;
using System.IO;
using System.Web;
using Common.Logging;
using Newtonsoft.Json;

namespace CancerGov.ClinicalTrials.Basic.v2
{
    /// <summary>
    /// Class to load and do heavy lifting on the json data
    /// </summary>
    class ZipCodeGeoLoader
    {
        static ILog log = LogManager.GetLogger(typeof(ZipCodeGeoLoader));

        static string zipFilePath = ConfigurationManager.AppSettings["ZipCodesJsonMap"].ToString();

        /// <summary>
        /// Deserialize the json zip/coord mapping file into a dictionary object
        /// </summary>
        /// <returns>ZipCodeDictionary zipCodes</returns>
        public static ZipCodeDictionary LoadDictionary()
        {
            // - Get the context object for the current HTTP request and map the physical filepath to the 
            //   relative filepath (the relative filepath is specified in the Web.config).
            // - Read the json file using StreamReader.
            // - Deserialize the json data into a ZipCodeDictionary object.
            zipFilePath = HttpContext.Current.Server.MapPath(zipFilePath);
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
                log.ErrorFormat("LoadDictionary(): Path {0} not found.", ex, zipFilePath);
                return null;
            }
            catch (Exception ex)
            {
                log.ErrorFormat("LoadDictionary(): Failed to read dictionary file on path {0}", ex, zipFilePath);
                return null;
            }
        }
    }
}
