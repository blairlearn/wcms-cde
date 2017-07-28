using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CancerGov.ClinicalTrials.Basic.v2
{
    /// <summary>
    /// Represents a search param error for the CTSSearchParams
    /// </summary>
    public class CTSSearchParamError
    {
        /// <summary>
        /// The parameter name that caused the error.
        /// </summary>
        public string Param { get; set; }

        /// <summary>
        /// The error message associated with the broken parameter
        /// </summary>
        public string ErrorMessage { get; set; }
    }
}
