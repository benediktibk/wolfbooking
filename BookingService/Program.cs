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
                var service = new BookingService();
                service.StartScheduler();

                Console.WriteLine("press any key to stop");
                Console.ReadKey();
                service.Stop();
            }
            else
            {
                var service = new BookingService();
                ServiceBase.Run(new[] { service });
            }
        }
    }
}
