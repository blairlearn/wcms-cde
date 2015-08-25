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
        /// Load the analytics that have been set on the navon. If there is no value,
        /// recurse through parents until a value is found or until root is reached.
        /// </summary>
        /// <param name="section">Section details</param>
        public WebAnalyticsInfo LoadCustomAnalytics(SectionDetail section)
        {
            try
            {
                WebAnalyticsInfo wai = section.WebAnalyticsInfo;
                if (wai == null)
                {
                    if (section.ParentPath != null)
                    {
                        wai = section.Parent.WebAnalyticsInfo;
                        LoadCustomAnalytics(section.Parent);
                    }
                    else return null;
                }
                return wai;
            }
            catch (NullReferenceException ex)
            {
                Logger.LogError("CDE:WebAnalyticsInfo.cs:LoadCustomAnalytics()",
                "SectionDetails.xml not found.", NCIErrorLevel.Error, ex);
                return null;
            }
        }

        protected List<WebAnalyticsInfo> WaiAll = new List<WebAnalyticsInfo> { };
        /// <summary>
        /// Load the analytics that have been set on the navon and all of its parents
        /// until root is reached.
        /// </summary>
        /// <param name="section">Section details</param>
        public List<WebAnalyticsInfo> LoadAllCustomAnalytics(SectionDetail section)
        {
            try
            {
                WaiAll.Add(section.WebAnalyticsInfo);
                if (section.ParentPath != null)
                {
                    LoadAllCustomAnalytics(section.Parent);
                }
                else
                {
                    return null;
                }
                return WaiAll;
            }
            catch (NullReferenceException ex)
            {
                Logger.LogError("CDE:WebAnalyticsInfo.cs:LoadAllCustomAnalytics()",
                "SectionDetails.xml not found.", NCIErrorLevel.Error, ex);
                return null;
            }
        }

        /// <summary>
        /// Load the report suite(s) that have been set on this navon. If there is no value,
        /// recurse through parents until a value is found. Suites set on a loweer folder 
        /// overwrite parents' suites.
        /// </summary>
        /// <param name="section">Section details</param>
        public string LoadSuite(SectionDetail section)
        {
            try
            {
                WebAnalyticsInfo wai = LoadCustomAnalytics(section);
                string suite = wai.WAReportSuites;

                if (String.IsNullOrEmpty(suite))
                {
                    wai = LoadCustomAnalytics(section.Parent);
                    suite = LoadSuite(section.Parent);
                }
                return suite;
            }
            catch (Exception ex)
            {
                Logger.LogError("CDE:WebAnalyticsInfo.cs:LoadSuite()",
                      "Exception encountered while retrieving web analytics suites.",
                      NCIErrorLevel.Warning, ex);
                return "";
            }
        }

        /// <summary>
        /// Load the content group that has been set on this navon.
        /// </summary>
        /// <param name="section">Section details</param>
        public string LoadContentGroup(SectionDetail section)
        {
            try
            {
                WebAnalyticsInfo wai = LoadCustomAnalytics(section);
                string group = wai.WAContentGroups;

                if (String.IsNullOrEmpty(group))
                {
                    group = "";
                }
                return group;
            }
            catch (Exception ex)
            {
                Logger.LogError("CDE:WebAnalyticsInfo.cs:LoadContentGroup()",
                      "Exception encountered while retrieving web analytics content group.",
                      NCIErrorLevel.Debug, ex);
                return "";
            }
        }

        protected List<string> _events = new List<string> { };
        /// <summary>
        /// Load the custom events from the navon and all parents until RemoveParent 
        /// flag is set or root is reached. Lower levels override parents' values.
        /// <param name="section">Section details</param>
        public List<string> LoadEvents(SectionDetail section)
        {
            try
            {
                string key = "";
                bool removeParents = false;

                WaiAll.Clear();
                List<WebAnalyticsInfo> waInfos = LoadAllCustomAnalytics(section);
                foreach (WebAnalyticsInfo waInfo in waInfos)
                {
                    if ((removeParents == false) && (waInfo != null))
                    {
                        WebAnalyticsCustomVariableOrEvent[] waEvents = waInfo.WAEvents;
                        foreach (WebAnalyticsCustomVariableOrEvent waEvent in waEvents)
                        {
                            key = waEvent.Key;
                            _events.Add(key);
                        }
                        removeParents = waInfo.RemoveParentEvents;
                    }
                }
                return _events;
            }
            catch (Exception ex)
            {
                Logger.LogError("CDE:WebAnalyticsInfo.cs:LoadEvents()",
                      "Exception encountered while retrieving web analytics custom props",
                      NCIErrorLevel.Warning, ex);
                return null;
            }
        }


        protected Dictionary<string, string> _props = new Dictionary<string, string> { };
        /// <summary>
        /// Load the custom props set on the navon and all parents until RemoveParent 
        /// flag is set or root is reached. Lower levels override parents' values.
        /// <param name="section">Section details</param>
        public Dictionary<string, string> LoadProps(SectionDetail section)
        {
            try
            {
                string key = "";
                string value = "";
                bool removeParents = false;

                WaiAll.Clear();
                List<WebAnalyticsInfo> waInfos = LoadAllCustomAnalytics(section);
                foreach (WebAnalyticsInfo waInfo in waInfos)
                {
                    if ((removeParents == false) && (waInfo != null))
                    {
                        WebAnalyticsCustomVariableOrEvent[] waProps = waInfo.WAProps;
                        foreach (WebAnalyticsCustomVariableOrEvent waProp in waProps)
                        {
                            key = waProp.Key;
                            value = waProp.Value;
                            if (!_props.ContainsKey(key))
                            {
                                _props.Add(key, value);
                            }
                        }
                        removeParents = waInfo.RemoveParentProps;
                    }
                }
                return _props;
            }
            catch (Exception ex)
            {
                Logger.LogError("CDE:WebAnalyticsInfo.cs:LoadProps()",
                      "Exception encountered while retrieving web analytics custom props",
                      NCIErrorLevel.Warning, ex);
                return null;
            }
        }

        protected Dictionary<string, string> _evars = new Dictionary<string, string> { };
        /// <summary>
        /// Load the custom eVars set on the navon and all parents until RemoveParent 
        /// flag is set or root is reached. Lower levels override parents' values.
        /// <param name="section">Section details</param>
        public Dictionary<string, string> LoadEvars(SectionDetail section)
        {
            try
            {
                string key = "";
                string value = "";
                bool removeParents = false;

                WaiAll.Clear();
                List<WebAnalyticsInfo> waInfos = LoadAllCustomAnalytics(section);
                foreach (WebAnalyticsInfo waInfo in waInfos)
                {
                    if ((removeParents == false) && (waInfo != null))
                    {
                        WebAnalyticsCustomVariableOrEvent[] waEvars = waInfo.WAEvars;
                        foreach (WebAnalyticsCustomVariableOrEvent waEvar in waEvars)
                        {
                            key = waEvar.Key;
                            value = waEvar.Value;
                            if (!_evars.ContainsKey(key))
                            {
                                _evars.Add(key, value);
                            }
                        }
                        removeParents = waInfo.RemoveParentEvars;
                    }
                }
                return _evars;
            }
            catch (Exception ex)
            {
                Logger.LogError("CDE:WebAnalyticsInfo.cs:LoadEvars()",
                      "Exception encountered while retrieving web analytics custom evars",
                      NCIErrorLevel.Warning, ex);
                return null;
            }
        }

        /// <summary>
        /// Get event key if value matches enum in WebAnalyticsOptions
        /// <param name="cEvent">Event key string</param>
        public string GetEventKey(string cEvent)
        {
            string customEvent = "";
            foreach (WebAnalyticsOptions.Events ev in Enum.GetValues(typeof(WebAnalyticsOptions.Events)))
            {
                if (cEvent == ev.ToString())
                    customEvent = cEvent;
            }
            return customEvent;
        }

        /// <summary>
        /// Get prop key/value if key matches enum in WebAnalyticsOptions
        /// <param name="cProp">Prop key/value pair</param>
        public string GetPropKey(KeyValuePair<string, string> cProp)
        {
            string customProp = "";
            foreach (WebAnalyticsOptions.Props prop in Enum.GetValues(typeof(WebAnalyticsOptions.Props)))
            {
                if (cProp.Key == prop.ToString())
                    customProp = cProp.Key;
            }
            return customProp;
        }

        /// <summary>
        /// Get eVar key/value if key matches enum in WebAnalyticsOptions
        /// <param name="cEvar">eVar key/value pair</param>
        public string GetEvarKey(KeyValuePair<string, string> cEvar)
        {
            string customEvar = "";
            foreach (WebAnalyticsOptions.eVars evar in Enum.GetValues(typeof(WebAnalyticsOptions.eVars)))
            {
                if (cEvar.Key == evar.ToString())
                    customEvar = cEvar.Key;
            }
            return customEvar;
        }

       /*
        * TODO:
        * - Update content group functionality (future story)
        */
    } 
}