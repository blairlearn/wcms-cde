using System;
using System.IO;
using Common.Logging;
using NCI.Web.CDE.Configuration;

namespace Imaging.Web
{
    public class Global : System.Web.HttpApplication
    {
        static ILog log = LogManager.GetLogger(typeof(Global));

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
                NCI.Logging.Logger.LogError("Monitoring of PromoUrl mapping file could not be established", NCI.Logging.NCIErrorLevel.Error, ex);
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
                    NCI.Logging.Logger.LogError("Global:monitorPromoUrlMappingFile", "ContentDeliveryEngineConfig.PathInformation.PromoUrlMappingPath is empty, cannot set the file monitoring", NCI.Logging.NCIErrorLevel.Error);
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
            catch (Exception e)
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
            catch (Exception e)
            {
            }
            return "CancerGov";
        }

        protected void Application_Error(object sender, EventArgs e)
        {
            Exception objErr = Server.GetLastError();

            if (objErr != null)
            {

                string err = "Error Caught in Application_Error event\n" +
                    "Error in: " + Request.Url.ToString() +
                    "\nError Message:" + objErr.Message.ToString() +
                    "\nStack Trace:" + objErr.ToString();

                try
                {
                    log.Error(err);
                }
                catch (System.ComponentModel.Win32Exception)
                { //Since we cannot log to the eventlog, then we should not try again
                }
                catch { }
            }

            Server.ClearError();
            string error = Request.Params["TransferredByError"];

            if ((error != null) && (error == "1"))
            {
                //Response.Write("<b>Unexpected errors occurred. Our technicians have been notified and are working to correct the situation.</b>");
            }
            else
            {
                Response.Redirect(System.Configuration.ConfigurationManager.AppSettings["ErrorPage"], true);
            }
        }

        #endregion
    }
}