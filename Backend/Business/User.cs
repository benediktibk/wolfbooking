using System;

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

        public int Id { get; set; }
        public string Login { get; set; }
        public string Password { get; set; }
        public DateTime Deleted { get; set; }

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
