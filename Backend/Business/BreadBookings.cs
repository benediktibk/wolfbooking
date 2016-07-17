using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace Backend.Business
{
    public class BreadBookings
    {
        private IList<BreadBooking> _bookings;

        public BreadBookings(Persistence.BreadBookings bookings)
        {
            Id = bookings.Id;
            Room = bookings.Room.Id;
            Date = bookings.Date;

            _bookings = new List<BreadBooking>();
            foreach (var booking in bookings.Bookings)
                _bookings.Add(new BreadBooking(booking));
        }

        public BreadBookings(Facade.BreadBookings bookings)
        {
            Id = bookings.Id;
            Room = bookings.Room;
            Date = bookings.Date;

            _bookings = new List<BreadBooking>();
            foreach (var booking in bookings.Bookings)
                _bookings.Add(new BreadBooking(booking));
        }

        public int Id { get; private set; }
        public int Room { get; private set; }
        public DateTime Date { get; private set; }
        public IReadOnlyList<BreadBooking> Bookings => new ReadOnlyCollection<BreadBooking>(_bookings);

        public void UpdateWith(Facade.BreadBookings breadBookings)
        {
            if (Id != breadBookings.Id)
                throw new ArgumentException("breadBookings", "wrong id");

            Room = breadBookings.Room;
            Date = breadBookings.Date;

            _bookings = new List<BreadBooking>();
            foreach (var booking in breadBookings.Bookings)
                _bookings.Add(new BreadBooking(booking));
        }
    }
}
