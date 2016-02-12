using Backend.Business;
using Backend.Persistence;
using System.Collections.Generic;
using System.Linq;

namespace Backend.Facade
{
    public class BookingFacade
    {
        private readonly BreadRepository _breadRepository;
        private readonly UserRepository _userRepository;
        private readonly RoleRepository _roleRepository;

        public BookingFacade(BreadRepository breadRepository, UserRepository userRepository, RoleRepository roleRepository)
        {
            _breadRepository = breadRepository;
            _userRepository = userRepository;
            _roleRepository = roleRepository;
        }

        public IList<User> GetCurrentAvailableUsers()
        {
            return _userRepository.GetCurrentAvailableUsers().Select(x => new User(x)).ToList();
        }

        public IList<Bread> GetCurrentAvailableBreads()
        {
            return _breadRepository.GetCurrentAvailableBreads().Select(x => new Bread(x)).ToList();
        }

        public User GetUser(int id)
        {
            var user = _userRepository.Get(id);
            return user == null ? null : new User(user);
        }

        public Bread GetBread(int id)
        {
            var bread = _breadRepository.Get(id);
            return bread == null ? null : new Bread(bread);
        }

        public int AddUser(User user)
        {
            return _userRepository.Add(new Business.User(user));
        }

        public int AddBread(Bread bread)
        {
            return _breadRepository.Add(new Business.Bread(bread));
        }

        public bool UpdateBread(Bread bread)
        {
            var businessBread = _breadRepository.Get(bread.Id);

            if (businessBread == null)
                return false;

            businessBread.UpdateWith(bread);
            _breadRepository.Update(businessBread);
            return true;
        }

        public bool UpdateUser(User user)
        {
            var businessUser = _userRepository.Get(user.Id);

            if (businessUser == null)
                return false;

            businessUser.UpdateWith(user);
            _userRepository.Update(businessUser);
            return true;
        }

        public bool DeleteUser(int id)
        {
            var user = _userRepository.Get(id);

            if (user == null)
                return false;

            user.MarkAsDeleted();
            _userRepository.Update(user);
            return true;
        }

        public bool DeleteBread(int id)
        {
            var bread = _breadRepository.Get(id);

            if (bread == null)
                return false;

            bread.MarkAsDeleted();
            _breadRepository.Update(bread);
            return true;
        }

        public bool IsLoginValid(string login, string password)
        {
            var user = _userRepository.GetByLogin(login);

            if (user == null)
                return false;
            
            return user.Password == password;
        }

        public List<string> GetRolesForUser(string login)
        {
            var user = _userRepository.GetByLogin(login);

            if (user == null)
                return new List<string>();

            var roles = _roleRepository.GetRolesForUser(user.Id);
            return roles.Select(role => role.Name).ToList();
        }

        public List<Role> GetAllRoles()
        {
            return _roleRepository.GetAllRoles().Select(x => new Role(x)).ToList();
        }
    }
}
