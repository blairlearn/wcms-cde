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
            hash ^= obj.Codes.GetHashCode();
            hash ^= obj.Label.GetHashCode();

            return hash;
        }

        #endregion
    }
}
