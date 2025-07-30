using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YemekSiparis.Application.Dtos
{
    public class StoreDto
    {
        public string Email { get; set; }
        public string Password { get; set; }
        public string Category { get; set; }
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public List<ProductDto>? Products { get; set; }
        public string Description { get; set; }
        public string? BannerImageUrl { get; set; }
        public string Address { get; set; }
        public string WorkingHours { get; set; }
        public string Phone { get; set; }
        public string? ImageUrl { get; set; }
        public int OrderCount { get; set; }



    }
}
