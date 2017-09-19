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
        /// Gets a fields value as a string suitable for things like, oh, a velocity template
        /// </summary>
        /// <param name="field">A FormFields enum value</param>
        /// <returns>The value of the field, OR, and error message</returns>
        public abstract string GetFieldAsString(FormFields field);

        /// <summary>
        /// Identified is a field within this location parameter is set.  This is a helper for Velocity that
        /// does not understand enums.
        /// </summary>
        /// <param name="field">The field to check using the String representation of its name</param>
        /// <returns>true if set, false if not.</returns>
        public bool IsFieldSet(string fieldName)
        {
            FormFields field = (FormFields)Enum.Parse(typeof(FormFields), fieldName, true);
            return IsFieldSet(field);
        }

        /// <summary>
        /// Identified is a field within this location parameter is set.
        /// </summary>
        /// <param name="field">The field to check</param>
        /// <returns>true if set, false if not.</returns>
        public bool IsFieldSet(FormFields field)
        {
            return (_usedFields & field) == field;
        }

        /// <summary>
        /// Gets a fields value as a string suitable for things like, oh, a velocity template
        /// </summary>
        /// <param name="fieldName">The string representation of a FormFields enum value</param>
        /// <returns>The value of the field, OR, and error message</returns>
        public string GetFieldAsString(string fieldName)
        {
            try
            {
                FormFields field = (FormFields)Enum.Parse(typeof(FormFields), fieldName, true);
                return GetFieldAsString(field);
            }
            catch (Exception)
            {
                return "Error Retrieving Field";
            }

        }

    }
}
