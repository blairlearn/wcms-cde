using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Xml.Serialization;
using System.Xml.Schema;

namespace NCI.Web.CDE.PageAssembly
{
    /// <summary>
    /// This class represents the properties of a page option item.
    /// </summary>
    public class TopicSearch
    {

        /// <summary>
        /// The Content ID for the Topic Search
        /// </summary>
        [XmlAttribute(Form = XmlSchemaForm.Unqualified, AttributeName = "TopicSearchID")]
        public int TopicSearchID { get; set; }

        /// <summary>
        /// The name for the Topic Search.
        /// </summary>
        [XmlElement(Form = XmlSchemaForm.Unqualified)]
        public string TopicSearchName { get; set; }

        /// <summary>
        /// The mesh query that matches to the Topic Search.
        /// </summary>
        [XmlElement(Form = XmlSchemaForm.Unqualified)]
        public string MeshQuery { get; set; }
    }
}
