using System;
using System.Collections.Generic;

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

        public User(string login, string password)
        {
            Login = login;
            Password = password;
            Deleted = DateTime.MaxValue;
        }

        public int Id { get; private set; }
        public string Login { get; private set; }
        public string Password { get; private set; }
        public DateTime Deleted { get; private set; }

        public IReadOnlyList<Role> Roles => _roles;

        public Persistence.User ToPersistence()
        {
            return new Persistence.User
            {
                Id = Id,
                Login = Login,
                Password = Password,
                Deleted = Deleted
            };
        }

        public void UpdateWith(Facade.User user, RoleFactory roleFactory)
        {
            if (user.Id != Id)
                throw new ArgumentException("user", "wrong id");

            Login = user.Login;
            Password = user.Password;
            _roles = roleFactory.GetRolesByName(user.Roles);

            if (_roles.Count != user.Roles.Count)
                throw new ArgumentException("user", "user has invalid roles");
        }

        public void MarkAsDeleted()
        {
            Deleted = DateTime.Now;
        }
    }
}
