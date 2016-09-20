using System;
using System.Xml.Schema;
using System.Xml.Serialization;

namespace NCI.Web.CDE
{
    /// <summary>
    /// Defines a single Event, eVar, or Prop element.
    /// </summary>
    public class WebAnalyticsCustomVariableOrEvent
    {

        /// <summary>
        /// Unique variable ID
        /// </summary>
        [XmlAttribute(AttributeName = "Key", Form = XmlSchemaForm.Unqualified)]
        public string Key { get; set; }

        /// <summary>
        /// String value of variable
        /// </summary>
        [XmlAttribute(AttributeName = "Value", Form = XmlSchemaForm.Unqualified)]
        public string Value { get; set; }

        /// <summary>
        /// String description of variable (optional)
        /// </summary>
        [XmlAttribute(AttributeName = "Description", Form = XmlSchemaForm.Unqualified)]
        public string Description { get; set; }


        // Default constructor
        public WebAnalyticsCustomVariableOrEvent()
        {
            this.Key = String.Empty;
            this.Value = String.Empty;
            this.Description = String.Empty;
        }
    }
}