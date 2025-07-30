using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YemekSiparis.Application.Dtos;

public class OrderDetailDto
{
    public int Id { get; set; }
    public string StoreName { get; set; }
    public string Status { get; set; } 
    public string? CourierName { get; set; } 
    public List<OrderItemDto> Items { get; set; }
}
