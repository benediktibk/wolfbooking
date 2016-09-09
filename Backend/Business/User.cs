using System;
using System.Collections.Generic;
using System.Linq;

namespace Backend.Business
{
    public class User
    {
        private List<int> _roles;

        public User(Persistence.User user)
        {
            Id = user.Id;
            Login = user.Login;
            Password = user.Password;
            Deleted = user.Deleted;
            Room = user.Room?.Id ?? -1;
            _roles = user.Roles.Select(x => x.Id).ToList();
        }

        public User(Facade.User user)
        {
            Login = user.Login;
            Password = user.Password;
            Deleted = DateTimeHelper.MaxValue;
            Room = user.Room;
            _roles = user.Roles.ToList();
        }

        public int Id { get; private set; }
        public string Login { get; private set; }
        public string Password { get; private set; }
        public DateTime Deleted { get; private set; }
        public int Room { get; private set; }

        public IReadOnlyList<int> Roles => _roles;

        public void UpdateWith(Facade.User user)
        {
            if (user.Id != Id)
                throw new ArgumentException("user", "wrong id");

            Login = user.Login;
            Password = user.Password;
            Room = user.Room;
            _roles = user.Roles.ToList();
        }

        public void MarkAsDeleted()
        {
            Deleted = DateTime.Now;
        }
    }
}
