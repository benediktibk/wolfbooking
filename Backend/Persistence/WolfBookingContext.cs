using System.Data.Entity;
using Microsoft.AspNet.Identity.EntityFramework;

namespace Backend.Persistence
{
    public class WolfBookingContext : IdentityDbContext<User>
    {
        public WolfBookingContext(string databaseConnectionString) :
            base(databaseConnectionString)
        { }

        public DbSet<Bread> Breads { get; set; }
        public DbSet<Room> Rooms { get; set; }
        public DbSet<BreadBooking> BreadBooking { get; set; }
        public DbSet<BreadBookings> BreadBookings { get; set; }
    }
}
