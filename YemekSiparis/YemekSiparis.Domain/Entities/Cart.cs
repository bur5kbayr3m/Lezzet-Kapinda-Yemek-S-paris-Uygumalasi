using YemekSiparis.Domain.Entities;

public class Cart
{
    public int Id { get; set; }
    public int StoreId { get; set; }
    public int ProductId { get; set; }
    public int Quantity { get; set; }
    public List<CartItem> Items { get; set; }
    public int UserId { get; set; }

    public Store Store { get; set; } = null!;
    public Product Product { get; set; } = null!;
}
