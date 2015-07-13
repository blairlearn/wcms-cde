using System;
using System.Xml.Serialization;
using System.Xml.Schema;
using System.Configuration;
using NCI.Logging;
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
        /// Boolean to determine whether or not to remove the parent value. 
        /// If false, the parent is added to the collection of variables.
        /// If true, the parent is removed.
        /// </summary>
        [XmlAttribute(AttributeName = "RemoveParent", Form = XmlSchemaForm.Unqualified)]
        public Boolean RemoveParent { get; set; }

        // Default constructor
        public WebAnalyticsCustomVariableOrEvent()
        {
            this.Key = String.Empty;
            this.Value = String.Empty;
            this.RemoveParent = false;
        }
    }
}