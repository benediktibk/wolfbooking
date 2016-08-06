using System;

namespace Backend.Business
{
    public class BillEntry
    {
        public BillEntry(Persistence.BreadBooking breadBooking, DateTime date)
        {
            Bread = breadBooking.Bread.Name;
            Date = date;
            Price = breadBooking.Bread.Price;
            Amount = breadBooking.Amount;
        }

        public string Bread { get; private set; }
        public DateTime Date { get; private set; }
        public decimal Price { get; private set; }
        public int Amount { get; private set; }
        public decimal Total => Price * Amount;
    }
}
