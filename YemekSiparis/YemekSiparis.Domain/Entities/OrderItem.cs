using YemekSiparis.Domain.Entities;

public class OrderItem
{
    public int Id { get; set; }
    public int OrderId { get; set; }
    public int ProductId { get; set; }
    public int Quantity { get; set; }

    public Product Product { get; set; } = null!;
    public List<OrderItem> OrderItems { get; set; } = new();

}
