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

            CreateRoleIfNotExistent(context, "User");
            CreateRoleIfNotExistent(context, "Manager");
            CreateRoleIfNotExistent(context, "Admin");

            if (!(context.Users.Any(u => u.UserName == "admin")))
            {
                var userStore = new WolfBookingUserStore(context);
                var manager = new WolfBookingUserManager(userStore, null);
                var user = new User {UserName = "admin", Email = "info@wolf.tirol"};
                var result = manager.Create(user, "Einstieg1!");
                if (!result.Succeeded)
                    throw new Exception(result.ToString());

                manager.AddToRole(user.Id, "Admin");
            }

            context.SaveChanges();
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
