using System;
using System.ServiceProcess;

namespace BookingService
{
    static class Program
    {
        static void Main(string[] args)
        {
            if (Environment.UserInteractive)
            {
                var worker = new BookingServiceWorker();
                worker.DoWork();
            }
            else
            {
                var service = new BookingService();
                ServiceBase.Run(new[] { service });
            }
        }
    }
}
