using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

        public Bread(Facade.Bread bread)
        {
            Id = bread.Id;
            Name = bread.Name;
            Price = bread.Price;
            Created = DateTime.Now;
            Deleted = DateTime.MaxValue;
        }

        public Facade.Bread ToFacade()
        {
            return new Facade.Bread { Id = Id, Name = Name, Price = Price };
        }

        public Persistence.Bread ToPersistence()
        {
            return new Persistence.Bread { Id = Id, Name = Name, Price = Price };
        }

        public int Id { get; private set; }
        public string Name { get; private set; }
        public decimal Price { get; private set; }
        public DateTime Created { get; set; }
        public DateTime Deleted { get; set; }
    }
}
