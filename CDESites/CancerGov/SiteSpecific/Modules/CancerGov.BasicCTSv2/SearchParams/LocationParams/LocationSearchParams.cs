using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CancerGov.ClinicalTrials.Basic.v2
{
    /// <summary>
    /// Defines an interface for Location Search Parameters.
    /// </summary>
    public abstract class LocationSearchParams
    {
        /// <summary>
        /// Identifies which fields of the form were used.
        /// </summary>
        protected FormFields _usedFields = FormFields.None;

        /// <summary>
        /// Identified is a field within this location parameter is set.
        /// </summary>
        /// <param name="field">The field to check</param>
        /// <returns>true if set, false if not.</returns>
        public bool IsFieldSet(FormFields field)
        {
            return (_usedFields & field) == field;
        }
    }
}
