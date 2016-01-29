using Backend.Persistence;

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
    }
}
