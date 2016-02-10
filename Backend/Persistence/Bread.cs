using System;
using System.ComponentModel.DataAnnotations;

namespace Backend.Persistence
{
    public class Bread
    {
        public Bread()
        { }

        public Bread(Business.Bread bread)
        {
            Name = bread.Name;
            Price = bread.Price;
            Deleted = bread.Deleted;
        }

        [Key]
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public decimal Price { get; set; }
        [Required]
        public DateTime Deleted { get; set; }

        internal void UpdateWith(Business.Bread bread)
        {
            if (Id != bread.Id)
                throw new ArgumentException("bread", "id mismatch");

            Name = bread.Name;
            Price = bread.Price;
            Deleted = bread.Deleted;
        }
    }
}
