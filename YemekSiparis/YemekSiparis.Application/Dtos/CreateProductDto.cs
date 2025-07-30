namespace YemekSiparis.Application.Dtos
{
    public class CreateProductDto
    {
        public string Name { get; set; } = null!;
        public string? Description { get; set; }
        public decimal Price { get; set; }
        public decimal? DiscountPrice { get; set; }
        public int Stock { get; set; }
        public string Category { get; set; } = null!;
        public string? ImageUrl { get; set; }
        public string? Tags { get; set; } // örn: "vegan,acılı,glutensiz"
        public int? Calories { get; set; }
        public int? PreparationTime { get; set; } 
        public string? Allergens { get; set; }
        public string? Ingredients { get; set; }
        public bool? IsVegan { get; set; }
        public bool? IsVegetarian { get; set; }
        public string? Unit { get; set; }
        public bool? IsActive { get; set; }
        public int? DisplayOrder { get; set; }
    }
}
