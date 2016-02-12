using System.Collections.Generic;

namespace Backend.Facade
{
    public class User
    {
        public User()
        {
            Roles = new List<int>();
        }

        public User(Business.User user)
        {
            Id = user.Id;
            Login = user.Login;
            Password = user.Password;
            Roles = new List<int>(user.Roles);
        }

        public int Id { get; set; }
        public string Login { get; set; }
        public string Password { get; set; }
        public List<int> Roles { get; set; }
    }
}
