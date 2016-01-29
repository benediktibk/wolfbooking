using System.Data.Entity;

namespace Backend.Persistence
{
    public class WolfBookingContext : DbContext
    {
        public WolfBookingContext(string databaseConnectionString) :
            base(databaseConnectionString)
        { }

        public DbSet<Bread> Breads { get; set; }
        public DbSet<User> Users { get; set; }
    }
}
