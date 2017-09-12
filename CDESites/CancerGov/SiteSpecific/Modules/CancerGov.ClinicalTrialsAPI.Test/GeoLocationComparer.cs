using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CancerGov.ClinicalTrialsAPI.Test
{
    public class GeoLocationComparer : IEqualityComparer<ClinicalTrial.StudySite.GeoLocation>
    {

        #region IEqualityComparer<GeoLocation> Members

        public bool Equals(ClinicalTrial.StudySite.GeoLocation x, ClinicalTrial.StudySite.GeoLocation y)
        {
            // If the items are both null, or if one or the other is null, return 
            // the correct response right away.
            if (x == null && y == null)
            {
                return true;
            }
            else if (x == null || y == null)
            {
                return false;
            }

            //This should compare every single property.
            bool isEqual =
                x.Latitude == y.Latitude &&
                x.Longitude == y.Longitude;

            return isEqual;
        }

        public int GetHashCode(ClinicalTrial.StudySite.GeoLocation obj)
        {
            int hash = 0;

            hash ^= obj.Latitude.GetHashCode();
            hash ^= obj.Longitude.GetHashCode();

            return hash;
        }

        #endregion
    }
}
