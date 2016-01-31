using System;
using System.Collections.Generic;

namespace Backend.Business
{
    public class User
    {
        public User(Persistence.User user)
        {
            Id = user.Id;
            Login = user.Login;
            Password = user.Password;
            Deleted = user.Deleted;
        }

        public int Id { get; private set; }
        public string Login { get; private set; }
        public string Password { get; private set; }
        public DateTime Deleted { get; private set; }

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
