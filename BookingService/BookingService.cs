using Quartz.Impl;
using Quartz;
using System.ServiceProcess;
using System;
using NLog;

namespace BookingService
{
    class BookingService : ServiceBase
    {
        private static Logger logger = LogManager.GetCurrentClassLogger();
        private IJobDetail _worker;
        private ITrigger _trigger;
        private IScheduler _scheduler;

        public BookingService()
        {
            logger.Debug("creating booking service");
            ServiceName = "Wolfbooking";
            _worker = JobBuilder.Create<BookingServiceWorker>()
                    .WithIdentity("mailsending", "wolfbooking")
                    .Build();


            logger.Debug("loading time settings");
            var appSettings = System.Configuration.ConfigurationSettings.AppSettings;
            var sendingTime = DateTime.Parse(appSettings["sendingtime"]);

            _trigger = TriggerBuilder.Create()
                    .WithIdentity("daily", "wolfbooking")
                    .StartNow()
                    .WithDailyTimeIntervalSchedule(x => x
                        .OnEveryDay()
                        .WithIntervalInHours(24)
                        .StartingDailyAt(TimeOfDay.HourAndMinuteOfDay(sendingTime.Hour, sendingTime.Minute)))
                    .Build();
            _scheduler = StdSchedulerFactory.GetDefaultScheduler();
        }

        public void StartScheduler()
        {
            logger.Debug("starting scheduler");
            _scheduler.Start();
            _scheduler.ScheduleJob(_worker, _trigger);
        }

        protected override void OnStart(string[] args)
        {
            logger.Debug("start of service requested");
            try
            {
                StartScheduler();
            }
            catch (Exception)
            {
                logger.Fatal("start of service failed, stopping again");
                Stop();
            }
            logger.Debug("service started");
        }

        protected override void OnStop()
        {
            logger.Debug("stop of service requested");
            _scheduler.Shutdown();
            logger.Debug("service stopped");
        }
    }
}
