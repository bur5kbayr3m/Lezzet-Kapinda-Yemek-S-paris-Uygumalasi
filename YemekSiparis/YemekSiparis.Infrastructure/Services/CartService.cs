using YemekSiparis.Infrastructure.Context;
using YemekSiparis.Domain.Entities;
using Microsoft.EntityFrameworkCore;

public class CartService : ICartService
{
    private readonly YemekSiparisDbContext _context;

    public CartService(YemekSiparisDbContext context)
    {
        _context = context;
    }

    public async Task UpdateCartItemAsync(int userId, UpdateCartItemDto dto)
    {
        var cart = await _context.Carts
            .Include(c => c.Items)
            .FirstOrDefaultAsync(c => c.UserId == userId);

        if (cart == null)
            throw new Exception("Sepet bulunamadı.");

        var product = await _context.Products.FirstOrDefaultAsync(p => p.Id == dto.ProductId);
        if (product == null)
            throw new Exception("Ürün bulunamadı.");

        var existingItem = cart.Items.FirstOrDefault(i => i.ProductId == dto.ProductId);

        if (existingItem == null)
        {
            if (dto.Quantity > 0)
            {
                if (product.Stock < dto.Quantity)
                    throw new Exception("Yeterli stok yok.");

                product.Stock -= dto.Quantity;

                cart.Items.Add(new CartItem
                {
                    ProductId = dto.ProductId,
                    Quantity = dto.Quantity
                });
            }
        }
        else
        {
            int quantityDiff = dto.Quantity - existingItem.Quantity;

            if (dto.Quantity <= 0)
            {
                product.Stock += existingItem.Quantity;
                cart.Items.Remove(existingItem);
            }
            else
            {
                if (quantityDiff > 0)
                {
                    if (product.Stock < quantityDiff)
                        throw new Exception("Yeterli stok yok.");

                    product.Stock -= quantityDiff;
                }
                else
                {
                    product.Stock += -quantityDiff;
                }

                existingItem.Quantity = dto.Quantity;
            }
        }

        _context.Products.Update(product);
        await _context.SaveChangesAsync();
    }

    public async Task AddToCartAsync(int storeId, CartDto cartDto)
    {
        var product = await _context.Products.FirstOrDefaultAsync(p => p.Id == cartDto.ProductId);
        if (product == null)
            throw new Exception("Ürün bulunamadı.");

        if (product.Stock < cartDto.Quantity)
            throw new Exception("Yeterli stok yok.");

        product.Stock -= cartDto.Quantity;

        var cartItem = new Cart
        {
            StoreId = storeId,
            ProductId = cartDto.ProductId,
            Quantity = cartDto.Quantity
        };

        _context.Carts.Add(cartItem);
        _context.Products.Update(product);
        await _context.SaveChangesAsync();
    }

    public async Task<List<Cart>> GetCartItemsAsync(int storeId)
    {
        return await _context.Carts
            .Include(c => c.Product)
            .Where(c => c.StoreId == storeId)
            .ToListAsync();
    }

    public async Task RemoveFromCartAsync(int cartItemId)
    {
        var item = await _context.Carts.FindAsync(cartItemId);
        if (item != null)
        {
            var product = await _context.Products.FirstOrDefaultAsync(p => p.Id == item.ProductId);
            if (product != null)
            {
                product.Stock += item.Quantity;
                _context.Products.Update(product);
            }

            _context.Carts.Remove(item);
            await _context.SaveChangesAsync();
        }
    }
}
