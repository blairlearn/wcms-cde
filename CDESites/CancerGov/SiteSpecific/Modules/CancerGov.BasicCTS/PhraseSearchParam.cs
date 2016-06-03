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

        protected override void AddTemplateParams(Nest.FluentDictionary<string, object> paramdict)
        {

            // Set the searchstring only if we have one.  Maybe clean it up too if needbe.

            if (!String.IsNullOrWhiteSpace(Phrase))
                paramdict.Add("searchstring", this.Phrase);
            else
                paramdict.Add("searchstring", "cancer"); //Hack for now until Min fixes the template. :(
            
        }

        protected override Nest.SearchTemplateDescriptor<T> ModifySearchParams<T>(Nest.SearchTemplateDescriptor<T> descriptor)
        {

            return descriptor;
        }
    }
}
