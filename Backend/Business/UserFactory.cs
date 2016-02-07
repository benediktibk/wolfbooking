using System;
using Backend.Persistence;
using System.Collections.Generic;
using System.Linq;

namespace Backend.Business
{
    public class UserFactory
    {
        private UserRepository _userRepository;

        public UserFactory(UserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public User GetByLogin(string login)
        {
            var user = _userRepository.GetByLogin(login);
            return user == null ? null : new User(user);
        }

        public IList<User> GetCurrentAvailableUsers()
        {
            var users = _userRepository.GetAvailableUsers(DateTime.Now);
            return users.Select(x => new User(x)).ToList();
        }

        public User Get(int id)
        {
            var user = _userRepository.Get(id);
            return user == null ? null : new User(user);
        }

        public int Create(Facade.User user)
        {
            var businessUser = new User(user.Login, user.Password);
            return _userRepository.Add(businessUser.ToPersistence());
        }
    }
}
