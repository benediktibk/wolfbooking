using Backend.Business;
using Backend.Persistence;
using System.Collections.Generic;
using System.Linq;
using System;

namespace Backend.Facade
{
    public class BookingFacade
    {
        private readonly BreadFactory _breadFactory;
        private readonly BreadRepository _breadRepository;
        private readonly UserFactory _userFactory;

        public BookingFacade(BreadFactory breadFactory, BreadRepository breadRepository, UserFactory userFactory)
        {
            _breadFactory = breadFactory;
            _breadRepository = breadRepository;
            _userFactory = userFactory;
        }

        public IList<Bread> GetCurrentAvailableBreads()
        {
            return _breadFactory.GetCurrentAvailableBreads().Select(x => new Bread(x)).ToList();
        }

        public Bread GetBread(int id)
        {
            var bread = _breadFactory.Get(id);
            return bread == null ? null : new Bread(bread);
        }

        public int AddBread(Bread bread)
        {
            return _breadFactory.Create(bread);
        }

        public bool UpdateBread(Bread bread)
        {
            var oldBread = _breadFactory.Get(bread.Id);

            if (oldBread == null)
                return false;

            oldBread.UpdateWith(bread);
            return _breadRepository.Update(oldBread.ToPersistence());
        }

        public bool DeleteBread(int id)
        {
            var bread = _breadFactory.Get(id);

            if (bread == null)
                return false;

            bread.MarkAsDeleted();
            return _breadRepository.Update(bread.ToPersistence());
        }

        public bool IsLoginValid(string login, string password)
        {
            var user = _userFactory.GetByLogin(login);

            if (user == null)
                return false;

            return user.Password == password;
        }
    }
}
