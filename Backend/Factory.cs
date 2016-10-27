﻿using System;
using System.Configuration;
using Backend.Persistence;
using Backend.Facade;

namespace Backend
{
    public static class Factory
    {
        private static string _databaseConnectionString;
        private static WolfBookingContextFactory _contextFactory;
        private static BreadRepository _breadRepository;
        private static UserRepository _userRepository;
        private static RoleRepository _roleRepository;
        private static RoomRepository _roomRepository;
        private static BreadBookingsRepository _breadBookingsRepository;
        private static BookingFacade _bookingFacade;

        public static string DatabaseConnectionString
        {
            get
            {
                if (_databaseConnectionString == null)
                {
                    var hostName = Environment.MachineName;
                    _databaseConnectionString = ConfigurationManager.ConnectionStrings[hostName].ConnectionString;
                    //_databaseConnectionString = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
                }

                return _databaseConnectionString;
            }
        }

        public static WolfBookingContextFactory WolfBookingContextFactory
        {
            get
            {
                if (_contextFactory == null)
                    _contextFactory = new WolfBookingContextFactory(DatabaseConnectionString);

                return _contextFactory;
            }
        }

        public static BreadRepository BreadRepository
        {
            get
            {
                if (_breadRepository == null)        
                    _breadRepository = new BreadRepository(WolfBookingContextFactory);

                return _breadRepository;
            }
        }

        public static UserRepository UserRepository
        {
            get
            {
                if (_userRepository == null)
                    _userRepository = new UserRepository(WolfBookingContextFactory);

                return _userRepository;
            }
        }

        public static RoleRepository RoleRepository
        {
            get
            {
                if (_roleRepository == null)
                    _roleRepository = new RoleRepository(WolfBookingContextFactory);

                return _roleRepository;
            }
        }

        public static RoomRepository RoomRepository
        {
            get
            {
                if (_roomRepository == null)
                    _roomRepository = new RoomRepository(WolfBookingContextFactory);

                return _roomRepository;
            }
        }

        public static BreadBookingsRepository BreadBookingsRepository
        {
            get
            {
                if (_breadBookingsRepository == null)
                    _breadBookingsRepository = new BreadBookingsRepository(WolfBookingContextFactory);

                return _breadBookingsRepository;
            }
        }

        public static BookingFacade BookingFacade
        {
            get
            {
                if (_bookingFacade == null)
                    _bookingFacade = new BookingFacade(BreadRepository, UserRepository, RoleRepository, RoomRepository, BreadBookingsRepository);

                return _bookingFacade;
            }
        }
    }
}
