using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;

namespace CancerGov.ClinicalTrials.Basic.v2
{
    /// <summary>
    /// Represents the Search Parameters for a Phrase Search
    /// </summary>
    public class PhraseSearchParam : BaseCTSSearchParam
    {
        /// <summary>
        /// The Phrase to search with
        /// </summary>
        public string Phrase { get; set; }

        /// <summary>
        /// If autosuggest is broken, the cancer type entered will be parsed and searched like a phrase
        /// </summary>
        public bool IsBrokenCTSearchParam { get; set; }

    }
}
