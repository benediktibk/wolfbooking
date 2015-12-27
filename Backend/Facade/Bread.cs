namespace Backend.Facade
{
    public class Bread
    {
        public Bread()
        { }

        public Bread(Business.Bread bread)
        {
            Id = bread.Id;
            Name = bread.Name;
            Price = bread.Price;
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public decimal Price { get; set; }
    }
}
