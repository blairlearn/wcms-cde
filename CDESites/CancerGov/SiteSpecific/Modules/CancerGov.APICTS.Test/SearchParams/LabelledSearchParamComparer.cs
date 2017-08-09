using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CancerGov.ClinicalTrials.Basic.v2.Test
{
    /// <summary>
    /// Comparer for a LabelledSearchParam. Checks to see if equivalent.
    /// </summary>
    public class LabelledSearchParamComparer : IEqualityComparer<LabelledSearchParam>
    {
        #region IEqualityComparer<LabelledSearchParam> Members

        /// <summary>
        /// Determine whether the specified objects are equal.
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns></returns>
        public bool Equals(LabelledSearchParam x, LabelledSearchParam y)
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
            bool isEqual = (x.Key == y.Key && x.Label == y.Label);

            return isEqual;

        }

        /// <summary>
        /// Return a hash code for a specifed object.
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public int GetHashCode(LabelledSearchParam obj)
        {
            int hash = 0;
            hash ^= obj.Key.GetHashCode();
            hash ^= obj.Label.GetHashCode();

            return hash;
        }

        #endregion
    }
}
