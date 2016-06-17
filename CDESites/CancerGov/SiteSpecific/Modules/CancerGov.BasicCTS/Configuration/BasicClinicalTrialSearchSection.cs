using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CancerGov.ClinicalTrials.Basic.Configuration
{
    /// <summary>
    /// Elasticsearch configuration information for the Basic Clinical Trial Search
    /// </summary>
    public class BasicClinicalTrialSearchSection : ConfigurationSection
    {
        /// <summary>
        /// Gets the name of the SearchCluster configuration to use.
        /// </summary>
        [ConfigurationProperty("searchCluster")]
        public string SearchCluster
        {
            get { return (string)base["searchCluster"]; }
        }

        /// <summary>
        /// Gets the name of the search index
        /// </summary>
        [ConfigurationProperty("searchIndex")]
        public string SearchIndex
        {
            get { return (string)base["searchIndex"]; }
        }

        /// <summary>
        /// Gets the type of the trial document.
        /// </summary>
        [ConfigurationProperty("trialIndexType")]
        public string TrialIndexType
        {
            get { return (string)base["trialIndexType"]; }
        }

        /// <summary>
        /// Gets the type of the GeoLocation document.
        /// </summary>
        [ConfigurationProperty("geoLocIndexType")]
        public string GeoLocIndexType
        {
            get { return (string)base["geoLocIndexType"]; }
        }

        /// <summary>
        /// Gets the type of the MenuTerm document.
        /// </summary>
        [ConfigurationProperty("menuTermIndexType")]
        public string MenuTermIndexType
        {
            get { return (string)base["menuTermIndexType"]; }
        }



    }
}
