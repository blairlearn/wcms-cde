using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CancerGov.ClinicalTrials.Basic
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


        public override Nest.SearchDescriptor<T> ModifySearchParams<T>(Nest.SearchDescriptor<T> descriptor)
        {
            return base.ModifySearchParams<T>(descriptor);
                //.Query(q=> q.Term("OfficialTitle", Phrase));
        }
    }
}
