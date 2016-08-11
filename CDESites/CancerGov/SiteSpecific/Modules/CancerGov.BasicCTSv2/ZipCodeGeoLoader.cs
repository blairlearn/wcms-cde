using Newtonsoft.Json;
using System;
using System.Collections.Generic;
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
        public static ZipCodeDictionary LoadDictionary()
        {
            try { 
                using (StreamReader r = new StreamReader(@"C:\Development\WCMS\sites\CancerGov\PublishedContent\Files\Configuration\data\zip_codes.json"))
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
