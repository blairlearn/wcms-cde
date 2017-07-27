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
                x.Codes == y.Codes &&
                x.Label == y.Label;

            return isEqual;

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
