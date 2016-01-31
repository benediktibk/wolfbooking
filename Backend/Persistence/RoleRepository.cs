using System.Collections.Generic;
using System.Linq;

namespace Backend.Persistence
{
    public class RoleRepository
    {
        private WolfBookingContextFactory _contextFactory;

        public RoleRepository(WolfBookingContextFactory contextFactory)
        {
            _contextFactory = contextFactory;
        }

        public List<Role> GetRolesForUser(int userId)
        {
            using (var context = CreateContext())
            {
                var queryResult = from role in context.Roles
                                  where role.Users.Any(u => u.Id == userId)
                                  select role;

                return queryResult.ToList();
            }
        }

        private WolfBookingContext CreateContext()
        {
            return _contextFactory.Create();
        }
    }
}
