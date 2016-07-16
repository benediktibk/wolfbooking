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
        public DbSet<Role> Roles { get; set; }
        public DbSet<Room> Rooms { get; set; }
        public DbSet<BreadBooking> BreadBooking { get; set; }
        public DbSet<BreadBookings> BreadBookings { get; set; }
    }
}
