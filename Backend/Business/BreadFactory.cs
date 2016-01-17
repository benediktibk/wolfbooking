using Backend.Persistence;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Backend.Business
{
    public class BreadFactory
    {
        private BreadRepository _breadRepository;

        public BreadFactory(BreadRepository breadRepository)
        {
            _breadRepository = breadRepository;
        }

        public IEnumerable<Bread> GetCurrentAvailableBreads()
        {
            var breads = _breadRepository.GetAvailableBreads(DateTime.Now);
            return breads.Select(x => new Bread(x));
        }

        public Bread Get(int id)
        {
            var bread = _breadRepository.Get(id);
            return bread == null ? null : new Bread(bread);
        }

        public int Create(Facade.Bread bread)
        {
            var businessBread = new Bread(bread.Name, bread.Price);
            return _breadRepository.Add(businessBread.ToPersistence());
        }
    }
}
