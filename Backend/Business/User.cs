using System;
using System.Collections.Generic;
using System.Linq;

namespace Backend.Business
{
    public class User
    {
        private List<Role> _roles;

        public User(Persistence.User user)
        {
            Id = user.Id;
            Login = user.Login;
            Password = user.Password;
            Deleted = user.Deleted;
            _roles = new List<Role>();

            foreach (var role in user.Roles)
                _roles.Add(new Role(role));
        }

        public User(Facade.User user)
        {
            Login = user.Login;
            Password = user.Password;
            Deleted = DateTime.MaxValue;
        }

        public int Id { get; private set; }
        public string Login { get; private set; }
        public string Password { get; private set; }
        public DateTime Deleted { get; private set; }

        public IReadOnlyList<Role> Roles => _roles;

        public void UpdateWith(Facade.User user)
        {
            if (user.Id != Id)
                throw new ArgumentException("user", "wrong id");

            Login = user.Login;
            Password = user.Password;
        }

        public void MarkAsDeleted()
        {
            Deleted = DateTime.Now;
        }
    }
}
