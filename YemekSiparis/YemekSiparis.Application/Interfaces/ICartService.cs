using YemekSiparis.Domain.Entities;

public interface ICartService
{
    Task AddToCartAsync(int storeId, CartDto cartDto);
    Task<List<Cart>> GetCartItemsAsync(int storeId);
    Task RemoveFromCartAsync(int cartItemId);
    Task UpdateCartItemAsync(int userId, UpdateCartItemDto dto);

}
