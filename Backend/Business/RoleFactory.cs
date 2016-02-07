using Backend.Persistence;
using System.Collections.Generic;
using System.Linq;
using System;

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

        public List<Role> GetRolesByName(List<string> roleNames)
        {
            var roles = _roleRepository.GetRolesByName(roleNames);
            return roles.Select(role => new Role(role)).ToList();
        }
    }
}
