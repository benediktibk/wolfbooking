﻿using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;

namespace Backend.Persistence
{
    public class UserRepository
    {
        private readonly WolfBookingSignInManager _signInManager;
        private readonly WolfBookingUserManager _userManager;
        private readonly WolfBookingContext _dbContext;

        public UserRepository(WolfBookingSignInManager signInManager, WolfBookingUserManager userManager,
            WolfBookingContext dbContext)
        {
            _signInManager = signInManager;
            _userManager = userManager;
            _dbContext = dbContext;
        }

        public Business.User GetByLogin(string login)
        {
            var now = DateTime.Now;

            var queryResult = from user in _dbContext.Users.Include(x => x.Roles).Include(x => x.Room)
                where user.UserName == login && user.Deleted > now
                select user;

            if (queryResult.Count() > 1)
                throw new InvalidOperationException("found more than one user with the same login name");

            var result = queryResult.FirstOrDefault();

            return result != null ? new Business.User(result) : null;
        }

        public Business.User Get(int id)
        {
            var persistenceUser = _userManager.FindById(id);

            return persistenceUser != null ? new Business.User(persistenceUser) : null;
        }

        public int Add(Business.User user, string password)
        {
            var now = DateTime.Now;
            var persistenceUser = new User();

            if (_userManager.FindByName(user.UserName) != null)
            {
                return -1; // login already in use // TODO: why do we use int return values here?
            }

            persistenceUser.UserName = user.UserName;
            persistenceUser.Room = user.Room >= 0 ? _dbContext.Rooms.Find(user.Room) : null;

            _userManager.Create(persistenceUser, password);

            var result = _userManager.Create(persistenceUser, password);
            if (!result.Succeeded)
                throw new Exception(result.ToString());

            UpdateRoles(persistenceUser, user.Roles);
            _dbContext.Entry(persistenceUser).State = EntityState.Modified;
            _dbContext.SaveChanges();

            return persistenceUser.Id;
        }

        private void UpdateRoles(User persistenceUser, IEnumerable<WolfBookingRole> userRoles)
        {
            var allOldUserRoles = _userManager.GetRoles(persistenceUser.Id).ToArray();
            _userManager.RemoveFromRoles(persistenceUser.Id, allOldUserRoles);

            var allNewUserRoles = userRoles.Select(x => x.Name).ToArray();
            _userManager.AddToRoles(persistenceUser.Id, allNewUserRoles);
        }

        public IList<Business.User> GetCurrentAvailableUsers()
        {
            return GetAvailableUsers(DateTime.Now);
        }

        public IList<Business.User> GetAvailableUsers(DateTime dateTime)
        {
            var queryResult = from user in _dbContext.Users.Include(x => x.Roles).Include(x => x.Room)
                where user.Deleted > dateTime
                select user;

            return queryResult.Select(x => new Business.User(x)).ToList();
        }

        public void Update(Business.User user)
        {
            var userStore = new WolfBookingUserStore(_dbContext);
            var userManager = new WolfBookingUserManager(userStore);

            var persistenceUser = userManager.FindById(user.Id);

            if (persistenceUser == null)
                throw new ArgumentException($"user with id {user.Id} does not exist", nameof(user));

            persistenceUser.Deleted = user.Deleted;
            persistenceUser.Room = user.Room >= 0 ? _dbContext.Rooms.Find(user.Room) : null;
        }

        public IEnumerable<WolfBookingRole> GetAllRoles()
        {
            var store = new RoleStore<IdentityRole>(_dbContext);
            var manager = new RoleManager<IdentityRole>(store);
            return manager.Roles.Select(x => new WolfBookingRole(x.Name));
        }

        public ICollection<WolfBookingUserRole> GetRolesForUserName(string userName)
        {
            return _userManager.FindByName(userName).Roles;
        }

        public IEnumerable<string> GetUserRoleNamesForUserName(string username)
        {
            var store = new WolfBookingUserStore(_dbContext);
            var manager = new WolfBookingUserManager(store);
            var roleStore = new WolfBookingRoleStore(_dbContext);
            var roleManager = new RoleManager<WolfBookingRole, int>(roleStore);

            var roles = manager.FindByName(username)?.Roles;

            foreach (var role in roles)
            {
                var rolename = roleManager.FindById(role.RoleId).Name;
                yield return rolename;
            }
        }

        public SignInStatus Login(string userName, string password)
        {
            var result = _signInManager.PasswordSignIn(userName, password, true, shouldLockout: false);
            return result;
        }
    }
}
