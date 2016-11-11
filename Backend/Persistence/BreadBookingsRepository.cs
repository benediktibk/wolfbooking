using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;

namespace Backend.Persistence
{
    public class BreadBookingsRepository
    {
        private readonly WolfBookingContext _dbContext;

        public BreadBookingsRepository(WolfBookingContext dbContext)
        {
            _dbContext = dbContext;
        }

        public Business.BreadBookings GetBreadBookingsById(int id)
        {
            BreadBookings result;

            result =
                _dbContext.BreadBookings.Include(x => x.Bookings.Select(y => y.Bread))
                    .Include(x => x.Room)
                    .FirstOrDefault(x => x.Id == id);

            return result == null ? null : new Business.BreadBookings(result);
        }

        public Business.BreadBookings GetCurrentBreadBookingsForRoom(int room)
        {
            var now = DateTime.Now;
            BreadBookings result;
            var tomorrow = CalculateTomorrow();

            result =
                _dbContext.BreadBookings.Include(x => x.Bookings.Select(y => y.Bread))
                    .Include(x => x.Room)
                    .FirstOrDefault(x => x.Date.Equals(tomorrow) && x.Room.Id == room);

            if (result == null)
            {
                var persistenceRoom = _dbContext.Rooms.FirstOrDefault(x => x.Deleted > now && x.Id == room);

                if (persistenceRoom == null)
                    return null;

                result = new BreadBookings
                {
                    Room = persistenceRoom,
                    Date = tomorrow,
                    Bookings = new List<BreadBooking>()
                };
                var allBreads = _dbContext.Breads.Where(x => x.Deleted > now);

                foreach (var bread in allBreads)
                {
                    var breadBooking = new BreadBooking() {Bread = bread, Amount = 0};
                    _dbContext.BreadBooking.Add(breadBooking);
                    result.Bookings.Add(breadBooking);
                }
                _dbContext.BreadBookings.Add(result);
                _dbContext.SaveChanges();
            }

            return new Business.BreadBookings(result);
        }

        public Business.Bill GetBreadBookingsForRoomBetween(int room, DateTime start, DateTime end)
        {
            List<BreadBookings> result;

            var queryResult =
                _dbContext.BreadBookings.Include(x => x.Bookings.Select(y => y.Bread))
                    .Include(x => x.Room)
                    .Where(
                        x =>
                            DateTime.Compare(x.Date, start) >= 0 && DateTime.Compare(x.Date, end) <= 0 &&
                            x.Room.Id == room);
            result = queryResult.ToList();

            return new Business.Bill(result);
        }

        public void Update(Business.BreadBookings breadBookings)
        {
            var now = DateTime.Now;

            var persistenceBookings =
                _dbContext.BreadBookings.Include(x => x.Room)
                    .Include(x => x.Bookings)
                    .SingleOrDefault(x => x.Id == breadBookings.Id);

            if (persistenceBookings == null)
                throw new ArgumentException("breadBookings", $"bread booking with id {breadBookings.Id} does not exist");

            if (persistenceBookings.AlreadyOrdered)
                throw new ArgumentException("breadBookings",
                    $"bread booking with id {breadBookings.Id} has already been ordered");

            _dbContext.BreadBookings.Attach(persistenceBookings);
            persistenceBookings.UpdateWith(breadBookings);
            persistenceBookings.Room = _dbContext.Rooms.Find(breadBookings.Room);
            persistenceBookings.Bookings.Clear();

            foreach (var booking in breadBookings.Bookings)
            {
                var persistenceBooking = _dbContext.BreadBooking.Find(booking.Id);

                if (persistenceBooking == null)
                {
                    persistenceBooking = new BreadBooking();
                    _dbContext.BreadBooking.Add(persistenceBooking);
                }

                persistenceBooking.UpdateWith(booking);
                persistenceBookings.Bookings.Add(persistenceBooking);
            }

            _dbContext.SaveChanges();
        }

        private static DateTime CalculateTomorrow()
        {
            return DateTime.Today.AddDays(1);
        }
    }
}
