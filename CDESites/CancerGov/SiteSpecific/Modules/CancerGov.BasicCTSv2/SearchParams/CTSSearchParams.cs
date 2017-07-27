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
        /// Gets or sets the lead org used in the search
        /// </summary>
        public String LeadOrg { get; set; }

        /// <summary>
        /// Gets or sets the Hospital used in the search
        /// </summary>
        public String Hospital { get; set; }

        /// <summary>
        /// Gets or sets the Investigator used in the search
        /// </summary>
        public String Investigator { get; set; }


        /// <summary>
        /// Gets or sets the main cancer type that was selected.
        /// </summary>
        public TerminologyFieldSearchParam MainType { get; set; }

        /// <summary>
        /// Gets or sets an array of the subtypes for this search definition
        /// </summary>
        public TerminologyFieldSearchParam[] SubTypes { get; set; }

        /// <summary>
        /// Gets or sets an array of the stages for this search definition
        /// </summary>
        public TerminologyFieldSearchParam[] Stages { get; set; }

        /// <summary>
        /// Gets or sets an array of the findings for this search definition
        /// </summary>
        public TerminologyFieldSearchParam[] Findings { get; set; }

        /// <summary>
        /// Gets or sets an array of the drugs for this search definition
        /// </summary>
        public TerminologyFieldSearchParam[] Drugs { get; set; }

        /// <summary>
        /// Gets or sets an array of the other treatments for this search definition
        /// </summary>
        public TerminologyFieldSearchParam[] OtherTreatments { get; set; }



        //Add some sort of Errors array so we can identify when a parse error occurred.
    }
}
