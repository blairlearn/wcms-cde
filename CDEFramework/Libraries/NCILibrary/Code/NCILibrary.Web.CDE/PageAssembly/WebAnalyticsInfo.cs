using System;
using System.Xml.Serialization;
using System.Xml.Schema;
using System.Configuration;
using NCI.Logging;
namespace NCI.Web.CDE
{
    /// <summary>
    /// Defines custom web analytics values set on folders or content types.
    /// </summary>
    public class WebAnalyticsInfo
    {
        /// <summary>
        /// Gets comma-separated list of analytics channel namess
        /// </summary>
        [XmlElement(Form = XmlSchemaForm.Unqualified)]
        public string WAChannels { get; set; }

        /// <summary>
        /// Gets a comma-separated list of analytics report suites
        /// </summary>
        [XmlElement(Form = XmlSchemaForm.Unqualified)]
        public string WAReportSuites { get; set; }

        /// <summary>
        /// Gets a string of content groups ("buckets")
        /// </summary>
        [XmlElement(Form = XmlSchemaForm.Unqualified)]
        public string WAContentGroups { get; set; }

        /// <summary>
        /// Gets a collection of zero or more custom web analytics events
        /// </summary>
        [XmlArray(ElementName = "WAEvents", Form = XmlSchemaForm.Unqualified)]
        [XmlArrayItem("WAEvent", typeof(WebAnalyticsCustomVariableOrEvent), Form = XmlSchemaForm.Unqualified)]
        public WebAnalyticsCustomVariableOrEvent[] WAEvents { get; set; }

        /// <summary>
        /// Gets a collection of zero or more custom web analytics props
        /// </summary>
        [XmlArray(ElementName = "WAProps", Form = XmlSchemaForm.Unqualified)]
        [XmlArrayItem("WAProp", typeof(WebAnalyticsCustomVariableOrEvent), Form = XmlSchemaForm.Unqualified)]
        public WebAnalyticsCustomVariableOrEvent[] WAProps { get; set; }

        /// <summary>
        /// Gets a collection of zero or more custom web analytics evars
        /// </summary>
        [XmlArray(ElementName = "WAEvars", Form = XmlSchemaForm.Unqualified)]
        [XmlArrayItem("WAEvar", typeof(WebAnalyticsCustomVariableOrEvent), Form = XmlSchemaForm.Unqualified)]
        public WebAnalyticsCustomVariableOrEvent[] WAEvars { get; set; }

        /// <summary>
        /// Boolean to determine whether or not to remove the parent value. 
        /// If false, the parent is added to the collection of events.
        /// If true, the parent is removed.
        /// </summary>
        [XmlElement(Form = XmlSchemaForm.Unqualified)]
        public bool RemoveParentEvents { get; set; }

        /// <summary>
        /// Boolean to determine whether or not to remove the parent value. 
        /// If false, the parent is added to the collection of props.
        /// If true, the parent is removed.
        /// </summary>
        [XmlElement(Form = XmlSchemaForm.Unqualified)]
        public bool RemoveParentProps { get; set; }

        /// <summary>
        /// Boolean to determine whether or not to remove the parent value. 
        /// If false, the parent is added to the collection of evars.
        /// If true, the parent is removed.
        /// </summary>
        [XmlElement(Form = XmlSchemaForm.Unqualified)]
        public bool RemoveParentEvars { get; set; }

        // Default constructor
        public WebAnalyticsInfo()
        {
            WAChannels = String.Empty;
            WAReportSuites = String.Empty;
            WAContentGroups = String.Empty;
            WAEvents = new WebAnalyticsCustomVariableOrEvent[0];
            WAProps = new WebAnalyticsCustomVariableOrEvent[0];
            WAEvars = new WebAnalyticsCustomVariableOrEvent[0];
            RemoveParentEvents = false;
            RemoveParentProps = false;
            RemoveParentEvars = false;
        }
    }
}