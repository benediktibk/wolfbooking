using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Backend.Persistence
{
    public class BreadBookings
    {
        public BreadBookings ()
        { }

        [Key]
        public int Id { get; set; }
        public virtual Room Room { get; set; }
        public DateTime Date { get; set; }
        public bool AlreadyOrdered { get; set; }
        public virtual ICollection<BreadBooking> Bookings { get; set; }

        public void UpdateWith(Business.BreadBookings breadBookings)
        {
            Date = breadBookings.Date;
            AlreadyOrdered = breadBookings.AlreadyOrdered;
        }
    }
}
