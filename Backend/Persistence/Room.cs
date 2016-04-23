using System;
using System.ComponentModel.DataAnnotations;

namespace Backend.Persistence
{
    public class Room
    {
        public Room()
        { }

        [Key]
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public string Description { get; set; }
        [Required]
        public DateTime Deleted { get; set; }

        public void UpdateWith(Business.Room room)
        {
            Name = room.Name;
            Description = room.Description;
            Deleted = room.Deleted;
        }
    }
}
