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

        public IList<Bread> GetCurrentAvailableBreads()
        {
            var breads = _breadRepository.GetAvailableBreads(DateTime.Now);
            return breads.Select(x => new Bread(x)).ToList();
        }

    }
}
