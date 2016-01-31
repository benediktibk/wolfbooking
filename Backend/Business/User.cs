using System;
using System.Collections.Generic;

namespace Backend.Business
{
    public class User
    {
        private readonly IList<string> _roles;

        public User(Persistence.User user)
        {
            Id = user.Id;
            Login = user.Login;
            Password = user.Password;
            Deleted = user.Deleted;

            _roles = new List<string>();
            foreach (var role in user.Roles)
                _roles.Add(role.Name);
        }

        public int Id { get; private set; }
        public string Login { get; private set; }
        public string Password { get; private set; }
        public DateTime Deleted { get; private set; }

        public bool HasAtLeastOneRole(IEnumerable<string> roles)
        {
            foreach (var role in roles)
                if (_roles.Contains(role))
                    return true;

            return false;
        }

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
    }
}
