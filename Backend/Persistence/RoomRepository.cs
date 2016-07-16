using System;
using System.Linq;
using System.Collections.Generic;

namespace Backend.Persistence
{
    public class RoomRepository
    {
        private WolfBookingContextFactory _contextFactory;

        public RoomRepository(WolfBookingContextFactory contextFactory)
        {
            _contextFactory = contextFactory;
        }

        public int Add(Business.Room room)
        {
            Room persistenceRoom;

            using (var context = CreateContext())
            {
                persistenceRoom = new Room();
                persistenceRoom.UpdateWith(room);
                context.Rooms.Add(persistenceRoom);
                context.SaveChanges();
            }

            return persistenceRoom.Id;
        }

        public void Update(Business.Room room)
        {
            using (var context = CreateContext())
            {
                var persistenceRoom = context.Rooms.Find(room.Id);

                if (persistenceRoom == null)
                    throw new ArgumentException("room", $"room with id {room.Id} does not exist");

                context.Rooms.Attach(persistenceRoom);
                persistenceRoom.UpdateWith(room);
                context.SaveChanges();
            }
        }

        public Business.Room Get(int id)
        {
            using (var context = CreateContext())
                return new Business.Room(context.Rooms.Find(id));
        }

        public IList<Business.Room> GetCurrentAvailableRooms()
        {
            return GetAvailableRooms(DateTime.Now);
        }

        public IList<Business.Room> GetAvailableRooms(DateTime dateTime)
        {
            IList<Room> result;

            using (var context = CreateContext())
            {
                var queryResult = from room in context.Rooms
                                  where room.Deleted > dateTime
                                  select room;

                result = queryResult.ToList();
            }

            return result.Select(x => new Business.Room(x)).ToList();
        }

        public bool IsRoomInUse(Business.Room room)
        {
            var now = DateTime.Now;

            using (var context = CreateContext())
            {
                var queryResult = from user in context.Users
                                  where user.Deleted > now && user.Room.Id == room.Id
                                  select user;

                return queryResult.Count() > 0;
            }
        }

        private WolfBookingContext CreateContext()
        {
            return _contextFactory.Create();
        }
    }
}
