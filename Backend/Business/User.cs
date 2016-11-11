using System;
using System.Collections.Generic;
using System.Linq;
using Backend.Persistence;
using Microsoft.AspNet.Identity;

namespace Backend.Business
{
    public class User
    {
        private IEnumerable<WolfBookingRole> _roles;

        public User(Persistence.User user)
        {
            Id = user.Id;
            UserName = user.UserName;
            Deleted = user.Deleted;
            Room = user.Room?.Id ?? -1;

            var roleManager = Factory.RoleManager;
            _roles = user.Roles.Select(r => new WolfBookingRole(roleManager.FindById(r.RoleId).Name));
        }

        public User(Facade.User user)
        {
            UserName = user.Login;
            Deleted = DateTime.MaxValue;
            Room = user.Room;
            _roles = user.Roles;
        }

        public int Id { get; private set; }
        public string UserName { get; private set; }
        public DateTime Deleted { get; private set; }
        public int Room { get; private set; }

        public IEnumerable<WolfBookingRole> Roles => _roles;

        public void UpdateWith(Facade.User user)
        {
            if (user.Id != Id)
                throw new ArgumentException("wrong id", "user.Id");

            UserName = user.Login;
            Room = user.Room;
            _roles = user.Roles;
        }

        public void MarkAsDeleted()
        {
            Deleted = DateTime.Now;
        }
    }
}
