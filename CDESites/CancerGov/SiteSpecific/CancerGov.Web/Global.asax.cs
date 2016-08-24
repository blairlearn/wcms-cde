using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.SessionState;
using System.IO;
using System.Configuration;

using Quartz;
using Quartz.Impl;

using NCI.Web.CDE;
using NCI.Web.CDE.Configuration;
using NCI.Logging;
using NCI.Search.BestBets.Index;

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
                Application["reloadPromoUrlMappingInfo"] = false;
                monitorPromoUrlMappingFile();
            }
            catch (Exception ex)
            {
                NCI.Logging.Logger.LogError("Monitoring of PromoUrl mapping file could not be established", NCI.Logging.NCIErrorLevel.Error, ex);
            }
            #endregion


            #region Setup Quartz.NET jobs

            if (BestBetsIndex.IndexRebuilderJob.ExecutionSchedule == string.Empty)
            {
                Logger.LogError(this.GetType().ToString(), "BestBets Reindexing Schedule not set.  Skipping QuartzScheduler for Best Bets.", NCI.Logging.NCIErrorLevel.Info);
            }
            else
            {
                try
                {
                    // This schedule stuff should move to a config file...
                    IScheduler scheduler = StdSchedulerFactory.GetDefaultScheduler();
                    scheduler.Start();

                    string TRIGGER_NAME = "BestBetsTrigger";
                    string TRIGGER_GROUP = "BestBetsGroup";

                    //Schedule best bets indexing
                    IJobDetail job = JobBuilder.Create<BestBetsIndex.IndexRebuilderJob>()
                        .WithIdentity(TRIGGER_NAME, TRIGGER_GROUP)
                        .Build();

                    // Create the atual schedule, run according to the schedule specified by the
                    // BestBets IndexRebuilder, and make it eligible to start running immediately.
                    ITrigger trigger = TriggerBuilder.Create()
                        .WithIdentity(TRIGGER_NAME, TRIGGER_GROUP)
                        .WithCronSchedule(BestBetsIndex.IndexRebuilderJob.ExecutionSchedule)
                        .StartNow()
                        .Build();

                    // Add the job to the scheduler.
                    scheduler.ScheduleJob(job, trigger);

                    //Trigger the job for immediate execution.
                    scheduler.TriggerJob(new JobKey(TRIGGER_NAME, TRIGGER_GROUP));
                }
                catch (Exception ex)
                {
                    Logger.LogError(this.GetType().ToString(), "An error occured while setting up QuartzScheduler for Best Bets.", NCI.Logging.NCIErrorLevel.Error, ex);
                }
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
                    //Make sure the response's status code matches the correct response for 
                    //things like search engines.
                    if (objErr is HttpRequestValidationException)
                    {
                        // By default .NET uses 500 for a malformed URL or unsafe request.  This 
                        // is exactly what the 400 status is for.  See:
                        // https://tools.ietf.org/html/rfc7231#section-6.5.1
                        Response.StatusCode = 400;
                    }
                    else
                    {
                        Response.StatusCode = ((HttpException)objErr).GetHttpCode();
                    }                    
                    return;
                }
                else
                {

                    string err = "Error Caught in Application_Error event\n" +
                        "Error in: " + Request.Url.ToString() +
                        "\nError Message:" + objErr.Message.ToString() +
                        "\nStack Trace:" + objErr.ToString();

                    try
                    {
                        NCI.Logging.Logger.LogError("Application Exception", err, NCIErrorLevel.Error);
                    }
                    catch (System.ComponentModel.Win32Exception)
                    { //Since we cannot log to the eventlog, then we should not try again
                    }
                    catch { }
                }
            }

            Server.ClearError();

            //Set the status code so we know some bad mojo happened.
            Response.StatusDescription = "Application Error";
            Response.StatusCode = 500;

            return;
        }

        #endregion
    }
}