using System;
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
                var queryResult = from user in context.Users
                                  where user.Login == login && user.Deleted > now
                                  select user;

                if (queryResult.Count() > 1)
                    throw new InvalidOperationException("found more than one user with the same login name");

                return queryResult.FirstOrDefault();
            }
        }

        private WolfBookingContext CreateContext()
        {
            return _contextFactory.Create();
        }
    }
}
