using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;

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
            // The purpose of matching the entered phrase to this pattern is so that certain 
            // search phrases with only one item in quotes (such as "nci-match", as opposed 
            // to breast cancer) are passed along with a parameter to ES for a specific search.
            string pattern = @"^""[^""]+""$";

            // Set the searchstring only if we have one.  Maybe clean it up too if needbe.
            if (!String.IsNullOrWhiteSpace(this.Phrase))
            {
                if (Regex.IsMatch(this.Phrase, pattern))
                {
                    // Add in parameter without strings, along with an extra parameter that
                    // specifies that the search string was entered with quotes.
                    paramdict.Add("searchstring", this.Phrase.Replace("\"", ""));
                    paramdict.Add("searchtype", "phrase");
                }
                else
                {
                    paramdict.Add("searchstring", this.Phrase.Replace("\"", ""));
                }
            }
            
        }

        protected override Nest.SearchTemplateDescriptor<T> ModifySearchParams<T>(Nest.SearchTemplateDescriptor<T> descriptor)
        {

            return descriptor;
        }
    }
}
