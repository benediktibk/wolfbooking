using System;

namespace Backend.Persistence
{
    public class Bread
    {
        public Bread()
        {

        }

        public int Id { get; set; }
        public string Name { get; set; }
        public decimal Price { get; set; }
        public DateTime Created { get; set; }
        public DateTime Deleted { get; set; }
    }
}
