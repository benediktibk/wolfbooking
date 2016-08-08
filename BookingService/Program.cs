using System;
using System.ServiceProcess;

namespace BookingService
{
    static class Program
    {
        static void Main(string[] args)
        {
            var appSettings = System.Configuration.ConfigurationSettings.AppSettings;
            var mailSettings = new MailSettings
            {
                Port = int.Parse(appSettings["port"]),
                UseStartTls = bool.Parse(appSettings["starttls"]),
                Server = appSettings["server"],
                From = appSettings["from"],
                To = appSettings["to"],
                Cc = appSettings["cc"],
                Username = appSettings["username"],
                Password = appSettings["password"]
            };

            if (Environment.UserInteractive)
            {
                var worker = new BookingServiceWorker(mailSettings);
                worker.DoWork();
            }
            else
            {
                var service = new BookingService(mailSettings);
                ServiceBase.Run(new[] { service });
            }
        }
    }
}
