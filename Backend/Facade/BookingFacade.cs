using Backend.Business;
using System.Collections.Generic;
using System.Linq;

namespace Backend.Facade
{
    public class BookingFacade
    {
        private readonly BreadFactory _breadFactory;

        public BookingFacade(BreadFactory breadFactory)
        {
            _breadFactory = breadFactory;
        }

        public IEnumerable<Bread> GetCurrentAvailableBreads()
        {
            return _breadFactory.GetCurrentAvailableBreads().Select(x => new Bread(x));
        }

        public Bread GetBread(int id)
        {
            var bread = _breadFactory.Get(id);
            return bread == null ? null : new Bread(bread);
        }
    }
}
