using Backend.Business;
using Backend.Persistence;
using System.Collections.Generic;
using System.Linq;
using System;

namespace Backend.Facade
{
    public class BookingFacade
    {
        private readonly BreadRepository _breadRepository;
        private readonly BreadFactory _breadFactory;
        private readonly UserRepository _userRepository;
        private readonly UserFactory _userFactory;
        private readonly RoleFactory _roleFactory;

        public BookingFacade(BreadFactory breadFactory, BreadRepository breadRepository, UserFactory userFactory, RoleFactory roleFactory, UserRepository userRepository)
        {
            _breadFactory = breadFactory;
            _userRepository = userRepository;
            _userFactory = userFactory;
            _roleFactory = roleFactory;
        }

        public IList<User> GetCurrentAvailableUsers()
        {
            return _userFactory.GetCurrentAvailableUsers().Select(x => new User(x)).ToList();
        }

        public IList<Bread> GetCurrentAvailableBreads()
        {
            return _breadFactory.GetCurrentAvailableBreads().Select(x => new Bread(x)).ToList();
        }

        public User GetUser(int id)
        {
            var user = _userFactory.Get(id);
            return user == null ? null : new User(user);
        }

        public Bread GetBread(int id)
        {
            var bread = _breadFactory.Get(id);
            return bread == null ? null : new Bread(bread);
        }

        public int AddUser(User user)
        {
            return _userFactory.Create(user);
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

        public bool UpdateUser(User user)
        {
            var oldUser = _userFactory.Get(user.Id);

            if (oldUser == null)
                return false;

            oldUser.UpdateWith(user);
            return _userRepository.Update(oldUser.ToPersistence());
        }

        public bool DeleteUser(int id)
        {
            var user = _userFactory.Get(id);

            if (user == null)
                return false;

            user.MarkAsDeleted();
            return _userRepository.Update(user.ToPersistence());
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

        public List<string> GetRolesForUser(string login)
        {
            var user = _userFactory.GetByLogin(login);

            if (user == null)
                return new List<string>();

            var roles = _roleFactory.GetRolesForUser(user.Id);
            return roles.Select(role => role.Name).ToList();
        }
    }
}
