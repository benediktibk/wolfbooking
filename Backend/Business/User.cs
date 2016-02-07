using System;
using System.Collections.Generic;
using Backend.Facade;

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
