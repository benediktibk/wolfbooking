using System.Collections.Generic;
using System.Linq;

namespace Backend.Facade
{
    public class Bill
    {
        public Bill()
        {
            Entries = new List<BillEntry>();
        }

        public Bill(Business.Bill bill)
        {
            Total = bill.Total;
            Entries = bill.Entries.Select(x => new BillEntry(x)).ToList();
        }

        public IList<BillEntry> Entries { get; set; }
        public decimal Total { get; set; }
    }
}
