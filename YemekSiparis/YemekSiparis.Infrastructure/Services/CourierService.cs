using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.EntityFrameworkCore;
using YemekSiparis.Application.Dtos;
using YemekSiparis.Application.Interfaces;
using YemekSiparis.Domain.Entities;
using YemekSiparis.Infrastructure.Context;

public class CourierService : ICourierService
{
    private readonly YemekSiparisDbContext _context;

    public CourierService(YemekSiparisDbContext context)
    {
        _context = context;
    }

    public async Task<List<CourierDto>> GetAllCouriersAsync()
    {
        var couriers = await _context.Couriers.ToListAsync();

        return couriers.Select(c => new CourierDto
        {
            Id = c.Id,
            FullName = c.FullName,
            PhoneNumber = c.PhoneNumber
        }).ToList();
    }

    public async Task AssignCourierAsync(AssignCourierDto dto)
    {
        var order = await _context.Orders.FindAsync(dto.OrderId);
        if (order == null)
            throw new Exception("Sipariş bulunamadı.");

        var courier = await _context.Couriers.FindAsync(dto.CourierId);
        if (courier == null || !courier.IsAvailable)
            throw new Exception("Geçerli ve uygun bir kurye bulunamadı.");

        order.CourierId = courier.Id;
        courier.IsAvailable = false;

        await _context.SaveChangesAsync();
    }

    public async Task<List<OrderDto>> GetOrdersForCourierAsync(int courierId)
    {
        var orders = await _context.Orders
            .Include(o => o.Store)
            .Where(o => o.CourierId == courierId)
            .ToListAsync();

        return orders.Select(o => new OrderDto
        {
            Id = o.Id,
            StoreId = o.StoreId,
            UserId = o.UserId,
            OrderDate = o.OrderDate,
            Status = o.Status.ToString(),
            StoreName = o.Store.Name
        }).ToList();
    }
}
