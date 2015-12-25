namespace Frontend.Models
{
    public class Bread
    {
        public Bread()
        { }

        public Bread(Backend.Business.Bread bread)
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