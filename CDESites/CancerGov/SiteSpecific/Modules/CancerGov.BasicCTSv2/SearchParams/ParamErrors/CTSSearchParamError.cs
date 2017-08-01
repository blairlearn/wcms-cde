using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CancerGov.ClinicalTrials.Basic.v2
{
    public class CTSSearchParamError
    {
        /// <summary>
        /// The parameter name that caused the error.
        /// </summary>
        public virtual string Param { get; set; }

        /// <summary>
        /// The error message associated with the broken parameter
        /// </summary>
        public string ErrorMessage { get; set; }
    }
}
