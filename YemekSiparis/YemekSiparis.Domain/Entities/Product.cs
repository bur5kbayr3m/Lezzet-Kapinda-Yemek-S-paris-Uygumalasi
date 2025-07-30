namespace YemekSiparis.Domain.Entities
{
    public class Product
    {
        public int Id { get; set; }
        public int StoreId { get; set; }

        public string Name { get; set; } = null!;
        public string? Description { get; set; }
        public decimal Price { get; set; }
        public Store Store { get; set; } = null!;
        public int Stock { get; set; }
        public string ImageUrl { get; set; }

    }
}
