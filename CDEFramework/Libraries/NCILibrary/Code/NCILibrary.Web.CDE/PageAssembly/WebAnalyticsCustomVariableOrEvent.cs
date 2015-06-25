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
        [XmlAttribute(Form = XmlSchemaForm.Unqualified)]
        public string Key { get; set; }

        /// <summary>
        /// String value of variable
        /// </summary>
        [XmlAttribute(Form = XmlSchemaForm.Unqualified)]
        public string Value { get; set; }

        /// <summary>
        /// Boolean to determine whether or not to remove the parent value. 
		/// If true, the parent is added to the collection of variables.
		/// If false, it is removed.
        /// </summary>
        [XmlAttribute(Form = XmlSchemaForm.Unqualified)]
        public boolean RemoveParent { get; set; }

		// Default constructor
        public WebAnalyticsCustomVariableOrEvent()
        {
            this.Locale = String.Empty;
            this.Id = String.Empty;
            this.RemoveParent = true;
        }
    }
}