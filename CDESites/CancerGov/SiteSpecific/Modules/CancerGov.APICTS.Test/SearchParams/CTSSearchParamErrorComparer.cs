using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CancerGov.ClinicalTrials.Basic.v2.Test
{
    public class CTSSearchParamErrorComparer : IEqualityComparer<CTSSearchParamError>
    {
        #region IEqualityComparer<CTSSearchParamError> Members

        /// <summary>
        /// Determine whether the specified objects are equal.
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns></returns>
        public bool Equals(CTSSearchParamError x, CTSSearchParamError y)
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

            if(x.GetType() != y.GetType())
            {
                return false;
            }

            //This should compare every single property.
            bool isEqual = (x.Param == y.Param && x.ErrorMessage == y.ErrorMessage);

            return isEqual;

        }

        /// <summary>
        /// Return a hash code for a specifed object.
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public int GetHashCode(CTSSearchParamError obj)
        {
            int hash = 0;
            hash ^= obj.Param.GetHashCode();
            hash ^= obj.ErrorMessage.GetHashCode();

            return hash;
        }

        #endregion
    }
}
