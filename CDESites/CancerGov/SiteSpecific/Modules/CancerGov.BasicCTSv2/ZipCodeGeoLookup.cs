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
		// TODO: add Reloader to constructor once it's built out
        static ZipCodeGeoLookup()
        {
            zipCodeDict = ZipCodeGeoLoader.LoadDictionary();
        }

        
        public static ZipCodeGeoEntry GetZipCodeGeoEntry(string zipcode)
        {
            ZipCodeDictionary zips = zipCodeDict;
            if(zips.ContainsKey(zipcode))
            {
                return zips[zipcode];
            }
            else
            {
                return null;
            }
        }
    }
}
