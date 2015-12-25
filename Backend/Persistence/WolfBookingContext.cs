using System.Data.Entity;

namespace Backend.Persistence
{
    public class WolfBookingContext : DbContext
    {
        public WolfBookingContext() :
            base()
        {

        }

        public DbSet<Bread> Breads { get; set; }
    }
}
