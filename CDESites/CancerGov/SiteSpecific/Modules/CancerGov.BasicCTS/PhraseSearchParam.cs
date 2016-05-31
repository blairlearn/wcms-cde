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


        public override string GetBody()
        {

            return String.Format(
                @"{{
                        ""from"" : {0},
                        ""size"" : {1},
                        ""query"" : {{
                            ""term"" : {{ ""OfficialTitle"" : ""{2}"" }}
                        }}
                    }}
                ",
                 From,
                 Size,
                 Phrase
            );
        }
    }
}
