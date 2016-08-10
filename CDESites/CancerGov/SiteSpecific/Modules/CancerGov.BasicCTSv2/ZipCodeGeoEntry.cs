using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CancerGov.ClinicalTrials.Basic.v2
{
    public class ZipCodeGeoEntry
    {
        /// <summary>
        /// Zip code atitude
        /// </summary>
        [JsonProperty("lat")]
        public string Latitude { get; set; }

        /// <summary>
        /// Zip code longitude
        /// </summary>
        [JsonProperty("lon")]
        public string Longitude { get; set; }
    }
}
