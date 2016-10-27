using Backend;
using Backend.Facade;
using NLog;
using Quartz;
using System.Collections.Generic;
using System.ComponentModel;
using System.Net.Mail;
using System.Text;
using System;

namespace Frontend
{
    class BookingServiceWorker : IJob
    {
        private static Logger logger = LogManager.GetCurrentClassLogger();
        private MailSettings _mailSettings;
        private BookingFacade _bookingFacade;

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
            _bookingFacade = Factory.BookingFacade;
        }

        public void Execute(IJobExecutionContext context)
        {
            try
            {
                logger.Info("sending an order");

                var breadBookings = _bookingFacade.GetCurrentBreadBookingsForAllRooms();
                var breads = _bookingFacade.GetCurrentAvailableBreads();

                var mailContent = CreateMailContent(breads, breadBookings);
                logger.Info($"mail content: {mailContent}");
                SendMail(mailContent);
                logger.Info("setting the bookings as ordered");
                SetBookingsAsOrdered(breadBookings);
            }
            catch (Exception e)
            {
                logger.Error($"error occured: {e}");
            }
        }

        private void SetBookingsAsOrdered(IList<BreadBookings> breadBookings)
        {
            foreach (var breadBooking in breadBookings)
            {
                var success = _bookingFacade.UpdateBreadBookingsAndSetAsOrdered(breadBooking);

                if (!success)
                    logger.Error($"couldn't set booking {breadBooking.Id} as ordered");
            }
        }

        private static string CreateMailContent(IList<Bread> breads, IList<BreadBookings> breadBookings)
        {
            var stringBuilder = new StringBuilder();
            var breadAmountsById = new Dictionary<int, int>();

            foreach (var bread in breads)
                breadAmountsById.Add(bread.Id, 0);

            foreach (var breadBooking in breadBookings)
            {
                foreach (var booking in breadBooking.Bookings)
                    breadAmountsById[booking.Bread] += booking.Amount;
            }

            stringBuilder.AppendLine($"Hallo Reini,<br/><br/>ich möchte Dich gerne für morgen, den {DateTime.UtcNow.AddDays(1):dd.MM.yyyy}, um folgende Lieferung bitten:<br/>");
            stringBuilder.AppendLine("<table>");
            stringBuilder.AppendLine("\t<th>");
            stringBuilder.AppendLine("\t\t<td>Menge</td>");
            stringBuilder.AppendLine("\t\t<td>Brot</td>");
            stringBuilder.AppendLine("\t</th>");

            foreach (var bread in breads)
            {
                stringBuilder.AppendLine("\t<tr>");
                stringBuilder.AppendLine($"\t\t<td>{breadAmountsById[bread.Id]}</td>");
                stringBuilder.AppendLine($"\t\t<td>{bread.Name}</td>");
                stringBuilder.AppendLine("\t</tr>");
            }

            stringBuilder.AppendLine("</table>");
            stringBuilder.AppendLine("<br/>Danke &amp; liebe Grüße aus Ladis<br/>Familie Doris und Georg Wolf");
            return stringBuilder.ToString();
        }

        private void SendMail(string content)
        {
            SmtpClient client = new SmtpClient();
            client.Port = _mailSettings.Port;
            client.EnableSsl = _mailSettings.UseStartTls;
            client.UseDefaultCredentials = false;
            client.Credentials = new System.Net.NetworkCredential(_mailSettings.Username, _mailSettings.Password);
            client.DeliveryMethod = SmtpDeliveryMethod.Network;
            client.Host = _mailSettings.Server;

            MailMessage mail = new MailMessage(_mailSettings.From, _mailSettings.To);
            mail.IsBodyHtml = true;
            mail.CC.Add(_mailSettings.Cc);
            mail.Subject = "Brot Bestellung";
            mail.Body = content;

            client.SendCompleted += (object sender, AsyncCompletedEventArgs e) =>
            {
                if (e.Error == null)
                    logger.Info("successfully sent the mail");
                else
                    logger.Error($"errors occured while trying to send an order: {e.Error.ToString()}");
            };

            client.SendAsync(mail, null);
        }
    }
}
