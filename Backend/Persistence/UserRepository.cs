using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;

namespace Backend.Persistence
{
    public class UserRepository
    {
        private WolfBookingContextFactory _contextFactory;

        public UserRepository(WolfBookingContextFactory contextFactory)
        {
            _contextFactory = contextFactory;
        }

        public Business.User GetByLogin(string login)
        {
            var now = DateTime.Now;
            User result;

            using (var context = CreateContext())
            {
                var queryResult = from user in context.Users.Include(x => x.Roles)
                                  where user.Login == login && user.Deleted > now
                                  select user;

                if (queryResult.Count() > 1)
                    throw new InvalidOperationException("found more than one user with the same login name");

                result = queryResult.FirstOrDefault();
            }

            return result != null ? new Business.User(result) : null;
        }

        public Business.User Get(int id)
        {
            User result;

            using (var context = CreateContext())
            {
                var queryResult = from user in context.Users.Include(x => x.Roles)
                                  where user.Id == id
                                  select user;

                result = queryResult.FirstOrDefault();
            }

            return result != null ? new Business.User(result) : null;
        }

        public int Add(Business.User user)
        {
            User persistenceUser;

            using (var context = CreateContext())
            {
                persistenceUser = new User();
                persistenceUser.UpdateWith(user);
                context.Users.Add(persistenceUser);
                context.SaveChanges();
            }

            return persistenceUser.Id;
        }

        public IList<Business.User> GetCurrentAvailableUsers()
        {
            return GetAvailableUsers(DateTime.Now);
        }

        public IList<Business.User> GetAvailableUsers(DateTime dateTime)
        {
            IList<User> result;

            using (var context = CreateContext())
            {
                var queryResult = from user in context.Users.Include(x => x.Roles)
                                  where user.Deleted > dateTime
                                  select user;
                result = queryResult.ToList();
            }

            return result.Select(x => new Business.User(x)).ToList();
        }

        public void Update(Business.User user)
        {
            using (var context = CreateContext())
            {
                var persistenceUser = context.Users.Find(user.Id);

                if (persistenceUser == null)
                    throw new ArgumentException("user", $"user with id {user.Id} does not exist");

                context.Users.Attach(persistenceUser);
                persistenceUser.UpdateWith(user);
                persistenceUser.Roles.Clear();

                foreach (var role in user.Roles)
                {
                    var persistenceRole = context.Roles.Find(role);

                    if (persistenceRole == null)
                        throw new ArgumentException("user", $"contains invalid role {role}");

                    persistenceUser.Roles.Add(persistenceRole);
                }
                
                context.SaveChanges();
            }
        }

        private WolfBookingContext CreateContext()
        {
            return _contextFactory.Create();
        }
    }
}
