using System;
using System.ComponentModel.DataAnnotations;

namespace Backend.Persistence
{
    public class Bread
    {
        public Bread()
        { }

        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
        [Required]
        public decimal Price { get; set; }
        [Required]
        public DateTime Deleted { get; set; }

        public void UpdateWith(Business.Bread bread)
        {
            Name = bread.Name;
            Price = bread.Price;
            Deleted = bread.Deleted;
        }
    }
}
