using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Backend.Facade
{
    public class User
    {
        public User()
        { }

        public User(Business.User user)
        {
            Id = user.Id;
            Login = user.Login;
            Password = user.Password;
        }

        public int Id { get; set; }
        public string Login { get; set; }
        public string Password { get; set; }
    }
}
