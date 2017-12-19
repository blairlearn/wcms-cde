using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Schema;
using System.Xml.Serialization;

namespace CancerGov.Dictionaries.Configuration
{
    public class CDRFriendlyNameMappingFile
    {
        /// <summary>
        /// The locale of the CDR to friendly name mapping file
        /// </summary>
        [XmlAttribute(Form = XmlSchemaForm.Unqualified)]
        public string Locale { get; set; }

        /// <summary>
        /// The filepath of the CDR to friendly name mapping file
        /// </summary>
        [XmlAttribute(Form = XmlSchemaForm.Unqualified)]
        public string Filepath { get; set; }

        public CDRFriendlyNameMappingFile()
        {
            this.Locale = String.Empty;
            this.Filepath = String.Empty;
        }
    }
}
