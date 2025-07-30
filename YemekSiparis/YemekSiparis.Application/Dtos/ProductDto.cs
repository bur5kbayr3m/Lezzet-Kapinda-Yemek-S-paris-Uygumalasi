namespace YemekSiparis.Application.Dtos
{
    public class ProductDto
    {
        public string Name { get; set; } = null!;
        public decimal Price { get; set; }
        public string? Description { get; set; }
        public int StoreId { get; set; }

        public int Id { get; set; }
        public string ImageUrl { get; set; }
        public int Stock { get; set; }
        public string Category { get; set; }



    }
}
