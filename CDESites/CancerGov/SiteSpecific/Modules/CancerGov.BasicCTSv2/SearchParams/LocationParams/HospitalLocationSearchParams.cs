using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CancerGov.ClinicalTrials.Basic.v2
{
    /// <summary>
    /// Defines the parameters for a Hospital location search
    /// </summary>
    public class HospitalLocationSearchParams : LocationSearchParams
    {
        string _hospital = string.Empty;

        /// <summary>
        /// Gets a fields value as a string suitable for things like, oh, a velocity template
        /// </summary>
        /// <param name="field">A FormFields enum value</param>
        /// <returns>The value of the field, OR, and error message</returns>
        public override string GetFieldAsString(FormFields field)
        {
            switch (field)
            {
                case FormFields.Hospital: return Hospital;
                default: return "Error Retrieving Field";
            }
        }

        /// <summary>
        /// Gets or sets the Hospital used in the search
        /// </summary>
        public String Hospital
        {
            get { return _hospital; }
            set { _hospital = value; _usedFields |= FormFields.Hospital; }
        }
    }
}
