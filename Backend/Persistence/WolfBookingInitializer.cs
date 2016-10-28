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
                var userStore = new UserStore<User>(context);
                var manager = new UserManager<User>(userStore);
                var user = new User {UserName = "admin"};
                var result = manager.Create(user, "Einstieg00");
                if (!result.Succeeded)
                    throw new Exception(result.ToString());

                manager.AddToRole(user.Id, "Admin");

            }

            // TODO: role management
            //context.Users.Add(adminUser);
            //context.Roles.Add(usersRole);
            //context.Roles.Add(managersRole);
            //context.Roles.Add(adminsRole);

            context.SaveChanges();

            var adminUser = context.Users.Single(x => x.UserName == "admin");
            //adminUser.Roles = new List<Role>();
            //var roles = context.Roles.Where(x => true);
            //foreach (var role in roles)
            //    adminUser.Roles.Add(role);

            context.SaveChanges();
        }

        private void CreateRoleIfNotExistent(WolfBookingContext context, string roleName)
        {
            if (!context.Roles.Any(r => r.Name == roleName))
            {
                var store = new RoleStore<IdentityRole>(context);
                var manager = new RoleManager<IdentityRole>(store);
                var role = new IdentityRole { Name = roleName };

                manager.Create(role);
            }
        }
    }
}
