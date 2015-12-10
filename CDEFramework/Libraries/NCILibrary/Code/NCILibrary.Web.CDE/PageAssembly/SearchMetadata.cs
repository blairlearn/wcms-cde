using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Schema;
using System.Xml.Serialization;

namespace NCI.Web.CDE
{
    /// <summary>
    /// Contains Search Metadata for a SinglePageAssemblyInstruction
    /// </summary>
    public class SearchMetadata
    {
        /// <summary>
        /// Should this page be available for searching or not.
        /// </summary>
        [XmlElement(Form = XmlSchemaForm.Unqualified)]
        public bool DoNotIndex { get; set; }

        //SiteSpecificSearchMetadata is another method of unknown data type,
        //not implementing this as part of the DoNotIndex/Meta Robots implementation

        public SearchMetadata()
        {
            //Setup Default Value if not in XML.
            DoNotIndex = false;
        }
    }
}
