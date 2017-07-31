using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CancerGov.ClinicalTrials.Basic.v2.Test
{
    public class LocationSearchParamsComparer: IEqualityComparer<LocationSearchParams>
    {
        #region IEqualityComparer<LocationSearchParams> Members

        /// <summary>
        /// Determine whether the specified objects are equal.
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns></returns>
        public bool Equals(LocationSearchParams x, LocationSearchParams y)
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
            } else if (x.GetType() != y.GetType()) {
                return false;
            }

            //This should compare every single property.
            //Ad we check above if x & y are the same types we don't need to check both below.
            if (x is AtNIHLocationSearchParams)
            {
                return true;
            }
            else if (x is CountryCityStateLocationSearchParams)
            {
                CountryCityStateLocationSearchParams ccsX = (CountryCityStateLocationSearchParams)x;
                CountryCityStateLocationSearchParams ccsY = (CountryCityStateLocationSearchParams)y;

                bool isEqual = ccsX.City == ccsY.City &&
                    ccsX.Country == ccsY.Country &&
                    (new LabelledSearchParamComparer()).Equals(ccsX.State, ccsY.State);

                return isEqual;
            }
            else if (x is HospitalLocationSearchParams)
            {
                bool isEqual = ((HospitalLocationSearchParams)(x)).Hospital == ((HospitalLocationSearchParams)(y)).Hospital;
                return isEqual;
            }
            else if (x is ZipCodeLocationSearchParams)
            {
                bool isEqual = ((ZipCodeLocationSearchParams)(x)).ZipCode == ((ZipCodeLocationSearchParams)(y)).ZipCode &&
                    ((ZipCodeLocationSearchParams)(x)).ZipRadius == ((ZipCodeLocationSearchParams)(y)).ZipRadius;

                return isEqual;
            }
            else
            {
                return false;
            }

        }

        /// <summary>
        /// Return a hash code for a specifed object.
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public int GetHashCode(LocationSearchParams obj)
        {
            int hash = 0;
            if (obj is AtNIHLocationSearchParams)
            {
                hash ^= true.GetHashCode();
            }
            else if (obj is CountryCityStateLocationSearchParams)
            {
                hash ^= ((CountryCityStateLocationSearchParams)(obj)).City.GetHashCode();
                hash ^= ((CountryCityStateLocationSearchParams)(obj)).Country.GetHashCode();
                hash ^= new LabelledSearchParamComparer().GetHashCode(((CountryCityStateLocationSearchParams)(obj)).State);
            }
            else if (obj is HospitalLocationSearchParams)
            {
                hash ^= ((HospitalLocationSearchParams)(obj)).Hospital.GetHashCode();                
            }
            else if (obj is ZipCodeLocationSearchParams)
            {
                hash ^= ((ZipCodeLocationSearchParams)(obj)).ZipCode.GetHashCode();
                hash ^= ((ZipCodeLocationSearchParams)(obj)).ZipRadius.GetHashCode();
            }
            else
            {
                throw new Exception("Unknown type: " + obj.GetType().ToString());
            }

            return hash;
        }

        #endregion
    }
}
