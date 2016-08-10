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
         * Manager class
         */
        public static void GetJson(string zipcode)
        {
            using (StreamReader r = new StreamReader(@"C:\Development\WCMS\sites\CancerGov\PublishedContent\Files\Configuration\data\zip_codes.json"))
            {
                string json = r.ReadToEnd();
                ZipCodeDictionary zipCodes = JsonConvert.DeserializeObject<ZipCodeDictionary>(json);
            }
        }
    }
}
