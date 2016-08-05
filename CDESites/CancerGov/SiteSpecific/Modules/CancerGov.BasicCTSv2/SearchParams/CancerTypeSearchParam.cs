using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CancerGov.ClinicalTrials.Basic.v2
{
    /// <summary>
    /// Represents the Parameters of a Cancer Type Search
    /// </summary>
    public class CancerTypeSearchParam : BaseCTSSearchParam
    {
        /// <summary>
        /// Get and Set the Cancer Type ID for this search
        /// </summary>
        public string CancerTypeID { get; set; }

        /// <summary>
        /// Gets and Sets the cancer type display name to use for this search
        /// </summary>
        public string CancerTypeDisplayName { get; set; }

    }
}
