using System;
using System.ComponentModel.DataAnnotations;

namespace Backend.Persistence
{
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
