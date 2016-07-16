using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Backend.Persistence
{
    public class BreadBookings
    {
        public BreadBookings ()
        {
            Id = 0;
        }

        [Key]
        public int Id { get; set; }
        public virtual Room Room { get; set; }
        public DateTime Date { get; set; }
        public virtual ICollection<BreadBooking> Bookings { get; set; }
    }
}
