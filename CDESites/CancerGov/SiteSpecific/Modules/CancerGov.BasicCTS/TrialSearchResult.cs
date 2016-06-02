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
                //GeoCode *IS* important here.
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

    }
}
