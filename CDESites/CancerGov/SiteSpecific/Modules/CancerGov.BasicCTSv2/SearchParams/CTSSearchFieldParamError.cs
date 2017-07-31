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
    public class CTSSearchFieldParamError : CTSSearchParamError
    {
        /// <summary>
        /// The parameter name that caused the error.
        /// </summary>
        public override string Param
        {
            get
            {
                return Field.ToString();
            }
            set {}
        }

        /// <summary>
        /// The field that caused the error.
        /// </summary>
        public FormFields Field { get; set; }
    }
}
