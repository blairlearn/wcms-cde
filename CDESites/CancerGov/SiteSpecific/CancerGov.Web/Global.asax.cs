﻿using System;
using System.Net;
using Common.Logging;
using NCI.Search.BestBets.Index;
using NCI.Web.CDE.Application;
using Quartz;
using Quartz.Impl;

namespace CancerGov.Web
{
    public class Global : GlobalApplication
    {
        static ILog log = LogManager.GetLogger(typeof(Global));

        protected override void SiteSpecificAppStart(object sender, EventArgs e)
        {

            //Setting to allow TLS 1.1 & 1.2 for HttpClient in addition the default 4.5.X TLS 1.0.
            //NOTE: This is supposed to be set for the AppDomain, however setting this in application start
            //This affects all connections.
            if ((ServicePointManager.SecurityProtocol & SecurityProtocolType.Tls12) != SecurityProtocolType.Tls12)
            {
                ServicePointManager.SecurityProtocol = ServicePointManager.SecurityProtocol | SecurityProtocolType.Tls12;
            }

            if ((ServicePointManager.SecurityProtocol & SecurityProtocolType.Tls11) != SecurityProtocolType.Tls11)
            {
                ServicePointManager.SecurityProtocol = ServicePointManager.SecurityProtocol | SecurityProtocolType.Tls11;
            }


            #region Setup Quartz.NET jobs

            if (BestBetsIndex.IndexRebuilderJob.ExecutionSchedule == string.Empty)
            {
                log.Info("BestBets Reindexing Schedule not set.  Skipping QuartzScheduler for Best Bets.");
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
                    log.Error("An error occured while setting up QuartzScheduler for Best Bets.", ex);
                }
            }

            #endregion            
        }
    }
}