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

        /// <summary>
        /// Load the report suite(s) that have been set on this navon. 
        /// </summary>
        /// <param name="infos">WebAnalyticsInfo</param>
        /// <returns></returns>
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
        /// <returns></returns>
        public static String GetChannels(IEnumerable<WebAnalyticsInfo> infos)
        {
            // Loop through infos until a report channel value is found. 
            foreach (WebAnalyticsInfo info in infos)
            {
                if (!string.IsNullOrEmpty(info.WAChannels))
                {
                    return info.WAReportSuites;
                }
            }
            return "";
        }

        /// <summary>
        /// Load the content groups that have been set on this navon. 
        /// </summary>
        /// <param name="infos">WebAnalyticsInfo</param>
        /// <returns></returns>
        public static String GetContentGroup(WebAnalyticsInfo info)
        {
            string group = "";
            if (!string.IsNullOrEmpty(info.WAContentGroups))
            {
                group = info.WAContentGroups;
            }
            return group;
        }

        /// <summary>
        /// Get the events from a collection of WebAnalyticsInfos.  This assumes that the input is a flattened
        /// tree where the first item is the current item and the last item is the root ancestor. 
        /// </summary>
        /// <param name="infos"></param>
        /// <returns></returns>
        public static IEnumerable<String> GetEvents(IEnumerable<WebAnalyticsInfo> infos)
        {
            List<string> seenID = new List<string>();

            //Loop through infos in order ... more comment here
            foreach (WebAnalyticsInfo info in infos)
            {
                foreach (WebAnalyticsCustomVariableOrEvent evt in info.WAEvents)
                {
                    //Put comment here
                    if (!seenID.Contains(evt.Key))
                    {
                        seenID.Add(evt.Key);
                        yield return evt.Key;
                    }
                }

                // If we are to remove the parent events we need to stop looping
                if (info.RemoveParentEvents)
                    break;
            }
        }

        /// <summary>
        /// Get the Props from a collection of WebAnalyticsInfos. This assumes that the input is a flattened
        /// tree where the first item is the current item and the last item is the root ancestor. 
        /// </summary>
        /// <param name="infos">collection of WebAnalyticsInfos</param>
        /// <returns></returns>
        public static IEnumerable<WebAnalyticsCustomVariableOrEvent> GetProps(IEnumerable<WebAnalyticsInfo> infos)
        {
            List<string> seenID = new List<string>();

            //Loop through infos in order ... more comment here
            foreach (WebAnalyticsInfo info in infos)
            {
                foreach (WebAnalyticsCustomVariableOrEvent prop in info.WAProps)
                {
                    // Check the list of seen IDs; if this key does not appear on the list, add it.
                    // Do not add if the key already exists - child keys override parents, and this loop
                    // starts at the current content item and moves through parents until the root or "removeParents" is hit.                    if (!seenID.Contains(prop.Key))
                    {
                        seenID.Add(prop.Key);
                        yield return prop;
                    }
                }

                // If we are to remove the parent events we need to stop looping
                if (info.RemoveParentProps)
                    break;
            }
        }

        /// <summary>
        /// Get the eVars from a collection of WebAnalyticsInfos. This assumes that the input is a flattened
        /// tree where the first item is the current item and the last item is the root ancestor. 
        /// </summary>
        /// <param name="infos">collection of WebAnalyticsInfos</param>
        /// <returns></returns>
        public static IEnumerable<WebAnalyticsCustomVariableOrEvent> GetEvars(IEnumerable<WebAnalyticsInfo> infos)
        {
            List<string> seenID = new List<string>();

            //Loop through infos in order ... more comment here
            foreach (WebAnalyticsInfo info in infos)
            {
                foreach (WebAnalyticsCustomVariableOrEvent evar in info.WAEvars)
                {
                    // Check the list of seen IDs; if this key does not appear on the list, add it.
                    // Do not add if the key already exists - child keys override parents, and this loop
                    // starts at the current content item and moves through parents until the root or "removeParents" is hit.
                    if (!seenID.Contains(evar.Key))
                    {
                        seenID.Add(evar.Key);
                        yield return evar;
                    }
                }

                // If we are to remove the parent events we need to stop looping
                if (info.RemoveParentEvars)
                    break;
            }
        }

    }
}