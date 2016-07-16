using System;
using System.Collections.Generic;

namespace Backend.Facade
{
    public class BreadBookings
    {
        public BreadBookings ()
        { }

        public BreadBookings (Business.BreadBookings bookings)
        {
            Id = bookings.Id;
            Room = bookings.Room;
            Date = bookings.Date;
            Bookings = new List<BreadBooking>(bookings.Bookings);
        }

        public int Id { get; set; }
        public int Room { get; set; }
        public DateTime Date { get; set; }
        public List<BreadBooking> Bookings { get; set; }
    }
}
