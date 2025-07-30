using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using YemekSiparis.Application.Dtos;

namespace YemekSiparis.Application.Interfaces
{
    public interface IOrderService
    {
        Task CreateOrderAsync(int storeId, CreateOrderDto dto);
        Task<List<OrderDto>> GetOrdersByStoreIdAsync(int storeId);
        Task<List<OrderDto>> GetOrdersByUserIdAsync(int userId);
        Task DeleteOrderAsync(int orderId);
        Task<OrderDetailDto?> GetOrderDetailAsync(int orderId);
        Task UpdateOrderStatusAsync(int orderId, string status);




    }
}
