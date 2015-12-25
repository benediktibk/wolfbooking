using System;
using System.Linq;
using System.Collections.Generic;

namespace Backend.Persistence
{
    public class BreadRepository
    {
        private string _databaseConnectionString;

        public BreadRepository(string databaseConnectionString)
        {
            _databaseConnectionString = databaseConnectionString;
        }

        public void Add(Bread bread)
        {
            using (var context = CreateContext())
            {
                bread.Created = DateTime.Now;
                bread.Deleted = DateTime.MaxValue;
                context.Breads.Add(bread);
                context.SaveChanges();
            }
        }

        public void Delete(Bread bread)
        {
            using (var context = CreateContext())
            {
                var databaseBread = context.Breads.Find(bread.Id);
                databaseBread.Deleted = DateTime.Now;
                context.SaveChanges();
            }
        }

        public IList<Bread> ListAvailableBreads()
        {
            IList<Bread> result;

            using (var context = CreateContext())
            {
                var now = DateTime.Now;
                var queryResult = from bread in context.Breads
                                  where bread.Created < now && bread.Deleted > now
                                  select bread;
                result = queryResult.ToList();
            }

            return result;
        }

        private WolfBookingContext CreateContext()
        {
            return new WolfBookingContext(_databaseConnectionString);
        }
    }
}
