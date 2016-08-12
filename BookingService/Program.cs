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

                Console.WriteLine("press q to stop");
                while (Console.ReadKey().KeyChar != 'q');

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
