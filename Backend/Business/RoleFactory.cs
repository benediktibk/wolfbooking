using Backend.Persistence;
using System.Collections.Generic;
using System.Linq;

namespace Backend.Business
{
    public class RoleFactory
    {
        private readonly RoleRepository _roleRepository;

        public RoleFactory(RoleRepository roleRepository)
        {
            _roleRepository = roleRepository;
        }

        public List<Role> GetRolesForUser(int userId)
        {
            var roles = _roleRepository.GetRolesForUser(userId);
            return roles.Select(role => new Role(role)).ToList();
        }
    }
}
