namespace YemekSiparis.Domain.Entities
{
    public class Store
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string PasswordHash { get; set; }
        public string? Role { get; set; } = "Store";
        public string Category { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public int TotalOrders { get; set; } = 0;

        public string? ImageUrl { get; set; } 
        public int OrderCount { get; set; }   
        public ICollection<Product> Products { get; set; } = new List<Product>();
        public string Phone { get; set; }
        public string Address { get; set; }
        public string Description { get; set; }
        public string WorkingHours { get; set; }

    }
}
