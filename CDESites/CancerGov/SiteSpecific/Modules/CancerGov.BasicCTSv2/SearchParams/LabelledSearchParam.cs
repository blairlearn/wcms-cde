using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CancerGov.ClinicalTrials.Basic.v2
{
    /// <summary>
    /// Represents a search parameter that needs a label. (e.g. state, trial type, or trial phase)
    /// </summary>
    public class LabelledSearchParam
    {
        /// <summary>
        /// The key value that is passed in for this search param.
        /// </summary>
        public string Key { get; set; }

        /// <summary>
        /// The label to display in criteria.
        /// </summary>
        public string Label { get; set; }
    }
}
