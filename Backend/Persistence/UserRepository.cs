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
                var queryResult = from user in context.Users.Include(x => x.Roles).Include(x => x.Room)
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
                var queryResult = from user in context.Users.Include(x => x.Roles).Include(x => x.Room)
                                  where user.Id == id
                                  select user;

                result = queryResult.FirstOrDefault();
            }

            return result != null ? new Business.User(result) : null;
        }

        public int Add(Business.User user)
        {
            var now = DateTime.Now;
            User persistenceUser;
            
            using (var context = CreateContext())
            {
                persistenceUser = new User();
                persistenceUser.UpdateWith(user);
                var loginAlreadyInUse = 
                    (from databaseUser in context.Users
                    where databaseUser.Login == persistenceUser.Login && databaseUser.Deleted > now
                    select databaseUser).Count() > 0;

                if (loginAlreadyInUse)
                    return -1;

                persistenceUser.Room = user.Room >= 0 ? context.Rooms.Find(user.Room) : null;

                context.Users.Add(persistenceUser);
                context.SaveChanges();

                persistenceUser.Roles = new List<Role>();

                foreach (var role in user.Roles)
                {
                    var persistenceRole = context.Roles.Find(role);

                    if (persistenceRole == null)
                        throw new ArgumentException("user", $"contains invalid role {role}");

                    persistenceUser.Roles.Add(persistenceRole);
                }
                context.Entry(persistenceUser).State = EntityState.Modified;
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
                var queryResult = from user in context.Users.Include(x => x.Roles).Include(x => x.Room)
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
                var persistenceUser = context.Users.Include(x => x.Room).SingleOrDefault(x => x.Id == user.Id);

                if (persistenceUser == null)
                    throw new ArgumentException("user", $"user with id {user.Id} does not exist");

                context.Users.Attach(persistenceUser);
                persistenceUser.UpdateWith(user);
                persistenceUser.Room = user.Room >= 0 ? context.Rooms.Find(user.Room) : null;
                persistenceUser.Roles.Clear();

                foreach (var role in user.Roles)
                {
                    var persistenceRole = context.Roles.Find(role);

                    if (persistenceRole == null)
                        throw new ArgumentException("user", $"contains invalid role {role}");

                    persistenceUser.Roles.Add(persistenceRole);
                }

                context.Entry(persistenceUser).State = EntityState.Modified;
                context.SaveChanges();
            }
        }

        private WolfBookingContext CreateContext()
        {
            return _contextFactory.Create();
        }
    }
}
