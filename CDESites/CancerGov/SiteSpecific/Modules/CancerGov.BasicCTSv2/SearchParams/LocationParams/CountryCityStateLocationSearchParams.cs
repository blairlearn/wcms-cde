using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CancerGov.ClinicalTrials.Basic.v2
{
    /// <summary>
    /// Defines the parameters for a Country/City/State location search
    /// </summary>
    public class CountryCityStateLocationSearchParams : LocationSearchParams
    {

        string _country = string.Empty;
        LabelledSearchParam[] _state = { };
        string _city = string.Empty;        

        /// <summary>
        /// Gets or sets the country used in the search
        /// </summary>
        public String Country
        {
            get { return _country; }
            set { _country = value; _usedFields |= FormFields.Country; }
        }

        /// <summary>
        /// Gets or sets the state used in the search
        /// </summary>
        public LabelledSearchParam[] State {
            get { return _state; }
            set { _state = value; _usedFields |= FormFields.State; } 
        }

        /// <summary>
        /// Gets or sets the city used in the search
        /// </summary>
        public String City
        {
            get { return _city; }
            set { _city = value; _usedFields = FormFields.City; }
        }

    }
}
