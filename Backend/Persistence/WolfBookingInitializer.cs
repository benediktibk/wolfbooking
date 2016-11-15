using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;

namespace Backend.Persistence
{
    public class WolfBookingInitializer : CreateDatabaseIfNotExists<WolfBookingContext>
    {
        protected override void Seed(WolfBookingContext context)
        {
            base.Seed(context);

            SeedRoles(context);
            SeedUsers(context);
            SeedRooms(context);

            context.SaveChanges();
        }

        private void SeedRooms(WolfBookingContext context)
        {
            for (var i = 1; i <= 3; i++)
            {
                var room = new Room() { Name = $"Fw{i}", Description = $"Ferienwohnung {i}", Deleted = DateTime.MaxValue};
                context.Rooms.Add(room);
            }
        }

        private void SeedUsers(WolfBookingContext context)
        {
            if (!(context.Users.Any(u => u.UserName == "admin")))
            {
                var userStore = new WolfBookingUserStore(context);
                var manager = new WolfBookingUserManager(userStore, null);
                var user = new User { UserName = "admin", Email = "info@wolf.tirol" };
                var result = manager.Create(user, "Einstieg1!");
                if (!result.Succeeded)
                    throw new Exception(result.ToString());

                manager.AddToRole(user.Id, "Admin");
                manager.AddToRole(user.Id, "Manager");
            }
        }

        private void SeedRoles(WolfBookingContext context)
        {
            CreateRoleIfNotExistent(context, "User");
            CreateRoleIfNotExistent(context, "Manager");
            CreateRoleIfNotExistent(context, "Admin");
        }

        private void CreateRoleIfNotExistent(WolfBookingContext context, string roleName)
        {
            if (!context.Roles.Any(r => r.Name == roleName))
            {
                var store = new WolfBookingRoleStore(context);
                var manager = new RoleManager<WolfBookingRole, int>(store);
                var role = new WolfBookingRole { Name = roleName };

                manager.Create(role);
            }
        }
    }
}
