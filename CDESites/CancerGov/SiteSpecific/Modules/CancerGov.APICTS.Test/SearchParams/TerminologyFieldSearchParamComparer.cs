using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CancerGov.ClinicalTrials.Basic.v2.Test
{
    /// <summary>
    /// Comparer for a TerminologyFieldSearchParam. Checks to see if equivalent.
    /// </summary>
    public class TerminologyFieldSearchParamComparer : IEqualityComparer<TerminologyFieldSearchParam>
    {
        #region IEqualityComparer<TerminologyFieldSearchParam> Members

        public bool Equals(TerminologyFieldSearchParam x, TerminologyFieldSearchParam y)
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
                this.AreCodesListsEqual(x.Codes, y.Codes) &&
                x.Label == y.Label;

            return isEqual;

        }

        /// <summary>
        /// Helper function to determine if two synonym lists are equal, order does not matter.
        /// </summary>
        /// <param name="x">Synonym list 1</param>
        /// <param name="y">Synonym list 2</param>
        /// <returns></returns>
        private bool AreCodesListsEqual(string[] x, string[] y)
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
            var diffxy = x.Except(y, StringComparer.CurrentCulture);

            return diffxy.Count() == 0;
        }


        public int GetHashCode(TerminologyFieldSearchParam obj)
        {
            int hash = 0;
            hash ^= GetOrderIndependentHashCode<string>(obj.Codes);
            hash ^= obj.Label.GetHashCode();

            return hash;
        }

        #endregion

        //Used to make sure hash of codes is same for two arrays with same codes.
        //Found at https://stackoverflow.com/questions/670063/getting-hash-of-a-list-of-strings-regardless-of-order
        private int GetOrderIndependentHashCode<T>(IEnumerable<T> source)
        {
            int hash = 0;
            int curHash;
            int bitOffset = 0;
            // Stores number of occurences so far of each value.
            var valueCounts = new Dictionary<T, int>();

            foreach (T element in source)
            {
                curHash = EqualityComparer<T>.Default.GetHashCode(element);
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

    }
}
