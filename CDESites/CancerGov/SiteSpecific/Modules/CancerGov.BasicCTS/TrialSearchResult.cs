using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CancerGov.ClinicalTrials.Basic
{
    /// <summary>
    /// Represents a Trial as returned by a search
    /// </summary>
    public class TrialSearchResult : TrialBase
    {
        public TrialLocation[] Locations { get; set; }

        private TrialLocation[] _USLocations = null;

        public class TrialLocation
        {
            public string FacilityName { get; set; }
            public LocationPostalAddress PostalAddress { get; set; }

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

        /// <summary>
        /// Get all us Locations, but filtered by origin and radius in miles
        /// </summary>
        /// <param name="origin"></param>
        /// <param name="radius"></param>
        /// <returns></returns>
        public TrialLocation[] GetUSLocations(GeoLocation origin, int radius)
        {
                return (from location in this.GetUSLocations()
                                where location.PostalAddress.GeoCode != null && origin.DistanceBetween(location.PostalAddress.GeoCode) <= radius
                                select location).ToArray();
        }

    }
}
