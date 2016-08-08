using System;
using System.ComponentModel;
using System.Net.Mail;
using System.Threading;

namespace BookingService
{
    class BookingServiceWorker
    {
        private ManualResetEvent _stop;

        public BookingServiceWorker()
        {
            _stop = new ManualResetEvent(false);
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
                client.Port = 25;
                client.EnableSsl = true;
                client.UseDefaultCredentials = false;
                client.Credentials = new System.Net.NetworkCredential("depp.huber@yahoo.com", "Test1234!");
                client.DeliveryMethod = SmtpDeliveryMethod.Network;
                client.Host = "smtp.mail.yahoo.com";

                MailMessage mail = new MailMessage("depp.huber@yahoo.com", "benediktibk@outlook.com");
                mail.Subject = "This is a very import mail";
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
