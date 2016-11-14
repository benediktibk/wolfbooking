using System;
using System.Linq;
using System.Collections.Generic;

namespace Backend.Persistence
{
    public class BreadRepository
    {
        private readonly WolfBookingContext _dbContext;

        public BreadRepository(WolfBookingContext dbContext)
        {
            _dbContext = dbContext;
        }

        public int Add(Business.Bread bread)
        {
            var persistenceBread = new Bread();
            persistenceBread.UpdateWith(bread);
            _dbContext.Breads.Add(persistenceBread);
            _dbContext.SaveChanges();

            return persistenceBread.Id;
        }

        public void Update(Business.Bread bread)
        {
            var persistenceBread = _dbContext.Breads.Find(bread.Id);

            if (persistenceBread == null)
                throw new ArgumentException("bread", $"bread with id {bread.Id} does not exist");

            _dbContext.Breads.Attach(persistenceBread);
            persistenceBread.UpdateWith(bread);
            _dbContext.SaveChanges();
        }

        public IList<Business.Bread> Get(IEnumerable<int> ids)
        {
            var queryResult = _dbContext.Breads.Where(x => ids.Contains(x.Id));
            IList<Bread> breads = queryResult?.ToList(); 

            return breads?.Select(x => new Business.Bread(x)).ToList();
        }

        public Business.Bread Get(int id)
        {
            return new Business.Bread(_dbContext.Breads.Find(id));
        }

        public IList<Business.Bread> GetCurrentAvailableBreads()
        {
            return GetAvailableBreads(DateTime.Now);
        }

        public IList<Business.Bread> GetAvailableBreads(DateTime dateTime)
        {
            var result = from bread in _dbContext.Breads
                where bread.Deleted > dateTime
                select bread;


            return result.Select(x => new Business.Bread(x)).ToList();
        }
    }
}
