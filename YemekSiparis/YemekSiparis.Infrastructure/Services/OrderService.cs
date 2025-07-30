using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using YemekSiparis.Application.Dtos;
using YemekSiparis.Application.Interfaces;
using YemekSiparis.Infrastructure.Context;
using YemekSiparis.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace YemekSiparis.Infrastructure.Services
{
    public class OrderService : IOrderService
    {
        private readonly YemekSiparisDbContext _context;

        public OrderService(YemekSiparisDbContext context)
        {
            _context = context;
        }

        public async Task CreateOrderAsync(int storeId, CreateOrderDto dto)
        {
            var order = new Order
            {
                StoreId = storeId,
                UserId = dto.UserId,
                OrderDate = DateTime.UtcNow,
                OrderItems = dto.OrderItems.Select(item => new OrderItem
                {
                    ProductId = item.ProductId,
                    Quantity = item.Quantity
                }).ToList()
            };

            await _context.Orders.AddAsync(order);
            await _context.SaveChangesAsync();
        }

        public async Task<List<OrderDto>> GetOrdersByStoreIdAsync(int storeId)
        {
            var orders = await _context.Orders
                .Include(o => o.OrderItems)
                    .ThenInclude(i => i.Product)
                .Where(o => o.StoreId == storeId)
                .ToListAsync();

            return orders.Select(order => new OrderDto
            {
                Id = order.Id,
                OrderDate = order.OrderDate,
                Items = order.OrderItems.Select(i => new OrderItemDto
                {
                    ProductId = i.ProductId,
                    ProductName = i.Product?.Name,
                    Quantity = i.Quantity,
                    Price = i.Product?.Price ?? 0
                }).ToList()
            }).ToList();
        }

        public async Task<List<OrderDto>> GetOrdersByUserIdAsync(int userId)
        {
            var orders = await _context.Orders
                .Include(o => o.OrderItems)
                    .ThenInclude(i => i.Product)
                .Where(o => o.UserId == userId)
                .ToListAsync();

            return orders.Select(order => new OrderDto
            {
                Id = order.Id,
                OrderDate = order.OrderDate,
                Items = order.OrderItems.Select(i => new OrderItemDto
                {
                    ProductId = i.ProductId,
                    ProductName = i.Product?.Name,
                    Quantity = i.Quantity,
                    Price = i.Product?.Price ?? 0
                }).ToList()
            }).ToList();
        }

        public async Task DeleteOrderAsync(int orderId)
        {
            var order = await _context.Orders
                .Include(o => o.OrderItems)
                .FirstOrDefaultAsync(o => o.Id == orderId);

            if (order == null)
                throw new Exception("Sipariş bulunamadı.");

            _context.OrderItems.RemoveRange(order.OrderItems);
            _context.Orders.Remove(order);
            await _context.SaveChangesAsync();
        }

        public async Task<OrderDetailDto?> GetOrderDetailAsync(int orderId)
        {
            var order = await _context.Orders
                .Include(o => o.Store)
                .Include(o => o.Courier)
                .Include(o => o.OrderItems)
                    .ThenInclude(i => i.Product)
                .FirstOrDefaultAsync(o => o.Id == orderId);

            if (order == null)
                return null;

            return new OrderDetailDto
            {
                Id = order.Id,
                StoreName = order.Store?.Name,
                Status = order.Status.ToString(),
                CourierName = order.Courier?.FullName,
                Items = order.OrderItems.Select(i => new OrderItemDto
                {
                    ProductName = i.Product?.Name,
                    Quantity = i.Quantity,
                    Price = i.Product?.Price ?? 0
                }).ToList()
            };
        }
        public async Task UpdateOrderStatusAsync(int orderId, string status)
        {
            var order = await _context.Orders.FirstOrDefaultAsync(o => o.Id == orderId);
            if (order == null)
                throw new Exception("Sipariş bulunamadı.");

            if (Enum.TryParse<YemekSiparis.Domain.Enums.OrderStatus>(status, out var parsedStatus))
            {
                order.Status = parsedStatus;
                await _context.SaveChangesAsync();
            }
            else
            {
                throw new Exception("Geçersiz sipariş durumu.");
            }
        }

    }
}
