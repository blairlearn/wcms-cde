using System;
using System.Configuration;
using System.Web;

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
            // raise a not found page
            RaisePage(callingClass, "Application Error", 500, "ErrorPage");
        }

        /// <summary>
        /// Display the "NotFoundPage" page and a status of 404.
        /// </summary>
        /// <param name="callingClass">The name of the class that called this - for logging.</param>
        public static void RaiseClinicalTrialsIdNotFound(string callingClass)
        {
            // raise a not found page
            RaisePage(callingClass, "Clinical Trials ID Not Found", 404, "ClinicalTrialInvalidSearchID");
        }

        /// <summary>
        /// Display the Invalid Clinical Trials Search Id page and a status of 404.
        /// </summary>
        /// <param name="callingClass">The name of the class that called this - for logging.</param>
        public static void RaisePageNotFound(string callingClass)
        {
            // raise a not found page
            RaisePage(callingClass, "Page Not Found", 404, "NotFoundPage");
        }

        /// <summary>
        /// Display a page using an arbitrary configuration string and repsonse code.
        /// </summary>
        /// <param name="callingClass">the calling class name - for logging purposes</param>
        /// <param name="pageDescription">the description of the page, used in the response</param>
        /// <param name="responseCode">the status code of the response</param>
        /// <param name="configurationKey">the name of the key containing the page URL in application settings</param>
        private static void RaisePage(string callingClass, string pageDescription, int responseCode, string configurationKey)
        {
            // Set the status code and description
            HttpContext.Current.Response.StatusDescription = pageDescription;
            HttpContext.Current.Response.StatusCode = responseCode;
            HttpContext.Current.Response.TrySkipIisCustomErrors = true;

            string pageUrl = System.Configuration.ConfigurationManager.AppSettings[configurationKey];
            bool hasDisplayedMessage = false;

            try
            {
                if (pageUrl != null && pageUrl.Trim() != "")
                {
                    HttpContext.Current.Server.Execute(pageUrl);
                    hasDisplayedMessage = true;
                }
                else
                {
                    Logging.Logger.LogError(callingClass, pageDescription + " URL is null or empty", NCI.Logging.NCIErrorLevel.Warning);
                }
            }
            catch (Exception ex)
            {
                Logging.Logger.LogError(callingClass, "Could not display '" + pageDescription + "' page.", NCI.Logging.NCIErrorLevel.Warning, ex);
            }

            if (!hasDisplayedMessage)
            {
                //There is no way to show the page, so just show the description message instead.
                HttpContext.Current.Response.Write(pageDescription);
            }
        }
    }
}
