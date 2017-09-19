using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CancerGov.ClinicalTrials.Basic.v2
{
    /// <summary>
    /// Defines the parameters for an At NIH location search
    /// </summary>
    public class AtNIHLocationSearchParams : LocationSearchParams
    {

        /// <summary>
        /// Gets a fields value as a string suitable for things like, oh, a velocity template
        /// </summary>
        /// <param name="field">A FormFields enum value</param>
        /// <returns>The value of the field, OR, and error message</returns>
        public override string GetFieldAsString(FormFields field)
        {
            switch (field)
            {
                default: return "Error Retrieving Field";
            }
        }

        /// <summary>
        /// Creates a new instance of the At NIH Location Search Params
        /// </summary>
        public AtNIHLocationSearchParams()
        {
            this._usedFields = FormFields.AtNIH;
        }
    }
}
