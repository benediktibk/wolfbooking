using System;
using System.Configuration;
using Backend.Persistence;
using Backend.Facade;
using Microsoft.AspNet.Identity.Owin;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.Owin.Security;

namespace Backend
{
    public static class Factory
    {
        private static string _databaseConnectionString;

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

        public static WolfBookingSignInManager SignInManager => HttpContext.Current.GetOwinContext().Get<WolfBookingSignInManager>();
        public static WolfBookingUserManager UserManager => HttpContext.Current.GetOwinContext().GetUserManager<WolfBookingUserManager>();
        public static WolfBookingContext DbContext => HttpContext.Current.GetOwinContext().Get<WolfBookingContext>();

        public static RoleManager<WolfBookingRole, int> RoleManager => new RoleManager<WolfBookingRole, int>(new WolfBookingRoleStore(DbContext));


        public static BreadRepository BreadRepository => new BreadRepository(DbContext);
        public static UserRepository UserRepository => new UserRepository(SignInManager, UserManager, DbContext);
        public static RoomRepository RoomRepository => new RoomRepository(DbContext);
        public static BreadBookingsRepository BreadBookingsRepository => new BreadBookingsRepository(DbContext);

        public static BookingFacade BookingFacade => new BookingFacade(BreadRepository, UserRepository, RoomRepository, BreadBookingsRepository);
    }
}
