using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CancerGov.ClinicalTrials.Basic.v2
{
    /// <summary>
    /// Defines the possible choices for a location search.
    /// </summary>
    public enum LocationType
    {
        /// <summary>
        /// No Location was chosen
        /// </summary>
        None = 0,
        /// <summary>
        /// The Zip Code search was chosen
        /// </summary>
        Zip = 1,        
        /// <summary>
        /// Country, City, State was chosen
        /// </summary>
        CountryCityState = 2,
        /// <summary>
        /// Hospital or Institution
        /// </summary>
        Hospital = 3,
        /// <summary>
        /// At NIH
        /// </summary>
        AtNIH = 4
    }
}
