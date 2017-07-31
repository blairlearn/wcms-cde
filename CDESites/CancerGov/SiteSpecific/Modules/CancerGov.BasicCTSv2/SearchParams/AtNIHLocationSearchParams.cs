using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CancerGov.ClinicalTrials.Basic.v2
{
    /// <summary>
    /// Defines the parameters for an At NIH location search
    /// </summary>
    public class AtNIHLocationSearchParams : LocationSearchParams
    {
        /// <summary>
        /// Creates a new instance of the At NIH Location Search Params
        /// </summary>
        public AtNIHLocationSearchParams()
        {
            this._usedFields = FormFields.AtNIH;
        }
    }
}
