using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CancerGov.ClinicalTrials.Basic
{
    /// <summary>
    /// Represents the complete information for a Trial.
    /// </summary>
    public class TrialDescription : TrialBase
    {
        public string BriefSummary { get; set; }
        public string DetailedDescription { get; set; }
        public string CTEntryCriteria { get; set; }
        public string CTGovDisclaimer { get; set; }
        public TrialLocation[] Locations { get; set; }

        //public IEnumerable<Object> SortedAllLocations
        //{
        //    get
        //    {
                
        //    }
        //}

        private TrialLocation[] _USLocations = null;

        /// <summary>
        /// Get all US Locations
        /// </summary>
        /// <returns></returns>
        public TrialLocation[] GetUSLocations()
        {
            if (_USLocations == null)
            {
                _USLocations = (from location in this.Locations
                                where location.PostalAddress.CountryName == "U.S.A."
                                select location).ToArray();
            }

            return _USLocations;
        }

        //public Dictionary<string,List<TrialLocation>> GetLocationsByCountry(string country)
        //{
        //    //Group By Political Sub Unit.
        //    //Then by hospital
                        
        //    if (country == "U.S.A.")
        //    {
        //        Dictionary<string, List<TrialLocation>> locations = new Dictionary<string,List<TrialLocation>>();

        //        var sortedSites = from location in GetUSLocations()
        //                          where location.PostalAddress.CountryName == "U.S.A."
        //                          orderby location.PostalAddress.PoliticalSubUnitName, location.FacilityName
        //                          select location;

        //        foreach (TrialLocation location in sortedSites)
        //        {

        //            if (!locations.ContainsKey(location.PostalAddress.PoliticalSubUnitName))
        //                locations.Add(location.PostalAddress.PoliticalSubUnitName, new List<TrialLocation>());

        //            locations[location.PostalAddress.PoliticalSubUnitName].Add(location);
        //        }

                

        //        return locations;

        //    }

        //    return new Dictionary<string, List<TrialLocation>>();
        //}


        public IEnumerable<TrialLocation> GetLocationsNearZip(GeoLocation origin, int radius)
        {
            return (from location in this.GetUSLocations()
                    where location.PostalAddress.GeoCode != null && origin.DistanceBetween(location.PostalAddress.GeoCode) <= radius
                    orderby origin.DistanceBetween(location.PostalAddress.GeoCode) ascending
                    select location).ToArray();
        }


        #region Supporting Classes

        public class TrialLocation
        {
            public string FacilityName { get; set; }
            public LocationPostalAddress PostalAddress { get; set; }
            /** need to add:
            "CTGovContact": { "type": "object"},
            "Investigator": { "type": "object"}
            **/


            public class LocationPostalAddress
            {
                public string CountryName { get; set; }
                public string City { get; set; }
                public string PoliticalSubUnitName { get; set; }
                public string PostalCode_ZIP { get; set; }
                public string PostalCodePosition { get; set; }
                public GeoLocation GeoCode { get; set; }
            }

        }


        #endregion

    }
}
