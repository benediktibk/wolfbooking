using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Backend.Persistence;
using Microsoft.AspNet.Identity;

namespace Backend.Facade
{
    public class User
    {
        public User()
        {
            Roles = new List<WolfBookingRole>();
        }

        public User(Business.User user) 
        {
            Id = user.Id;
            Login = user.UserName;
            Room = user.Room;
            Roles = user.Roles;
        }

        public int Id { get; set; }
        public string Login { get; set; }
        public IEnumerable<WolfBookingRole> Roles { get; set; }
        public int Room { get; set; }

        public override string ToString()
        {
            if (Roles.Any())
                return $"Id: {Id}, UserName: {Login}, Roles: {Roles.Select(x => x.ToString()).Aggregate((x, y) => $"{x}, {y}")}, Room: {Room}";
            else
                return $"Id: {Id}, UserName: {Login}, Roles: none, Room: {Room}";
        }
    }
}
