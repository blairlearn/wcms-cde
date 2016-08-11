using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CancerGov.ClinicalTrials.Basic.v2
{
    class ZipCodeGeoLoader
    {
        /*
         * Class to load and do heavy lifting on the json data
         */
        static string zipFilePath = ConfigurationSettings.AppSettings["ZipCodesJsonMap"].ToString();

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
            catch(Exception ex) 
            {
                return null;
            }
        }
    }
}
