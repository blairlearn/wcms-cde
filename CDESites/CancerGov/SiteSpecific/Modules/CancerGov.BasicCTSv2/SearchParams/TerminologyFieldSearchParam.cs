using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CancerGov.ClinicalTrials.Basic.v2
{
    /// <summary>
    /// Represents a search parameter that is a Terminology item. (e.g. disease or intervention)
    /// </summary>
    public class TerminologyFieldSearchParam
    {
        /// <summary>
        /// An array of thesaurus codes that represent this item.
        /// </summary>
        public string[] Codes { get; set; }

        /// <summary>
        /// The label to display in criteria.
        /// </summary>
        public string Label { get; set; }
    }
}
