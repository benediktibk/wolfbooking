using System;

namespace Backend.Business
{
    public class Room
    {
        public Room(Persistence.Room room)
        {
            Id = room.Id;
            Name = room.Name;
            Description = room.Description;
            Deleted = room.Deleted;
        }

        public Room(Facade.Room room)
        {
            Name = room.Name;
            Description = room.Description;
            Deleted = DateTimeHelper.MaxValue;
        }

        public int Id { get; private set; }
        public string Name { get; private set; }
        public string Description { get; private set; }
        public DateTime Deleted { get; private set; }

        public void UpdateWith(Facade.Room room)
        {
            if (room.Id != Id)
                throw new ArgumentException("room", "wrong id");

            Name = room.Name;
            Description = room.Description;
        }

        public void MarkAsDeleted()
        {
            Deleted = DateTime.Now;
        }
    }
}
