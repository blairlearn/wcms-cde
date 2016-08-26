using System;
using NCI.Logging;
using NCI.Search.BestBets.Index;
using NCI.Web.CDE.Application;
using Quartz;
using Quartz.Impl;

namespace CancerGov.Web
{
    public class Global : GlobalApplication
    {
        protected void Application_Start(object sender, EventArgs e)
        {
            base.Application_Start(sender, e);

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
    }
}