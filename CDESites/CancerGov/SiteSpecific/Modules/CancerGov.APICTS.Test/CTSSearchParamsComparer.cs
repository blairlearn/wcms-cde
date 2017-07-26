using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CancerGov.ClinicalTrials.Basic.v2.Test
{
    /// <summary>
    /// Comparer for a CTSSearchParams class.
    /// </summary>
    public class CTSSearchParamsComparer : IEqualityComparer<CTSSearchParams>
    {

        #region IEqualityComparer<CTSSearchParams> Members

        public bool Equals(CTSSearchParams x, CTSSearchParams y)
        {
            return false;
        }

        public int GetHashCode(CTSSearchParams obj)
        {
            return 0;
        }

        #endregion
    }
}
