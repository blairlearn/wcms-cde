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
        /// Gets a collection of zero or more custom web analytics variables
        /// </summary>
        [XmlArray(ElementName = "WACustomVariables", Form = XmlSchemaForm.Unqualified)]
        [XmlArrayItem("WACustomVariable", typeof(WebAnalyticsCustomVariableOrEvent), Form = XmlSchemaForm.Unqualified)]
        public WebAnalyticsCustomVariableOrEvent[] WACustomVariables { get; set; }
		
		// Default constructor
		public WebAnalyticsInfo()
        {
			WAChannels = String.Empty;
			WAReportSuites = String.Empty;
			WAContentGroups = String.Empty;
            WAEvents = new WebAnalyticsCustomVariableOrEvent[0];
            WACustomVariables = new WebAnalyticsCustomVariableOrEvent[0];
        }
    }
}