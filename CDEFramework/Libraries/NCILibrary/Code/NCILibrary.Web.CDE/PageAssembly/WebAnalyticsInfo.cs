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
    public class WebAnalyticsInfo : BasePageAssemblyInstruction
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
        public WebAnalyticsInfo LoadCustomAnalytics(SectionDetail section)
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

        /// <summary>
        /// Create list of WebAnalyticsInfo objects for current item and all ancestors
        /// </summary>
        protected List<WebAnalyticsInfo> WaiAll = new List<WebAnalyticsInfo> { };
        protected List<WebAnalyticsInfo> LoadAllCustomAnalytics(SectionDetail section)
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

        /// <summary>
        /// Load the report suite(s) that have been set on this navon. If there is no value,
        /// recurse through parents until a value is found. Suites set on a loweer folder 
        /// overwrite parents' suites.
        /// </summary>
        protected string LoadSuite(SectionDetail section)
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
                Logger.LogError("CDE:SinglePageAssemblyInstruction.cs:LoadSuite()",
                      "Exception encountered while retrieving web analytics suites.",
                      NCIErrorLevel.Error, ex);
                return "";
            }
        }

        /// <summary>
        /// Load the content group that has been set on this navon.
        /// </summary>
        protected string LoadContentGroup(SectionDetail section)
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
                Logger.LogError("CDE:SinglePageAssemblyInstruction.cs:LoadContentGroup()",
                      "Exception encountered while retrieving web analytics content group.",
                      NCIErrorLevel.Error, ex);
                return "";
            }
        }


        protected List<string> _events = new List<string> { };
        protected List<string> LoadEvents(SectionDetail section)
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
                Logger.LogError("CDE:SinglePageAssemblyInstruction.cs:LoadProps()",
                      "Exception encountered while retrieving web analytics custom props",
                      NCIErrorLevel.Error, ex);
                return null;
            }
        }


        protected Dictionary<string, string> _props = new Dictionary<string, string> { };
        protected Dictionary<string, string> LoadProps(SectionDetail section)
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
                Logger.LogError("CDE:SinglePageAssemblyInstruction.cs:LoadProps()",
                      "Exception encountered while retrieving web analytics custom props",
                      NCIErrorLevel.Error, ex);
                return null;
            }
        }

        protected Dictionary<string, string> _evars = new Dictionary<string, string> { };
        protected Dictionary<string, string> LoadEvars(SectionDetail section)
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
                Logger.LogError("CDE:SinglePageAssemblyInstruction.cs:LoadEvars()",
                      "Exception encountered while retrieving web analytics custom evars",
                      NCIErrorLevel.Error, ex);
                return null;
            }
        }

        public string GetCustomEvents(string cEvent)
        {
            string customEvent = "";
            foreach (WebAnalyticsOptions.Events ev in Enum.GetValues(typeof(WebAnalyticsOptions.Events)))
            {
                if (cEvent == ev.ToString())
                    customEvent = cEvent;
            }
            return customEvent;
        }


        public string GetCustomProps(KeyValuePair<string, string> cProp)
        {
            string customProp = "";
            foreach (WebAnalyticsOptions.Props prop in Enum.GetValues(typeof(WebAnalyticsOptions.Props)))
            {
                if (cProp.Key == prop.ToString())
                    customProp = cProp.Key;
            }
            return customProp;
        }


        public string GetCustomEvars(KeyValuePair<string, string> cEvar)
        {
            string customEvar = "";
            foreach (WebAnalyticsOptions.eVars evar in Enum.GetValues(typeof(WebAnalyticsOptions.eVars)))
            {
                if (cEvar.Key == evar.ToString())
                    customEvar = cEvar.Key;
            }
            return customEvar;
        }

        public void RegisterCustomWebAnalytics(SectionDetail detail)
        {

            // Get the section details for the content item, then load any custom analytics
            // values from it or its parents
            WebAnalyticsInfo wai = LoadCustomAnalytics(detail);
            string suite = LoadSuite(detail);
            string group = LoadContentGroup(detail);
            List<String> eventsList = LoadEvents(detail);
            Dictionary<string, string> props = LoadProps(detail);
            Dictionary<string, string> evars = LoadEvars(detail);


            try
            {
                foreach (string evn in eventsList)
                {

                    if (!String.IsNullOrEmpty(evn))
                    {
                        SetWebAnalytics(evn, waField =>
                        {
                            waField.Value = "";
                        });
                    }
                }


                // Register custom props entered on navon
                foreach (KeyValuePair<string, string> pr in props)
                {
                    String propKey = GetCustomProps(pr);
                    String propValue = pr.Value;

                    if (!String.IsNullOrEmpty(propKey))
                    {
                        SetWebAnalytics(propKey, waField =>
                        {
                            waField.Value = propValue;
                        });
                    }
                }

                // Register custom evars entered on navon
                foreach (KeyValuePair<string, string> evr in evars)
                {
                    String evarKey = GetCustomEvars(evr);
                    String evarValue = evr.Value;

                    if (!String.IsNullOrEmpty(evarKey))
                    {
                        SetWebAnalytics(evarKey, waField =>
                        {
                            waField.Value = evarValue;
                        });
                    }
                }
            }
            catch (NullReferenceException ex)
            {
                Logger.LogError("SinglePageAssemblyInstruction.cs:RegisterWebAnalyticsFieldFilters()",
                    "WebAnalyticsInfo is missing from SectionDetails XML", NCIErrorLevel.Error, ex);
                return;
            }
        } 

    } 
}