﻿using System;
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

        public Business.BreadBookings GetBreadBookingsById(int id)
        {
            BreadBookings result;

            using (var context = CreateContext())
            {
                result = context.BreadBookings.Find(id);  
            }

            return result == null ? null : new Business.BreadBookings(result);
        }

        public Business.BreadBookings GetCurrentBreadBookingsForRoom(int room)
        {
            var now = DateTime.Now;
            BreadBookings result;
            var tomorrow = CalculateTomorrow();

            using (var context = CreateContext())
            {
                result = context.BreadBookings.Include(x => x.Bookings).Include(x => x.Room).FirstOrDefault(x => x.Date.Equals(tomorrow) && x.Room.Id == room);

                if (result == null)
                {
                    var persistenceRoom = context.Rooms.FirstOrDefault(x => x.Deleted > now && x.Id == room);

                    if (persistenceRoom == null)
                        throw new ArgumentException("room", $"could not find room with id {room}");

                    result = new BreadBookings { Room = persistenceRoom, Date = tomorrow, Bookings = new List<BreadBooking>() };
                    context.BreadBookings.Add(result);
                    context.SaveChanges();
                }
            }

            return new Business.BreadBookings(result);
        }
        public List<Business.BreadBookings> GetPreviousBreadBookingsForRoom(int room)
        {
            var now = DateTime.Now;
            List<BreadBookings> result;

            using (var context = CreateContext())
            {
                var queryResult = context.BreadBookings.Include(x => x.Bookings).Include(x => x.Room).Where(x => DateTime.Compare(x.Date, now) <= 0 && x.Room.Id == room);
                result = queryResult.ToList();
            }

            return result.Select(x => new Business.BreadBookings(x)).ToList();
        }

        public void Update(Business.BreadBookings breadBookings)
        {
            using (var context = CreateContext())
            {
                var persistenceBookings = context.BreadBookings.Include(x => x.Room).Include(x => x.Bookings).SingleOrDefault(x => x.Id == breadBookings.Id);

                if (persistenceBookings == null)
                    throw new ArgumentException("breadBookings", $"bread booking with id {breadBookings.Id} does not exist");

                context.BreadBookings.Attach(persistenceBookings);
                persistenceBookings.UpdateWith(breadBookings);
                persistenceBookings.Room = context.Rooms.Find(breadBookings.Room);
                persistenceBookings.Bookings.Clear();

                foreach (var booking in breadBookings.Bookings)
                {
                    var persistenceBooking = context.BreadBooking.Find(booking.Id);

                    if (persistenceBooking == null)
                    {
                        persistenceBooking = new BreadBooking();
                        context.BreadBooking.Add(persistenceBooking);
                    }

                    persistenceBooking.UpdateWith(booking);
                    persistenceBookings.Bookings.Add(persistenceBooking);
                }

                context.SaveChanges();
            }
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
