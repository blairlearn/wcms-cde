using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.SessionState;
using System.IO;
using NCI.Web.CDE;
using NCI.Web.CDE.Configuration;

namespace CancerGov.Web
{
    public class Global : System.Web.HttpApplication
    {
        private FileSystemWatcher fsw;

        protected void Application_Start(object sender, EventArgs e)
        {
            #region Set Promo URl File Monitoring
            try
            {
                monitorPromoUrlMappingFile();
            }
            catch (Exception ex)
            {
                NCI.Logging.Logger.LogError("Monitoring of PromoUrl mapping file could not be established", NCI.Logging.NCIErrorLevel.Error, ex);
            }

            #endregion
        }

        protected void Application_BeginRequest(object sender, EventArgs e)
        {
            monitorPromoUrlMappingFile();
        }

        #region Private Methods
        private void monitorPromoUrlMappingFile()
        {
            if (fsw == null)
            {
                string promoUrlMappingFileAndPath = Context.Server.MapPath(ContentDeliveryEngineConfig.PathInformation.PromoUrlMappingPath.Path);

                if (!string.IsNullOrEmpty(promoUrlMappingFileAndPath))
                {
                    Application.Add("monitor_promoUrlMappingFile", new FileSystemWatcher(Path.GetDirectoryName(promoUrlMappingFileAndPath)));
                    fsw = (FileSystemWatcher)Application["monitor_promoUrlMappingFile"];
                    fsw.EnableRaisingEvents = true;
                    fsw.Filter = Path.GetFileName(promoUrlMappingFileAndPath);

                    // Monitor for file creation and changes.
                    fsw.Changed += new FileSystemEventHandler(OnChanged);
                    fsw.Created += new FileSystemEventHandler(OnChanged);
                }
                else
                    NCI.Logging.Logger.LogError("Global:monitorPromoUrlMappingFile", "ContentDeliveryEngineConfig.PathInformation.PromoUrlMappingPath is empty, cannot set the file monitoring", NCI.Logging.NCIErrorLevel.Error);
            }
        }

        /// <summary>
        /// This event handler , sets a flag of the application state that signifies a new promo url mapping 
        /// information is avaliable. If this flag is true the PromoUrlMappingLoader will load new mapping 
        /// information
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void OnChanged(object sender, FileSystemEventArgs e)
        {
            this.Application.Lock();
            this.Application["reloadPromoUrlMappingInfo"] = true;
            this.Application.UnLock();
        }

        #endregion
    }
}