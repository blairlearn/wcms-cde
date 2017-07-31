using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CancerGov.ClinicalTrials.Basic.v2
{
    /// <summary>
    /// Defines the parameters for a Zip Code location search
    /// </summary>
    public class ZipCodeLocationSearchParams : LocationSearchParams
    {
        int _zipRadius = 100;

        /// <summary>
        /// Gets or sets the zip code value
        /// TODO: verify how this should work with updated API
        /// </summary>
        public String ZipCode { get; set; }

        /// <summary>
        /// Gets or sets the zip code search radius
        /// TODO: verify how this should work with updated API
        /// </summary>
        public int ZipRadius
        {
            get { return _zipRadius; }
            set { _zipRadius = value; } //Don't set as used field because it is really zipcode that matters.
        }

    }
}
