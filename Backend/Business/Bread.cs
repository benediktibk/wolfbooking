using System;

namespace Backend.Business
{
    public class Bread
    {
        public Bread(Persistence.Bread bread)
        {
            Id = bread.Id;
            Name = bread.Name;
            Price = bread.Price;
            Deleted = bread.Deleted;
        }

        public Bread(Facade.Bread bread)
        {
            Name = bread.Name;
            Price = bread.Price;
            Deleted = DateTime.MaxValue;
        }

        public int Id { get; private set; }
        public string Name { get; private set; }
        public decimal Price { get; private set; }
        public DateTime Deleted { get; private set; }

        public void UpdateWith(Facade.Bread bread)
        {
            if (bread.Id != Id)
                throw new ArgumentException("bread", "wrong id");

            Name = bread.Name;
            Price = bread.Price;
        }

        public void MarkAsDeleted()
        {
            Deleted = DateTime.Now;
        }
    }
}
