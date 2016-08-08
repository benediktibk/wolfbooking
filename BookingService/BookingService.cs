using System.ServiceProcess;
using System.Threading;

namespace BookingService
{
    class BookingService : ServiceBase
    {
        private BookingServiceWorker _worker;
        private Thread _thread;

        public BookingService(MailSettings mailSettings)
        {
            ServiceName = "Wolfbooking";
            _worker = new BookingServiceWorker(mailSettings);
            _thread = new Thread(_worker.DoWork);
        }

        public void Run()
        {
            _thread.Start();
        }

        protected override void OnStart(string[] args)
        {
            _thread.Start();
        }

        protected override void OnStop()
        {
            _worker.Stop();

            // wait 3 seconds for the thread to stop
            if (!_thread.Join(3000))
                _thread.Abort();
        }
    }
}
