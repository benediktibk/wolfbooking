namespace Backend.Facade
{
    public class Room
    {
        public Room()
        { }

        public Room(Business.Room room)
        {
            Id = room.Id;
            Name = room.Name;
            Description = room.Description;
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }

        public override string ToString()
        {
            return $"Id: {Id}, Name: {Name}, Description: {Description}";
        }
    }
}
