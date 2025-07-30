using YemekSiparis.Domain.Entities;
using YemekSiparis.Domain.Enums;

public class Order
{
    public int Id { get; set; }
    public int StoreId { get; set; }
    public DateTime OrderDate { get; set; }

    public List<OrderItem> OrderItems { get; set; } = new(); 
    public int UserId { get; set; }
    public User User { get; set; }
    public OrderStatus Status { get; set; } = OrderStatus.Hazirlaniyor;
    public bool IsPaid { get; set; } = false;
    public string? StoreNote { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public Store Store { get; set; }
    public decimal TotalPrice { get; set; }
    public int? CourierId { get; set; }
    public Courier? Courier { get; set; }
}
