using System;
using System.Collections.Generic;
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
        private static ZipCodeDictionary zipCodeDict;

        // Constructor 
        static ZipCodeGeoLookup()
        {
            zipCodeDict = ZipCodeGeoLoader.LoadDictionary();
        }

        
        public static void GetJson(string zipcode)
        {
            double lat;
            double lon;
            ZipCodeDictionary zips = zipCodeDict;
            if(zips.ContainsKey(zipcode))
            {
                lat = zips[zipcode].Latitude;
                lon = zips[zipcode].Longitude;
            }

        }
    }
}
