using YemekSiparis.Application.Dtos;

namespace YemekSiparis.Application.Interfaces
{
    public interface IProductService
    {
        Task AddProductAsync(int storeId, CreateProductDto productDto);
        Task<List<ProductDto>> GetProductsByStoreAsync(int storeId);
        Task<List<ProductDto>> GetMyProductsAsync(int storeId);
        Task<List<ProductDto>> GetByStoreId(int storeId);
        Task UpdateProductAsync(int productId, UpdateProductDto dto);

    }
}
