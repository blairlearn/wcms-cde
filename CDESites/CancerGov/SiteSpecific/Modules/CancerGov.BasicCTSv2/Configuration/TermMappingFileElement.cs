using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CancerGov.ClinicalTrials.Basic.v2.Configuration
{
    /// <summary>
    /// This class represents a single term mapping file
    /// </summary>
    public class TermMappingFileElement : ConfigurationElement
    {
        /// <summary>
        /// Name of the term mapping file
        /// </summary>
        [ConfigurationProperty("name", IsKey = true, IsRequired = true)]
        public string Name
        {
            get { return (string)base["name"]; }
        }

        /// <summary>
        /// Priority of the term mapping file
        /// </summary>
        [ConfigurationProperty("priority", IsRequired = true)]
        public string Priority
        {
            get { return (string)base["priority"]; }
        }

        /// <summary>
        /// File path for the term mapping file
        /// </summary>
        [ConfigurationProperty("filePath", IsRequired = true)]
        public string FilePath
        {
            get { return (string)base["filePath"]; }
        }

        /// <summary>
        /// 
        /// </summary>
        [ConfigurationProperty("errOnDuplicates", IsRequired = false)]
        public bool ErrOnDuplicates
        {
            get { return (bool)base["errOnDuplicates"]; }
        }
    }
}
