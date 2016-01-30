using Backend.Persistence;
using System.Data.Entity;

namespace Backend
{
    public class DatabaseConfiguration : DbConfiguration
    {
        public DatabaseConfiguration()
        {
            SetContextFactory(typeof(WolfBookingContext), () => { return Factory.WolfBookingContextFactory.Create(); });
        }
    }
}
