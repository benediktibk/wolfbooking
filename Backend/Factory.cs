using System;
using System.Configuration;
using Backend.Business;
using Backend.Persistence;
using Backend.Facade;

namespace Backend
{
    public class Factory
    {
        private string _databaseConnectionString;
        private BreadRepository _breadRepository;
        private BreadFactory _breadFactory;
        private BookingFacade _bookingFacade;

        public string DatabaseConnectionString
        {
            get
            {
                if (_databaseConnectionString == null)
                {
                    var hostName = Environment.MachineName;
                    _databaseConnectionString = ConfigurationManager.ConnectionStrings[hostName].ConnectionString;
                }

                return _databaseConnectionString;
            }
        }

        public BreadRepository BreadRepository
        {
            get
            {
                if (_breadRepository == null)        
                    _breadRepository = new BreadRepository(DatabaseConnectionString);

                return _breadRepository;
            }
        }

        public BreadFactory BreadFactory
        {
            get
            {
                if (_breadFactory == null)
                    _breadFactory = new BreadFactory(BreadRepository);

                return _breadFactory;
            }
        }

        public BookingFacade BookingFacade
        {
            get
            {
                if (_bookingFacade == null)
                    _bookingFacade = new BookingFacade(BreadFactory);

                return _bookingFacade;
            }
        }
    }
}
