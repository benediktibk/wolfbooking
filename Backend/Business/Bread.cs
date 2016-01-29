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

        public Bread(string name, decimal price)
        {
            Name = name;
            Price = price;
            Deleted = DateTime.MaxValue;
        }

        public int Id { get; private set; }
        public string Name { get; private set; }
        public decimal Price { get; private set; }
        public DateTime Deleted { get; private set; }

        public Persistence.Bread ToPersistence()
        {
            return new Persistence.Bread { Id = Id, Name = Name, Price = Price, Deleted = Deleted};
        }

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
