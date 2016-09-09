using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;

namespace Backend.Persistence
{
    public class WolfBookingInitializer : CreateDatabaseIfNotExists<WolfBookingContext>
    {
        protected override void Seed(WolfBookingContext context)
        {
            base.Seed(context);

            var adminUser = new User
            {
                Login = "admin",
                Password = "1234",
                Deleted = DateTimeHelper.MaxValue
            };
            var usersRole = new Role
            {
                Name = "Users"
            };
            var managersRole = new Role
            {
                Name = "Managers"
            };
            var adminsRole = new Role
            {
                Name = "Administrators"
            };

            context.Users.Add(adminUser);
            context.Roles.Add(usersRole);
            context.Roles.Add(managersRole);
            context.Roles.Add(adminsRole);

            context.SaveChanges();

            adminUser = context.Users.FirstOrDefault(x => x.Login == "admin");
            adminUser.Roles = new List<Role>();
            var roles = context.Roles.Where(x => true);
            foreach (var role in roles)
                adminUser.Roles.Add(role);

            context.SaveChanges();
        }
    }
}
