using System;
using System.Collections.Generic;
using System.Linq;

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
            AlreadyOrdered = bookings.AlreadyOrdered;
            Bookings = new List<BreadBooking>();

            foreach (var booking in bookings.Bookings)
                Bookings.Add(new BreadBooking(booking));
        }

        public int Id { get; set; }
        public int Room { get; set; }
        public DateTime Date { get; set; }
        public bool AlreadyOrdered { get; set; }
        public List<BreadBooking> Bookings { get; set; }

        public override string ToString()
        {
            var bookings = Bookings.Select(x => x.ToString());
            var bookingsJoined = bookings.Aggregate((x, y) => x + ", " + y);
            return $"Id: {Id}, Room: {Room}, Date: {Date}, Bookings: [{bookingsJoined}]";
        }
    }
}
