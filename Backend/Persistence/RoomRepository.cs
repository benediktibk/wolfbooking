using System;
using System.Linq;
using System.Collections.Generic;

namespace Backend.Persistence
{
    public class RoomRepository
    {
        private readonly WolfBookingContext _dbContext;

        public RoomRepository(WolfBookingContext dbContext)
        {
            _dbContext = dbContext;
        }

        public int Add(Business.Room room)
        {
            Room persistenceRoom;

            persistenceRoom = new Room();
            persistenceRoom.UpdateWith(room);
            _dbContext.Rooms.Add(persistenceRoom);
            _dbContext.SaveChanges();

            return persistenceRoom.Id;
        }

        public void Update(Business.Room room)
        {
            var persistenceRoom = _dbContext.Rooms.Find(room.Id);

            if (persistenceRoom == null)
                throw new ArgumentException("room", $"room with id {room.Id} does not exist");

            _dbContext.Rooms.Attach(persistenceRoom);
            persistenceRoom.UpdateWith(room);
            _dbContext.SaveChanges();
        }

        public Business.Room Get(int id)
        {
            return new Business.Room(_dbContext.Rooms.Find(id));
        }

        public IList<Business.Room> GetCurrentAvailableRooms()
        {
            return GetAvailableRooms(DateTime.Now);
        }

        public IList<Business.Room> GetAvailableRooms(DateTime dateTime)
        {
            IList<Room> result;

            var queryResult = from room in _dbContext.Rooms
                where room.Deleted > dateTime
                select room;

            result = queryResult.ToList();


            return result.Select(x => new Business.Room(x)).ToList();
        }

        public bool IsRoomInUse(Business.Room room)
        {
            var now = DateTime.Now;

            var queryResult = from user in _dbContext.Users
                where user.Deleted > now && user.Room.Id == room.Id
                select user;

            return queryResult.Count() > 0;
        }
    }
}
