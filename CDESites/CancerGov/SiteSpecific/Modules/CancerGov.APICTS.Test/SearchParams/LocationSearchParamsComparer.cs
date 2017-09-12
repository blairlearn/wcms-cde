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
                    AreLabelledParamListsEqual(ccsX.State, ccsY.State);

                return isEqual;
            }
            else if (x is HospitalLocationSearchParams)
            {
                bool isEqual = ((HospitalLocationSearchParams)(x)).Hospital == ((HospitalLocationSearchParams)(y)).Hospital;
                return isEqual;
            }
            else if (x is ZipCodeLocationSearchParams)
            {
                bool isEqual = false;
                if(((ZipCodeLocationSearchParams)(x)).GeoLocation != null && ((ZipCodeLocationSearchParams)(y)).GeoLocation != null)
                {
                    isEqual = ((ZipCodeLocationSearchParams)(x)).ZipCode == ((ZipCodeLocationSearchParams)(y)).ZipCode &&
                    ((ZipCodeLocationSearchParams)(x)).ZipRadius == ((ZipCodeLocationSearchParams)(y)).ZipRadius &&
                    ((ZipCodeLocationSearchParams)(x)).GeoLocation.Lat == ((ZipCodeLocationSearchParams)(y)).GeoLocation.Lat &&
                    ((ZipCodeLocationSearchParams)(x)).GeoLocation.Lon == ((ZipCodeLocationSearchParams)(y)).GeoLocation.Lon;
                }
                else if(((ZipCodeLocationSearchParams)(x)).GeoLocation == null && ((ZipCodeLocationSearchParams)(y)).GeoLocation == null)
                {
                    isEqual = ((ZipCodeLocationSearchParams)(x)).ZipCode == ((ZipCodeLocationSearchParams)(y)).ZipCode &&
                    ((ZipCodeLocationSearchParams)(x)).ZipRadius == ((ZipCodeLocationSearchParams)(y)).ZipRadius;
                }
                


                return isEqual;
            }
            else
            {
                return false;
            }

        }

        /// <summary>
        /// Helper function to determine if two labelled objects are equal.
        /// </summary>
        /// <param name="x">Labelled object 1</param>
        /// <param name="y">Labelled object 2</param>
        /// <returns></returns>
        private bool AreLabelledParamListsEqual(LabelledSearchParam[] x, LabelledSearchParam[] y)
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

            //Generate a set of those values that are not in both lists.
            //if this is not 0, then there is an error.
            var diffxy = x.Except(y, new LabelledSearchParamComparer());

            return diffxy.Count() == 0;
        }

        //Used to make sure hash of codes is same for two arrays with same codes.
        //Found at https://stackoverflow.com/questions/670063/getting-hash-of-a-list-of-strings-regardless-of-order
        private int GetOrderIndependentHashCode<T>(IEnumerable<T> source, IEqualityComparer<T> comparer)
        {
            int hash = 0;
            int curHash;
            int bitOffset = 0;
            // Stores number of occurences so far of each value.
            var valueCounts = new Dictionary<T, int>();

            foreach (T element in source)
            {
                curHash = comparer.GetHashCode(element);
                if (valueCounts.TryGetValue(element, out bitOffset))
                    valueCounts[element] = bitOffset + 1;
                else
                    valueCounts.Add(element, bitOffset);

                // The current hash code is shifted (with wrapping) one bit
                // further left on each successive recurrence of a certain
                // value to widen the distribution.
                // 37 is an arbitrary low prime number that helps the
                // algorithm to smooth out the distribution.
                hash = unchecked(hash + ((curHash << bitOffset) |
                    (curHash >> (32 - bitOffset))) * 37);
            }

            return hash;
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
                hash ^= GetOrderIndependentHashCode(((CountryCityStateLocationSearchParams)(obj)).State, new LabelledSearchParamComparer());
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
