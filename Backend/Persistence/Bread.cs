using System;
using System.ComponentModel.DataAnnotations;

namespace Backend.Persistence
{
    public class Bread
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public decimal Price { get; set; }
        [Required]
        public DateTime Created { get; set; }
        [Required]
        public DateTime Deleted { get; set; }
    }
}
