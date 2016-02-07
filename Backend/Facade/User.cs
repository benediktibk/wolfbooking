using System.Collections.Generic;

namespace Backend.Facade
{
    public class User
    {
        public User()
        {
            Roles = new List<string>();
        }

        public User(Business.User user)
        {
            Id = user.Id;
            Login = user.Login;
            Password = user.Password;
            Roles = new List<string>();

            foreach (var role in user.Roles)
                Roles.Add(role.Name);
        }

        public int Id { get; set; }
        public string Login { get; set; }
        public string Password { get; set; }
        public List<string> Roles { get; set; }
    }
}
