using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CancerGov.ClinicalTrials.Basic.v2
{
    /// <summary>
    /// Defines the parameters for a Hospital location search
    /// </summary>
    public class HospitalLocationSearchParams : LocationSearchParams
    {
        string _hospital = string.Empty;

        /// <summary>
        /// Gets or sets the Hospital used in the search
        /// </summary>
        public String Hospital
        {
            get { return _hospital; }
            set { _hospital = value; _usedFields |= FormFields.Hospital; }
        }
    }
}
