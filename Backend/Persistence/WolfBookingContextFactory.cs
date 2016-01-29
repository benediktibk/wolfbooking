using System.Data.Entity.Infrastructure;

namespace Backend.Persistence
{
    public class WolfBookingContextFactory : IDbContextFactory<WolfBookingContext>
    {
        private string _databaseConnectionString;

        public WolfBookingContextFactory(string databaseConnectionString)
        {
            _databaseConnectionString = databaseConnectionString;
        }

        public WolfBookingContext Create()
        {
            return new WolfBookingContext(_databaseConnectionString);
        }
    }
}
