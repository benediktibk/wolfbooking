using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookingService
{
    class MailSettings
    {
        public int Port { get; set; }
        public bool UseStartTls { get; set; }
        public string Server { get; set; }
        public string From { get; set; }
        public string To { get; set; }
        public string Cc { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
    }
}
