using System.Collections.Generic;
using System.Linq;

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
            Room = user.Room;
            Roles = new List<int>(user.Roles);
        }

        public int Id { get; set; }
        public string Login { get; set; }
        public string Password { get; set; }
        public List<int> Roles { get; set; }
        public int Room { get; set; }

        public override string ToString()
        {
            if (Roles.Count > 0)
                return $"Id: {Id}, Login: {Login}, Roles: {Roles.Select(x => x.ToString()).Aggregate((x, y) => $"{x}, {y}")}, Room: {Room}";
            else
                return $"Id: {Id}, Login: {Login}, Roles: none, Room: {Room}";
        }
    }
}
