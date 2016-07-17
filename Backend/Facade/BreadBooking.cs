namespace Backend.Facade
{
    public class BreadBooking
    {
        public BreadBooking()
        { }

        public BreadBooking(Business.BreadBooking breadBooking)
        {
            Id = breadBooking.Id;
            Bread = breadBooking.Bread;
            Amount = breadBooking.Amount;
        }

        public int Id { get; set; }
        public int Bread { get; set; }
        public int Amount { get; set; }

        public override string ToString()
        {
            return $"Id: {Id}, Bread: {Bread}, Amount {Amount}";
        }
    }
}
