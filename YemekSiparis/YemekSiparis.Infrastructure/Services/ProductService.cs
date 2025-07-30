using Microsoft.EntityFrameworkCore;
using YemekSiparis.Application.Dtos;
using YemekSiparis.Application.Interfaces;
using YemekSiparis.Domain.Entities;
using YemekSiparis.Infrastructure.Context;

namespace YemekSiparis.Infrastructure.Services
{
    public class ProductService : IProductService
    {
        private readonly YemekSiparisDbContext _context;

        public ProductService(YemekSiparisDbContext context)
        {
            _context = context;
        }

        public async Task AddProductAsync(int storeId, CreateProductDto productDto)
        {
            var product = new Product
            {
                StoreId = storeId,
                Name = productDto.Name,
                Description = productDto.Description,
                Price = productDto.Price,
                Stock = productDto.Stock,
                ImageUrl = productDto.ImageUrl
            };

            _context.Products.Add(product);
            await _context.SaveChangesAsync();
        }

        public async Task<List<ProductDto>> GetProductsByStoreAsync(int storeId)
        {
            return await _context.Products
                .Where(p => p.StoreId == storeId)
                .Select(p => new ProductDto
                {
                    Id = p.Id,
                    StoreId = p.StoreId,
                    Name = p.Name,
                    Description = p.Description,
                    Price = p.Price,
                    Stock = p.Stock,
                    ImageUrl = p.ImageUrl
                })
                .ToListAsync();
        }

        public async Task UpdateProductAsync(int productId, UpdateProductDto dto)
        {
            var product = await _context.Products.FindAsync(productId);
            if (product == null)
                throw new Exception("Ürün bulunamadı.");

            product.Name = dto.Name;
            product.Price = dto.Price;
            product.Stock = dto.Stock;
            product.Description = dto.Description;
            product.ImageUrl = dto.ImageUrl;

            _context.Products.Update(product);
            await _context.SaveChangesAsync();
        }

        public async Task<List<ProductDto>> GetMyProductsAsync(int storeId)
        {
            return await GetProductsByStoreAsync(storeId);
        }

        public async Task<List<ProductDto>> GetByStoreId(int storeId)
        {
            return await GetProductsByStoreAsync(storeId);
        }
    }
}
