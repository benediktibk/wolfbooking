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

        public User GetByLogin(string login)
        {
            var now = DateTime.Now;

            using (var context = CreateContext())
            {
                var queryResult = from user in context.Users.Include(x => x.Roles)
                                  where user.Login == login && user.Deleted > now
                                  select user;

                if (queryResult.Count() > 1)
                    throw new InvalidOperationException("found more than one user with the same login name");

                return queryResult.FirstOrDefault();
            }
        }

        public User Get(int id)
        {
            using (var context = CreateContext())
                return context.Users.Find(id);
        }

        public int Add(User user)
        {
            using (var context = CreateContext())
            {
                context.Users.Add(user);
                context.SaveChanges();
            }

            return user.Id;
        }

        public IList<User> GetAvailableUsers(DateTime dateTime)
        {
            IList<User> result;

            using (var context = CreateContext())
            {
                var queryResult = from user in context.Users.Include(x => x.Roles)
                                  where user.Deleted > dateTime
                                  select user;
                result = queryResult.ToList();
            }

            return result;
        }

        public bool Update(User user)
        {
            int count;

            using (var context = CreateContext())
            {
                context.Users.Attach(user);
                context.Entry(user).State = EntityState.Modified;
                count = context.SaveChanges();
            }

            return count == 1;
        }

        private WolfBookingContext CreateContext()
        {
            return _contextFactory.Create();
        }
    }
}
