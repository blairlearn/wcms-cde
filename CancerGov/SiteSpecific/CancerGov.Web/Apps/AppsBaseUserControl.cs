using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Text.RegularExpressions;
using System.Web.UI.HtmlControls;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Text;
using System.Configuration;
using System.Xml;
using System.Globalization;
using NCI.Util;
using NCI.Web.CDE.WebAnalytics;
using NCI.Web.CDE;
using NCI.Web.CDE.UI;

namespace NCI.Web.CancerGov.Apps
{
    public class AppsBaseUserControl : SnippetControl
    {
        private WebAnalyticsPageLoad webAnalyticsPageLoad = new WebAnalyticsPageLoad();
        protected DisplayInformation pageDisplayInformation;
        private string strHelpPageLink = "#";

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            pageDisplayInformation = new DisplayInformation();
            switch(PageInstruction.Language)
            {
                case "en":
                    pageDisplayInformation.Language = DisplayLanguage.English;
                    break;
                case "es":
                    pageDisplayInformation.Language = DisplayLanguage.Spanish;
                    break;
                default:
                    pageDisplayInformation.Language = DisplayLanguage.English;
                    break;
            }
            pageDisplayInformation.Version  = PageAssemblyContext.Current.DisplayVersion;
        }

        virtual protected string GetResource(string key)
        {
            if( string.IsNullOrEmpty(key) )
                return "";
            object localizedObject = this.GetGlobalResourceObject("SiteWideSearch", key);
            if (localizedObject == null )
                return "key: " + key + " not localized";
            string val = localizedObject as string;
            if (string.IsNullOrEmpty(val))
                return key;
            return localizedObject as string;
        }

        public WebAnalyticsPageLoad WebAnalyticsPageLoad
        {
            get { return webAnalyticsPageLoad; }
            set { webAnalyticsPageLoad = value; }
        }

        public DisplayInformation PageDisplayInformation
        {
            get { return pageDisplayInformation; }
            set { pageDisplayInformation = value; }
        }

        virtual public void RaiseErrorPage(string messageKey)
        {
            string systemMessagePageUrl = ConfigurationSettings.AppSettings["SystemMessagePage"].Trim();

            if (systemMessagePageUrl.Substring(systemMessagePageUrl.Length - 1, 1) != "?")
                systemMessagePageUrl += "?";

            systemMessagePageUrl += "msg=" + messageKey.Trim();
            Response.Redirect(systemMessagePageUrl, true);
        }

        public string CurrentPageUrl
        {
            get { return this.Request.RawUrl; }
        }

        public string HelpPageLink
        {
            get { return strHelpPageLink; }
            set { strHelpPageLink = value; }
        }
    }
}