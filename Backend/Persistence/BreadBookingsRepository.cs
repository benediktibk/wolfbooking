using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;

namespace Backend.Persistence
{
    public class BreadBookingsRepository
    {
        private WolfBookingContextFactory _contextFactory;

        public BreadBookingsRepository(WolfBookingContextFactory contextFactory)
        {
            _contextFactory = contextFactory;
        }

        public void EnsureCurrentBreadBookingsExistForAllRooms()
        {
            var now = DateTime.Now;
            var tomorrow = CalculateTomorrow();

            using (var context = CreateContext())
            {
                var rooms = context.Rooms.Where(x => x.Deleted > now);

                foreach (var room in rooms)
                {
                    var breadBookings = context.BreadBookings.FirstOrDefault(x => x.Room.Id == room.Id && x.Date == tomorrow);

                    if (breadBookings.Id > 0)
                        continue;

                    breadBookings.Room = room;
                    breadBookings.Date = tomorrow;
                    breadBookings.Bookings = new List<BreadBooking>();
                    context.BreadBookings.Add(breadBookings);
                }

                context.SaveChanges();
            }
        }

        public List<Business.BreadBookings> GetCurrentBreadBookingsForRooms(IList<int> rooms)
        {
            List<BreadBookings> result;
            var tomorrow = CalculateTomorrow();

            using (var context = CreateContext())
            {
                var queryResult = context.BreadBookings.Include(x => x.Bookings).Where(x => x.Date.Equals(tomorrow) && rooms.Contains(x.Room.Id));
                result = queryResult.ToList();
            }

            return result.Select(x => new Business.BreadBookings(x)).ToList();
        }

        private static DateTime CalculateTomorrow()
        {
            return DateTime.Today.AddDays(1);
        }

        private WolfBookingContext CreateContext()
        {
            return _contextFactory.Create();
        }
    }
}
