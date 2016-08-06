using System.Collections.Generic;
using System.Linq;

namespace Backend.Business
{
    public class Bill
    {
        private List<BillEntry> _billEntries;

        public Bill(IList<Persistence.BreadBookings> breadBookings)
        {
            _billEntries = new List<BillEntry>();
            foreach (var singleDayBooking in breadBookings)
            {
                var billEntriesForDay = singleDayBooking.Bookings.Select(x => new BillEntry(x, singleDayBooking.Date));
                _billEntries.Capacity = _billEntries.Count + billEntriesForDay.Count();
                foreach (var entry in billEntriesForDay)
                    _billEntries.Add(entry);
            }
        }

        public IReadOnlyList<BillEntry> Entries => _billEntries.ToList();
        public decimal Total => _billEntries.Count == 0 ? 0 : _billEntries.Select(entry => entry.Total).Aggregate((x, y) => x + y);
    }
}
