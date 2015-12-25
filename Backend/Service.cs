using System;
using System.Threading;

namespace Backend
{
    public class Service
    {
        Thread _worker;
        volatile bool _stopRequested;

        public Service()
        {
            _stopRequested = false;
            _worker = new Thread(Function);
        }

        public delegate void FinishedEventHandler();
        public event FinishedEventHandler OnFinished;

        public void Start()
        {
            _worker.Start();
        }

        public void RequestStop()
        {
            _stopRequested = true;
        }

        private void Function()
        {
            for (var i = 0; i < 100 && !_stopRequested; ++i)
            {
                Console.WriteLine($"{i}/100");
                Thread.Sleep(100);
            }

            if (OnFinished != null)
                OnFinished();
        }
    }
}
