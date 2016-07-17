using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Backend.Persistence
{
    [Table("BreadBooking")]
    public class BreadBooking
    {
        public BreadBooking()
        { }

        [Key]
        public int Id { get; set; }
        public virtual Bread Bread { get; set; }
        [Required]
        public int Amount { get; set; }

        public void UpdateWith(Business.BreadBooking breadBooking)
        {
            Amount = breadBooking.Amount;
        }
    }
}
