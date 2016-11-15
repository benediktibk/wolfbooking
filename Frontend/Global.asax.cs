using Backend.Persistence;
using NLog;
using Quartz;
using Quartz.Impl;
using System;
using System.Data.Entity;
using System.Web;
using System.Web.Http;
using System.Web.Http.Dispatcher;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

namespace Frontend
{
    public class WebApiApplication : HttpApplication
    {
        private static Logger logger = LogManager.GetCurrentClassLogger();
        private IJobDetail _worker;
        private ITrigger _trigger;
        private IScheduler _scheduler;

        protected void Application_Start()
        {
            Database.SetInitializer(new WolfBookingInitializer());
            GlobalConfiguration.Configure(WebApiConfig.Register);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
            ConfigureMailSchedule();

            // wep api structuremap setup aso http://stackoverflow.com/a/24597695
            HttpConfiguration config = GlobalConfiguration.Configuration;
            config.Services.Replace(typeof(IHttpControllerActivator), new ServiceActivator(config));
        }

        private void ConfigureMailSchedule()
        {
            logger.Debug("creating booking service");
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

            logger.Debug("starting scheduler");
            _scheduler.Start();
            _scheduler.ScheduleJob(_worker, _trigger);
        }
    }
}
