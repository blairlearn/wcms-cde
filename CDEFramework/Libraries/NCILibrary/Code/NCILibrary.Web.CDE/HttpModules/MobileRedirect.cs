using System;
using System.Web;
using System.Globalization;
using System.Configuration;
using System.IO;
using NCI.Web.CDE.Configuration;
using NCI.Web.CDE.CapabilitiesDetection;
using NCI.Logging;
using System.Reflection;

namespace NCI.Web.CDE
{
    public class MobileRedirect : IHttpModule
    {
        private static readonly object REQUEST_URL_KEY = new object();

        /// <summary>
        /// You will need to configure this module in the web.config file of your
        /// web and register it with IIS before being able to use it. For more information
        /// see the following link: http://go.microsoft.com/?linkid=8101007
        /// </summary>
        #region IHttpModule Members

        public void Dispose()
        {
            //clean-up code here.
        }
        /// <summary>
        /// Instantiated  events OnBeginRequest and OnLogRequest
        /// </summary>
        /// <param name="context"></param>
        public void Init(HttpApplication context)
        {
            context.BeginRequest += new EventHandler(OnBeginRequest);
            
            // Handle LogRequest event and provide custom logging implementation
            //context.LogRequest += new EventHandler(OnLogRequest);
        }

        void OnBeginRequest(object sender, EventArgs e)
        {

            if (PageAssemblyContext.Current.PageAssemblyInstruction != null)
            {
               
                // if mobile device and mobile url exist, redirect to it
                if( (DisplayDeviceDetector.DisplayDevice == DisplayDevices.BasicMobile) ||
                    (DisplayDeviceDetector.DisplayDevice == DisplayDevices.AdvancedMobile) )
                 {

                    HttpContext context = ((HttpApplication)sender).Context;
                    string[] alternateContentVersionsKeys;
                    //NciUrl mUrl = PageAssemblyContext.Current.PageAssemblyInstruction.GetUrl("mobileurl");

                    alternateContentVersionsKeys = PageAssemblyContext.Current.PageAssemblyInstruction.AlternateContentVersionsKeys;

                    if (Array.Find(alternateContentVersionsKeys,
                                    element => element.StartsWith("mobileurl", StringComparison.Ordinal)) == "mobileurl")
                    {

                        NciUrl mobileUrl = PageAssemblyContext.Current.PageAssemblyInstruction.GetUrl("mobileurl");
                        context.Response.Redirect(mobileUrl.ToString(), true);
                    }
                }
            }
       }

        void OnLogRequest(object sender, EventArgs e)
        {
            //New comments for prototype.  So if there are multiple PrettyUrl modules, then this would
            //not be good to put here -- it should be in its own module.


            //IIS 7's logging is slightly different from IIS 6.  IIS 6 would log the prettyurl because
            //logging is not part of the asp.net pipeline and therefore the url of the GET header is what 
            //is logged.  
            //IIS 7's integrated mode logs the Request object's url.  Maybe the AbsoluteUrl, who knows,
            //but, since we do a RewritePath, the Request object's url is now the ugly url and that is
            //what is logged.  So what we can do at this stage is do a RewritePath back to the
            //orgininal requested url if the requested url was a pretty url.  Then the pretty url will
            //get logged and everyone is happy.  This should not affect anything since the page handler
            //has already run and rendered the content to the Response object.
            //SCR 30301.

            HttpContext context = ((HttpApplication)sender).Context;

            string url = string.Empty;

            if (context.Items.Contains(REQUEST_URL_KEY))
            {
                url = (string)context.Items[REQUEST_URL_KEY];

                if (context.Items.Contains(REQUEST_URL_KEY) && !string.IsNullOrEmpty((string)context.Items[REQUEST_URL_KEY]))
                    url += (string)context.Items[REQUEST_URL_KEY];
                else
                    url += "?";
            }

            if (!string.IsNullOrEmpty(url))
            {
                //context.RewritePath(url, false);
            }
        }


        private void RaiseErrorPage(string errMessage, Exception ex)
        {
            HttpContext.Current.Response.Write("There was an error processing the Request");
            if (DisplayErrorOnScreen)
            {
                HttpContext.Current.Response.Write("<br/><br/>Error Message: " + errMessage);
                if (ex != null)
                    HttpContext.Current.Response.Write("<br/><br/>Exception Message: " + ex.ToString());
            }
            HttpContext.Current.Response.End();
            return;
        }


        #endregion
        #region Private Members
        private bool DisplayErrorOnScreen
        {
            get
            {
                string displayErrorOnScreen =
                    System.Configuration.ConfigurationManager.AppSettings["DisplayErrorOnScreen"];

                if (string.IsNullOrEmpty(displayErrorOnScreen))
                    return false;

                bool flag = false;
                if (bool.TryParse(displayErrorOnScreen, out flag))
                    return flag;
                else
                    return false;
            }
        }
        #endregion
    }
}
