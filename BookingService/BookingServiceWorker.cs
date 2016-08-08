using System;
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
                // Replace the Sleep() call with the work you need to do
                Thread.Sleep(1000);
                Console.WriteLine("blub");
            }

        }
    }
}
