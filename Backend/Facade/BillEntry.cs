using System;

namespace Backend.Facade
{
    public class BillEntry
    {
        public BillEntry()
        { }

        public BillEntry(Business.BillEntry billEntry)
        {
            Bread = billEntry.Bread;
            Date = billEntry.Date;
            Price = billEntry.Price;
            Amount = billEntry.Amount;
            Total = billEntry.Total;
        }

        public string Bread { get; set; }
        public DateTime Date { get; set; }
        public decimal Price { get; set; }
        public int Amount { get; set; }
        public decimal Total { get; set; }
    }
}
