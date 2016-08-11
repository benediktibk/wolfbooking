using NLog;
using Quartz;
using System.ComponentModel;
using System.Net.Mail;

namespace BookingService
{
    class BookingServiceWorker : IJob
    {
        private static Logger logger = LogManager.GetCurrentClassLogger();
        private MailSettings _mailSettings;

        public BookingServiceWorker()
        {
            logger.Debug("loading mail settings");
            var appSettings = System.Configuration.ConfigurationSettings.AppSettings;
            _mailSettings = new MailSettings
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
        }

        public void Execute(IJobExecutionContext context)
        {
            logger.Info("sending an order");
            SmtpClient client = new SmtpClient();
            client.Port = _mailSettings.Port;
            client.EnableSsl = _mailSettings.UseStartTls;
            client.UseDefaultCredentials = false;
            client.Credentials = new System.Net.NetworkCredential(_mailSettings.Username, _mailSettings.Password);
            client.DeliveryMethod = SmtpDeliveryMethod.Network;
            client.Host = _mailSettings.Server;

            MailMessage mail = new MailMessage(_mailSettings.From, _mailSettings.To);
            mail.CC.Add(_mailSettings.Cc);
            mail.Subject = "This is another very import mail";
            mail.Body = "This is a very import mail, please don't delete it.";

            client.SendCompleted += (object sender, AsyncCompletedEventArgs e) =>
            {
                if (e.Error != null)
                    logger.Error($"errors occured while trying to send an order: {e.Error.ToString()}");
            };

            client.SendAsync(mail, null);
        }
    }
}
