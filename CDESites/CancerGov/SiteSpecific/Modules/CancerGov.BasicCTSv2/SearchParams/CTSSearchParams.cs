using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CancerGov.ClinicalTrials.Basic.v2
{
    /// <summary>
    /// Represents the Search Parameters of a new API-based Clinical Trials Search.
    /// This includes the parameters used for a search, the labels to be displayed,
    /// as well as helpers to determine if a field was used.
    /// </summary>
    public class CTSSearchParams
    {
        /// <summary>
        /// Gets or sets the Phrase/Keyword used in the search
        /// </summary>
        public String Phrase { get; set; }

        /// <summary>
        /// Gets or sets the main cancer type that was selected.
        /// </summary>
        public TerminologyFieldSearchParam MainType { get; set; }

        //Add some sort of Errors array so we can identify when a parse error occurred.
    }
}
