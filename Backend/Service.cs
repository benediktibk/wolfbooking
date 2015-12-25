using Backend.Persistence;
using System;
using System.Threading;
using System.Configuration;
using NLog;
using Backend.Business;

namespace Backend
{
    public class Service
    {
        Thread _worker;
        volatile bool _stopRequested;
        BreadRepository _breadRepository;
        BreadFactory _breadFactory;
        private Logger _logger;

        public Service()
        {
            _stopRequested = false;
            _worker = new Thread(Function);
            _logger = LogManager.GetCurrentClassLogger();
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
            Initialize();

            //_breadRepository.Add(new Bread { Name = "Schwarzbrot", Price = 2.2m });

            var breads = _breadFactory.GetCurrentAvailableBreads();

            foreach (var bread in breads)
                Console.WriteLine(bread.Name);

            if (OnFinished != null)
                OnFinished();
        }

        private void Initialize()
        {
            var databaseConnectionString = GetDatabaseConnectionString();
            _breadRepository = new BreadRepository(databaseConnectionString);
            _breadFactory = new BreadFactory(_breadRepository);
        }

        private string GetDatabaseConnectionString()
        {
            var hostName = Environment.MachineName;
            string databaseConnectionString;

            try
            {
                databaseConnectionString = ConfigurationManager.ConnectionStrings[hostName].ConnectionString;
            }
            catch (NullReferenceException e)
            {
                _logger.Fatal($"could not find database connection string for host {hostName} in App.config");
                throw e;
            }

            return databaseConnectionString;
        }
    }
}
