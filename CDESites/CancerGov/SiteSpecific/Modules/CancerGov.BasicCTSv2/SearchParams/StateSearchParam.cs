using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CancerGov.ClinicalTrials.Basic.v2
{
    /// <summary>
    /// Represents the state search parameter.
    /// </summary>
    public class StateSearchParam
    {
        /// <summary>
        /// The state abbreviation for this item.
        /// </summary>
        public string Abbreviation { get; set; }

        /// <summary>
        /// The label to display in criteria.
        /// </summary>
        public string Label { get; set; }
    }
}
