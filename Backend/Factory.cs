using System;
using System.Configuration;
using Backend.Business;
using Backend.Persistence;
using Backend.Facade;

namespace Backend
{
    public static class Factory
    {
        private static string _databaseConnectionString;
        private static BreadRepository _breadRepository;
        private static BreadFactory _breadFactory;
        private static BookingFacade _bookingFacade;

        public static string DatabaseConnectionString
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

        public static BreadRepository BreadRepository
        {
            get
            {
                if (_breadRepository == null)        
                    _breadRepository = new BreadRepository(DatabaseConnectionString);

                return _breadRepository;
            }
        }

        public static BreadFactory BreadFactory
        {
            get
            {
                if (_breadFactory == null)
                    _breadFactory = new BreadFactory(BreadRepository);

                return _breadFactory;
            }
        }

        public static BookingFacade BookingFacade
        {
            get
            {
                if (_bookingFacade == null)
                    _bookingFacade = new BookingFacade(BreadFactory, BreadRepository);

                return _bookingFacade;
            }
        }

        public static WolfBookingContext CreateWolfBookingContext()
        {
            return new WolfBookingContext(DatabaseConnectionString);
        }
    }
}
