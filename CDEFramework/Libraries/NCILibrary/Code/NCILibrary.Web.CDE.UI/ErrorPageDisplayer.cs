using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;
using System.Web;
using System.Web.Configuration;

namespace NCI.Web.CDE.UI
{
    /// <summary>
    /// This is a helper class for wrangling all of the various implementations of RaisePageError
    /// </summary>
    public static class ErrorPageDisplayer
    {

        /// <summary>
        /// Display the "ErrorPage" page and a status of 500.
        /// </summary>
        /// <param name="callingClass">The name of the class that called this - for logging.</param>
        public static void RaisePageError(string callingClass)
        {
            /*
             * The original code found in AppsBaseUserControlreferenced a SystemMessagePage  
             * AppSetting that cannot befound anywhere in our configs.  This seems to be 
             * referenced only in this place.  Currently on production causing an error that 
             * would enter this code results in a blank page.  So the original code was actually broken.
             */

            //Set the status code so we know some bad mojo happened.
            HttpContext.Current.Response.StatusDescription = "Application Error";
            HttpContext.Current.Response.StatusCode = 500;

            string errorPageURL = ConfigurationSettings.AppSettings["ErrorPage"];
            bool hasDisplayedMessage = false;

            try
            {
                if (errorPageURL != null && errorPageURL.Trim() != "")
                {                   
                    HttpContext.Current.Server.Execute(errorPageURL);
                    hasDisplayedMessage = true;
                }
                else
                {
                    Logging.Logger.LogError(callingClass, "Error page URL is null or empty", NCI.Logging.NCIErrorLevel.Warning);
                }
            }
            catch (Exception ex)
            {
                Logging.Logger.LogError(callingClass, "Could not display error page.", NCI.Logging.NCIErrorLevel.Warning, ex);
            }

            if (!hasDisplayedMessage)
            {
                //There is no way to show the error page, so just show this message instead.
                HttpContext.Current.Response.Write("Errors Occurred");
            }
        }
    }
}
