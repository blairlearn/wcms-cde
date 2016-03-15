using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NCI.Search.Configuration
{
    /// <summary>
    /// This class has the details of the field elements
    /// </summary>
    public class FieldElement : ConfigurationElement
    {
        /// <summary>
        /// Name of the field element
        /// </summary>
        [ConfigurationProperty("name")]
        public string FieldName
        {
            get { return (string)base["name"]; }
        }

    }
}
