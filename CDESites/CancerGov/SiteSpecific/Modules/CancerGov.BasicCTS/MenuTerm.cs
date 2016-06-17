using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CancerGov.ClinicalTrials.Basic
{
    public class MenuTerm
    {
        /// <summary>
        /// Gets and sets the CDRID for this Menu Term
        /// </summary>
        public string CDRID { get; set; }

        /// <summary>
        /// Gets and sets a unique id for this name and CDRID, used for finding the right display name on results
        /// </summary>
        public string Hash { get; set; }

        /// <summary>
        /// Gets and sets the name of this Menu Term
        /// </summary>
        public string Name { get; set; }
    }
}
