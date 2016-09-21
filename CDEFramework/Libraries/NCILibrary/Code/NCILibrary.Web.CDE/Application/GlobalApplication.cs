using System;
using System.IO;
using System.Web;
using Common.Logging;
using NCI.Web.CDE.Configuration;

namespace NCI.Web.CDE.Application
{
    public class GlobalApplication : System.Web.HttpApplication
    {
        static ILog log = LogManager.GetLogger(typeof(GlobalApplication));

        private FileSystemWatcher fsw;

        protected void Application_Start(object sender, EventArgs e)
        {
            #region Set Promo URl File Monitoring
            try
            {
                Application["reloadPromoUrlMappingInfo"] = false;
                monitorPromoUrlMappingFile();
            }
            catch (Exception ex)
            {
                log.Error("Monitoring of PromoUrl mapping file could not be established", ex);
            }

            #endregion
        }

        #region Private Methods
        /// <summary>
        /// This method will set up a file watcher on the promo url mapping file to
        /// identify the update to the contents of the file or creation of a new mapping 
        /// file
        /// </summary>
        private void monitorPromoUrlMappingFile()
        {
            if (fsw == null)
            {
                string promoUrlMappingFileAndPath = Context.Server.MapPath(ContentDeliveryEngineConfig.PathInformation.PromoUrlMappingPath.Path);

                if (!string.IsNullOrEmpty(promoUrlMappingFileAndPath))
                {
                    Application.Add("monitor_promoUrlMappingFile", new FileSystemWatcher(Path.GetDirectoryName(promoUrlMappingFileAndPath)));
                    fsw = (FileSystemWatcher)Application["monitor_promoUrlMappingFile"];
                    fsw.NotifyFilter = NotifyFilters.LastWrite | NotifyFilters.CreationTime | NotifyFilters.Size | NotifyFilters.LastAccess | NotifyFilters.Attributes;
                    fsw.EnableRaisingEvents = true;
                    fsw.Filter = Path.GetFileName(promoUrlMappingFileAndPath);

                    // Monitor for file creation and changes.
                    fsw.Changed += new FileSystemEventHandler(OnChanged);
                    fsw.Created += new FileSystemEventHandler(OnChanged);
                    fsw.Deleted += new FileSystemEventHandler(OnPromoUrlFileDeleted);
                }
                else
                    log.Error("monitorPromoUrlMappingFile(): ContentDeliveryEngineConfig.PathInformation.PromoUrlMappingPath is empty, cannot set the file monitoring");
            }
        }

        void OnPromoUrlFileDeleted(object sender, FileSystemEventArgs e)
        {
            this.Application.Lock();
            this.Application["reloadPromoUrlMappingInfo"] = true;
            this.Application.UnLock();
        }

        /// <summary>
        /// This event handler , sets a flag in the application state that signifies a new promo url mapping 
        /// information is avaliable. If this flag is true the PromoUrlMappingLoader will load new mapping 
        /// information from the xml file.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void OnChanged(object sender, FileSystemEventArgs e)
        {
            this.Application.Lock();
            this.Application["reloadPromoUrlMappingInfo"] = true;
            this.Application.UnLock();
        }

        /// <summary>
        /// Get the name of the event log to write to
        /// </summary>
        /// <returns>the name of the event log</returns>
        private static string GetEventLogName()
        {
            try
            {
                if (System.Configuration.ConfigurationManager.AppSettings["EventLogName"] != null)
                    return System.Configuration.ConfigurationManager.AppSettings["EventLogName"];
            }
            catch (Exception)
            {
            }
            return "CancerGov";
        }

        /// <summary>
        /// Get the source name for the event
        /// </summary>
        /// <returns>the source name of the event</returns>
        private static string GetSourceName()
        {
            try
            {
                if (System.Configuration.ConfigurationManager.AppSettings["EventLogSourceName"] != null)
                    return System.Configuration.ConfigurationManager.AppSettings["EventLogSourceName"];
            }
            catch (Exception)
            {
            }
            return "CancerGov";
        }

        protected void Application_Error(object sender, EventArgs e)
        {

            /* In order for the error handling to return the correct codes and display the right
             * pages, the customErrors section of the web.config needs to look like:
                <customErrors mode="On" defaultRedirect="/PublishedContent/ErrorMessages/error.html" redirectMode="ResponseRewrite">
                  <error statusCode="404" redirect="/PublishedContent/ErrorMessages/pagenotfound.html" />
                  <error statusCode="500" redirect="/PublishedContent/ErrorMessages/error.html" />
                </customErrors>            
            */


            Exception objErr = Server.GetLastError();

            if (objErr != null)
            {
                // any thrown exception (including the base HttpException becomes wrapped in an 
                // HttpUnhandledException, so retrieve the inner exception in this case
                if (objErr is HttpUnhandledException)
                {
                    objErr = objErr.InnerException;
                }

                if (objErr is HttpException && !(
                    objErr is HttpCompileException ||
                    objErr is HttpParseException ||
                    objErr is HttpRequestValidationException ||
                    objErr is HttpUnhandledException
                    ))
                {
                    // Retrieve the HttpException's status code
                    int statusCode = ((HttpException)objErr).GetHttpCode();

                    ErrorPageDisplayer.RaisePageByCode(this.GetType().ToString(), statusCode);

                    return;
                }
                else if (objErr is HttpRequestValidationException)
                {
                    // By default .NET uses 500 for a malformed URL or unsafe request.  This 
                    // is exactly what the 400 status is for.  See:
                    // https://tools.ietf.org/html/rfc7231#section-6.5.1
                    ErrorPageDisplayer.RaisePageByCode(this.GetType().ToString(), 400);

                    // log validation errors even though we generate a 400
                    try
                    {
                        log.ErrorFormat("HttpRequestValidationException Caught in Application_Error event\nError in: {0}", objErr, Request.Url.ToString());
                    }
                    catch (System.ComponentModel.Win32Exception)
                    { //Since we cannot log to the eventlog, then we should not try again
                    }
                    catch { }

                    return;
                }
                else
                {
                    try
                    {
                        log.ErrorFormat("Error Caught in Application_Error event\nError in: {0}", objErr, Request.Url.ToString());
                    }
                    catch (System.ComponentModel.Win32Exception)
                    { //Since we cannot log to the eventlog, then we should not try again
                    }
                    catch { }
                }
            }

            ErrorPageDisplayer.RaisePageError(this.GetType().ToString());
        }

        #endregion
    }
}