using System;
using System.ServiceProcess;
using System.Threading;

namespace Backend
{
    class Backend : ServiceBase
    {
        Service _service;
        EventWaitHandle _finishedWaitHandle;

        public Backend()
        {
            _service = new Service();
            _finishedWaitHandle = new AutoResetEvent(false);
            _service.OnFinished += ServiceFinished;
        }

        static void Main(string[] arguments)
        {
            var backend = new Backend();
            backend.TestStartupAndStop(arguments);
        }

        public void TestStartupAndStop(string[] arguments)
        {
            OnStart(arguments);
            Console.ReadKey();
            OnStop();
        }

        private void ServiceFinished()
        {
            _finishedWaitHandle.Set();
        }

        protected override void OnStart(string[] args)
        {
            _service.Start();
        }

        protected override void OnStop()
        {
            _service.RequestStop();
            _finishedWaitHandle.WaitOne();
        }
    }
}
