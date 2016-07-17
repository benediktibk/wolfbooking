using System;

namespace Backend.Business
{
    public class BreadBooking
    {
        public BreadBooking(Persistence.BreadBooking breadBooking)
        {
            Id = breadBooking.Id;
            Bread = breadBooking.Bread.Id;
            Amount = breadBooking.Amount;
        }

        public BreadBooking(Facade.BreadBooking breadBooking)
        {
            Id = breadBooking.Id;
            Bread = breadBooking.Bread;
            Amount = breadBooking.Amount;
        }

        public int Id { get; private set; }
        public int Bread { get; private set; }
        public int Amount { get; private set; }
    }
}
