using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Backend.Business;

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
        public virtual ICollection<BreadBooking> Bookings { get; set; }

        public void UpdateWith(Business.BreadBookings breadBookings)
        {
            Date = breadBookings.Date;
        }
    }
}
