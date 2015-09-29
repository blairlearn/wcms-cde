using System;
using System.Collections;
using System.Collections.Specialized;
using System.Collections.Generic;
using System.Xml.Serialization;
using System.Xml.Schema;
using System.Configuration;
using NCI.Logging;
using NCI.Web.CDE.WebAnalytics;

namespace NCI.Web.CDE
{
    /// <summary>
    /// Defines custom web analytics values set on folders or content types.
    /// </summary>
    public class WebAnalyticsInfo
    {
        /// <summary>
        /// Default constructor to ensure Events, Props, and eVars are not null
        /// </summary>
        public WebAnalyticsInfo()
        {
            this.WAEvents = new WebAnalyticsCustomVariableOrEvent[] { };
            this.WAProps = new WebAnalyticsCustomVariableOrEvent[] { };
            this.WAEvars = new WebAnalyticsCustomVariableOrEvent[] { };
        }

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
        /// Boolean to determine whether or not to remove the parent values
        /// of Events, Props, and eVars. 
        /// If false, the parent is added.
        /// If true, the parent is removed.
        /// </summary>
        [XmlElement(Form = XmlSchemaForm.Unqualified)]
        public bool RemoveParents { get; set; }

        /// <summary>
        /// Load the report suite(s) that have been set for this navon. 
        /// </summary>
        /// <param name="infos">WebAnalyticsInfo</param>
        /// <returns>Report suite string</returns>
        public static String GetSuites(IEnumerable<WebAnalyticsInfo> infos)
        {
            // Loop through infos until a report suite value is found. 
            foreach (WebAnalyticsInfo info in infos)
            {
                if (!string.IsNullOrEmpty(info.WAReportSuites))
                {
                    return info.WAReportSuites;
                }
            }
            return "";
        }

        /// <summary>
        /// Load the WA channel(s) that have been set on this navon. 
        /// </summary>
        /// <param name="infos">WebAnalyticsInfo</param>
        /// <returns>Channels string</returns>
        public static String GetChannels(IEnumerable<WebAnalyticsInfo> infos)
        {
            // Loop through infos until a report channel value is found. 
            foreach (WebAnalyticsInfo info in infos)
            {
                if (!string.IsNullOrEmpty(info.WAChannels))
                {
                    return info.WAChannels;
                }
            }
            return "";
        }

        /// <summary>
        /// Load the content groups that have been set on this navon. 
        /// </summary>
        /// <param name="infos">WebAnalyticsInfo</param>
        /// <returns>Content group string</returns>
        public static String GetContentGroups(IEnumerable<WebAnalyticsInfo> infos)
        {
            // Loop through infos until a Conteng Group value is found. 
            foreach (WebAnalyticsInfo info in infos)
            {
                if (!string.IsNullOrEmpty(info.WAContentGroups))
                {
                    return info.WAContentGroups;
                }
            }
            return "";
        }

        /// <summary>
        /// Determine if this WebAnalyticsInfo has a prop, evar, or event set
        /// </summary>
        /// <returns></returns>
        private bool HasPropEvarEvt()
        {
            return (this.WAEvars.Length + this.WAEvents.Length + this.WAProps.Length) > 0;
        }

        /// <summary>
        /// Get the events from a collection of WebAnalyticsInfos.  This assumes that the input is a flattened
        /// tree where the first item is the current item and the last item is the root ancestor. 
        /// </summary>
        /// <param name="infos">collection of WebAnalyticsInfos</param>
        /// <returns>Collection of event keys (string)</returns>
        public static IEnumerable<String> GetEvents(IEnumerable<WebAnalyticsInfo> infos)
        {
            // Loop through infos in order, starting at the current folder level and working through each successive
            // parent until either the site root or "RemoveParent" is hit.
            foreach (WebAnalyticsInfo info in infos)
            {
                if (info.HasPropEvarEvt())
                {
                    foreach (WebAnalyticsCustomVariableOrEvent evt in info.WAEvents)
                    {
                        yield return evt.Key;
                    }
                    break;
                }
                // If we are to remove the parent props we need to stop looping
                if (info.RemoveParents)
                    break;
            }
        }

        /// <summary>
        /// Get the Props from a collection of WebAnalyticsInfos. This assumes that the input is a flattened
        /// tree where the first item is the current item and the last item is the root ancestor. 
        /// </summary>
        /// <param name="infos">collection of WebAnalyticsInfos</param>
        /// <returns>collection of WebAnalyticCustomVariableOrEvents</returns>
        public static IEnumerable<WebAnalyticsCustomVariableOrEvent> GetProps(IEnumerable<WebAnalyticsInfo> infos)
        {
            // Loop through infos in order, starting at the current folder level and working through each successive
            // parent until either the site root or "RemoveParent" is hit.
            foreach (WebAnalyticsInfo info in infos)
            {
                if (info.HasPropEvarEvt())
                {
                    foreach (WebAnalyticsCustomVariableOrEvent prop in info.WAProps)
                    {
                        yield return prop;
                    }
                    break;
                }
                // If we are to remove the parent props we need to stop looping
                if (info.RemoveParents)
                    break;
            }
        }

        /// <summary>
        /// Get the eVars from a collection of WebAnalyticsInfos. This assumes that the input is a flattened
        /// tree where the first item is the current item and the last item is the root ancestor. 
        /// </summary> 
        /// <param name="infos">collection of WebAnalyticsInfos</param>
        /// <returns>collection of WebAnalyticCustomVariableOrEvents</returns>
        public static IEnumerable<WebAnalyticsCustomVariableOrEvent> GetEvars(IEnumerable<WebAnalyticsInfo> infos)
        {
            // Loop through infos in order, starting at the current folder level and working through each successive
            // parent until either the site root or "RemoveParent" is hit.
            foreach (WebAnalyticsInfo info in infos)
            {
                if (info.HasPropEvarEvt())
                {
                    foreach (WebAnalyticsCustomVariableOrEvent evar in info.WAEvars)
                    {
                        yield return evar;
                    }
                    break;
                }
                // If we are to remove the parent props we need to stop looping
                if (info.RemoveParents)
                    break;
            }
        } // end GetEvars()

    } // end WebAnalyticsInfo
}