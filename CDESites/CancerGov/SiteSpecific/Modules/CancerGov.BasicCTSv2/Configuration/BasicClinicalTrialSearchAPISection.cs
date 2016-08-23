using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CancerGov.ClinicalTrials.Basic.v2.Configuration
{
    /// <summary>
    /// ClinicalTrialsAPI configuration information for the Basic Clinical Trial Search
    /// </summary>
    public class BasicClinicalTrialSearchAPISection : ConfigurationSection
    {
        /// <summary>
        /// Gets the host name of the ClinicalTrialsAPI server.
        /// </summary>
        [ConfigurationProperty("apiHost", IsRequired=true)]
        public string APIHost
        {
            get { return (string)base["apiHost"]; }
        }

        /// <summary>
        /// Gets the API version 
        /// </summary>
        [ConfigurationProperty("apiVersion", IsRequired=true)]
        public string APIVersion
        {
            get { return (string)base["apiVersion"]; }
        }

        /// <summary>
        /// Gets the port number for the API
        /// </summary>
        [ConfigurationProperty("apiPort", IsRequired=false)]
        public string APIPort
        {
            get { return (string)base["apiPort"]; }
        }

        /// <summary>
        /// Gets the protocol for the API
        /// </summary>
        [ConfigurationProperty("apiProtocol", IsRequired = true)]
        public string APIProtocol
        {
            get { return (string)base["apiProtocol"]; }
        }

    }
}
