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
                context.Breads.Add(bread);
                context.SaveChanges();
            }
        }

        public void UpdateDates(Bread bread)
        {
            using (var context = CreateContext())
            {
                context.Breads.Attach(bread);
                context.Entry(bread).Property(x => x.Created).IsModified = true;
                context.Entry(bread).Property(x => x.Deleted).IsModified = true;
                context.SaveChanges();
            }
        }

        public IList<Bread> GetAvailableBreads(DateTime dateTime)
        {
            IList<Bread> result;

            using (var context = CreateContext())
            {
                var queryResult = from bread in context.Breads
                                  where bread.Created < dateTime && bread.Deleted > dateTime
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
