using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Backend.Persistence
{
    public class BreadRepository
    {
        private WolfBookingContext _context;

        public BreadRepository()
        {
            _context = new WolfBookingContext();
        }

        public void AddBread(Bread bread)
        {
            _context.Breads.Add(bread);
        }
    }
}
