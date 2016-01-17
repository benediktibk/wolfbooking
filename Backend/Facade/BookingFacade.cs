using Backend.Business;
using Backend.Persistence;
using System.Collections.Generic;
using System.Linq;

namespace Backend.Facade
{
    public class BookingFacade
    {
        private readonly BreadFactory _breadFactory;
        private readonly BreadRepository _breadRepository;

        public BookingFacade(BreadFactory breadFactory, BreadRepository breadRepository)
        {
            _breadFactory = breadFactory;
            _breadRepository = breadRepository;
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

        public bool UpdateBread(Bread bread)
        {
            var oldBread = _breadFactory.Get(bread.Id);

            if (oldBread == null)
                return false;

            oldBread.UpdateWith(bread);
            return _breadRepository.Update(oldBread.ToPersistence());
        }
    }
}
