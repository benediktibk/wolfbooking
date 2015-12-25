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
            Created = bread.Created;
            Deleted = bread.Deleted;
        }

        public Bread(string name, decimal price)
        {
            Name = name;
            Price = price;
            Created = DateTime.Now;
            Deleted = DateTime.MaxValue;
        }

        public Persistence.Bread ToPersistence()
        {
            return new Persistence.Bread { Id = Id, Name = Name, Price = Price };
        }

        public int Id { get; private set; }
        public string Name { get; private set; }
        public decimal Price { get; private set; }
        public DateTime Created { get; private set; }
        public DateTime Deleted { get; private set; }
    }
}
