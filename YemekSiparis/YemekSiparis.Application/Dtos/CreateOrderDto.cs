namespace YemekSiparis.Application.Dtos
{
    public class CreateOrderDto
    {
        public List<CreateOrderItemDto> OrderItems { get; set; }
        public int UserId { get; set; }
    }

    public class CreateOrderItemDto
    {
        public int ProductId { get; set; }
        public int Quantity { get; set; }
        

    }
}
