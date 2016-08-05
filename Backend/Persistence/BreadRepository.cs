using System;
using System.Linq;
using System.Collections.Generic;

namespace Backend.Persistence
{
    public class BreadRepository
    {
        private WolfBookingContextFactory _contextFactory;

        public BreadRepository(WolfBookingContextFactory contextFactory)
        {
            _contextFactory = contextFactory;
        }

        public int Add(Business.Bread bread)
        {
            Bread persistenceBread;

            using (var context = CreateContext())
            {
                persistenceBread = new Bread();
                persistenceBread.UpdateWith(bread);
                context.Breads.Add(persistenceBread);
                context.SaveChanges();
            }

            return persistenceBread.Id;
        }

        public void Update(Business.Bread bread)
        {
            using (var context = CreateContext())
            {
                var persistenceBread = context.Breads.Find(bread.Id);

                if (persistenceBread == null)
                    throw new ArgumentException("bread", $"bread with id {bread.Id} does not exist");

                context.Breads.Attach(persistenceBread);
                persistenceBread.UpdateWith(bread);
                context.SaveChanges();
            }
        }

        public IList<Business.Bread> Get(IEnumerable<int> ids)
        {
            IList<Bread> breads;

            using (var context = CreateContext())
            {
                var queryResult = context.Breads.Where(x => ids.Contains(x.Id));
                breads = queryResult.ToList(); 
            }

            return breads.Select(x => new Business.Bread(x)).ToList();
        }

        public Business.Bread Get(int id)
        {
            using (var context = CreateContext())
                return new Business.Bread(context.Breads.Find(id));
        }

        public IList<Business.Bread> GetCurrentAvailableBreads()
        {
            return GetAvailableBreads(DateTime.Now);
        }

        public IList<Business.Bread> GetAvailableBreads(DateTime dateTime)
        {
            IList<Bread> result;

            using (var context = CreateContext())
            {
                var queryResult = from bread in context.Breads
                                  where bread.Deleted > dateTime
                                  select bread;

                result = queryResult.ToList();
            }

            return result.Select(x => new Business.Bread(x)).ToList();
        }

        private WolfBookingContext CreateContext()
        {
            return _contextFactory.Create();
        }
    }
}
