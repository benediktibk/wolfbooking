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

        public List<Business.Role> GetRolesForUser(int userId)
        {
            IList<Role> result;

            using (var context = CreateContext())
            {
                var queryResult = from role in context.Roles
                                  where role.Users.Any(u => u.Id == userId)
                                  select role;

                result = queryResult.ToList();
            }

            return result.Select(x => new Business.Role(x)).ToList();
        }

        public List<Business.Role> GetRolesByName(List<string> roleNames)
        {
            IList<Role> result;

            using (var context = CreateContext())
            {
                var queryResult = from role in context.Roles
                                  where roleNames.Contains(role.Name)
                                  select role;

                result = queryResult.ToList();
            }

            return result.Select(x => new Business.Role(x)).ToList();
        }

        private WolfBookingContext CreateContext()
        {
            return _contextFactory.Create();
        }
    }
}
