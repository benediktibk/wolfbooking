using System;
using System.ComponentModel;
using System.Net.Mail;
using System.Threading;

namespace BookingService
{
    class BookingServiceWorker
    {
        private ManualResetEvent _stop;
        private MailSettings _mailSettings;

        public BookingServiceWorker(MailSettings mailSettings)
        {
            _stop = new ManualResetEvent(false);
            _mailSettings = mailSettings;
        }

        public void Stop()
        {
            _stop.Set();
        }

        public void DoWork()
        {
            while (!_stop.WaitOne(0))
            {
                Console.WriteLine("trying to send a mail");

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
                        Console.WriteLine($"errors: {e.Error.ToString()}");
                };

                client.SendAsync(mail, null);

                Thread.Sleep(100000);
            }
        }
    }
}
