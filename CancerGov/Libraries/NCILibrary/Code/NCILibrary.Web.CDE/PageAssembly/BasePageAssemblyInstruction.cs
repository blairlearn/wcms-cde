using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Globalization;
using NCI.Web.CDE.WebAnalytics;

namespace NCI.Web.CDE
{
    /// <summary>
    /// Abstract base class for both single page and multiple page classes to 
    /// encapsulate common members and functionality.
    /// </summary>
    abstract public class BasePageAssemblyInstruction
    {
        #region Private 
        /// <summary>
        /// Collection of FieldFilter delegates for Web analytics fields.
        /// </summary>
        private Dictionary<string, WebAnalyticsDataPointDelegate> _webAnalyticsFieldFilterDelegates = new Dictionary<string, WebAnalyticsDataPointDelegate>();
        private WebAnalyticsSettings webAnalyticsSettings = null;
        #endregion

        #region Private Methods
        /// <summary>
        /// This private method registers all delegates for web analytics.
        /// </summary>
        /// <param name="webAnalyticFieldName">The key or the name of the datapoint</param>
        /// <param name="filter">The actual delegate callback which will modify the FieldFilterData object</param>
        protected void SetWebAnalytics(string webAnalyticFieldName, WebAnalyticsDataPointDelegate filter)
        {
            if (string.IsNullOrEmpty(webAnalyticFieldName))
                throw new ArgumentException("The webAnalyticFieldName parameter may not be null or empty.");

            string fieldNameKey = webAnalyticFieldName;

            if (_webAnalyticsFieldFilterDelegates.ContainsKey(fieldNameKey) == false)
                _webAnalyticsFieldFilterDelegates.Add(fieldNameKey, filter);
            else
                _webAnalyticsFieldFilterDelegates[fieldNameKey] += filter;
        }
        #endregion

        #region Protected Members
        protected WebAnalyticsSettings WebAnalyticsSettings
        {
            get 
            {
                if (webAnalyticsSettings == null)
                    webAnalyticsSettings = new WebAnalyticsSettings();
                return webAnalyticsSettings;
            }
        }
        #endregion

        #region Protected Methods
        /// <summary>
        /// This method returns the web analytics settings for Event, Props and eVars data points.
        /// </summary>
        public virtual WebAnalyticsSettings GetWebAnalytics()
        {
            // Enumerate _webAnalyticsFieldFilterDelegates , so each delagate can be executed.
            foreach (KeyValuePair<string, WebAnalyticsDataPointDelegate> kvDel in _webAnalyticsFieldFilterDelegates)
            {
                FieldFilterData fieldData = null;
                if (Enum.IsDefined(typeof(WebAnalyticsOptions.Events), kvDel.Key))
                {
                    fieldData = new FieldFilterData();
                    // Execute the delegate so we can get the value of the field
                    kvDel.Value(fieldData);
                    this.WebAnalyticsSettings.Events.Add((WebAnalyticsOptions.Events)Enum.Parse(typeof(WebAnalyticsOptions.Events), kvDel.Key), fieldData.Value);
                }
                else if (Enum.IsDefined(typeof(WebAnalyticsOptions.eVars), kvDel.Key))
                {
                    fieldData = new FieldFilterData();
                    kvDel.Value(fieldData);
                    this.WebAnalyticsSettings.Evars.Add((WebAnalyticsOptions.eVars)Enum.Parse(typeof(WebAnalyticsOptions.eVars), kvDel.Key), fieldData.Value);
                }
                else if (Enum.IsDefined(typeof(WebAnalyticsOptions.Props), kvDel.Key))
                {
                    fieldData = new FieldFilterData();
                    kvDel.Value(fieldData);
                    this.WebAnalyticsSettings.Props.Add((WebAnalyticsOptions.Props)Enum.Parse(typeof(WebAnalyticsOptions.Props), kvDel.Key), fieldData.Value);
                }
            }

            return this.WebAnalyticsSettings;
        }

        /// <summary>
        /// Register all site wide field filters for Web Analytics.
        /// </summary>
        protected virtual void RegisterWebAnalyticsFieldFilters()
        {
            SetWebAnalytics(WebAnalyticsOptions.Props.RootPrettyURL.ToString(), wbField =>
            {
                wbField.Value = "";
            });

            //SetWebAnalytics(WebAnalyticsOptions.eVars.Language.ToString(), wbField =>
            //{
            //    string languageValue = CultureInfo.CurrentUICulture.TwoLetterISOLanguageName;
            //    switch (languageValue.ToLower())
            //    {
            //        case "en":
            //            wbField.Value = "english";
            //            break;
            //        case "es":
            //            wbField.Value = "spanish";
            //            break;
            //        default:
            //            wbField.Value = "english";
            //            break;
            //    }
            //});
        }
        #endregion

        #region Public 
        public ContentDates ContentDates
        {
            get
            {
                ContentDates contentDates = null;
                if (this is SinglePageAssemblyInstruction)
                    contentDates = ((SinglePageAssemblyInstruction)this).ContentDates;
                else if (this is MultiPageAssemblyInstruction)
                    contentDates = ((MultiPageAssemblyInstruction)this).ContentDates;
                return contentDates;
            }
        }

        #endregion
    }
}
