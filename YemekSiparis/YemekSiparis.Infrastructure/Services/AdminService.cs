using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using YemekSiparis.Application.Dtos;
using YemekSiparis.Application.Interfaces;
using YemekSiparis.Domain.Entities;
using YemekSiparis.Domain.Enums;
using YemekSiparis.Infrastructure.Context;

public class AdminService : IAdminService
{
    private readonly YemekSiparisDbContext _context;

    public AdminService(YemekSiparisDbContext context)
    {
        _context = context;
    }
    public async Task UpdateStoreAsync(int storeId, UpdateStoreDto dto)
    {
        var store = await _context.Stores.FirstOrDefaultAsync(s => s.Id == storeId);
        if (store == null)
            throw new Exception("Mağaza bulunamadı.");

        store.Name = dto.Name;
        store.Email = dto.Email;
        if (dto.Category != null)
            store.Category = dto.Category;

        _context.Stores.Update(store);
        await _context.SaveChangesAsync();
    }


    public async Task RegisterAsync(AdminRegisterDto dto)
    {
        var admin = new Admin { Email = dto.Email, Password = dto.Password };
        await _context.Admins.AddAsync(admin);
        await _context.SaveChangesAsync();
    }

    public async Task<string> LoginAsync(AdminLoginDto dto)
    {
        var admin = await _context.Admins
            .FirstOrDefaultAsync(a => a.Email == dto.Email && a.Password == dto.Password);

        if (admin == null)
            throw new Exception("Invalid login.");

        return admin.Id.ToString();
    }
    public async Task DeleteUserAsync(int userId)
    {
        var user = await _context.Users.FirstOrDefaultAsync(u => u.Id == userId);

        if (user == null)
            throw new Exception("Kullanıcı bulunamadı.");

        _context.Users.Remove(user);
        await _context.SaveChangesAsync();
    }
    public async Task UpdateUserAsync(int userId, UpdateUserDto dto)
    {
        var user = await _context.Users.FirstOrDefaultAsync(u => u.Id == userId);

        if (user == null)
            throw new Exception("Kullanıcı bulunamadı.");

        user.Username = dto.FullName;
        user.Email = dto.Email;
        user.PasswordHash = dto.Password;

        _context.Users.Update(user);
        await _context.SaveChangesAsync();
    }
    public async Task UpdateOrderStatusAsync(int orderId, UpdateOrderDto dto)
    {
        var order = await _context.Orders.FirstOrDefaultAsync(o => o.Id == orderId);
        if (order == null)
            throw new Exception("Sipariş bulunamadı.");


        if (!Enum.TryParse<OrderStatus>(dto.Status, true, out var parsedStatus))
            throw new Exception("Geçersiz sipariş durumu.");

        order.Status = parsedStatus;

        _context.Orders.Update(order);
        await _context.SaveChangesAsync();
    }
    public async Task DeleteStoreAsync(int storeId)
    {
        var store = await _context.Stores
            .Include(s => s.Products)
            .FirstOrDefaultAsync(s => s.Id == storeId);

        if (store == null)
            throw new Exception("Mağaza bulunamadı.");

        _context.Products.RemoveRange(store.Products);


        _context.Stores.Remove(store);
        await _context.SaveChangesAsync();
    }
    public async Task<List<UserDto>> GetAllUsersAsync()
    {
        return await _context.Users
            .Select(u => new UserDto
            {
                Id = u.Id,
                Username = u.Username,
                Email = u.Email
            }).ToListAsync();
    }

    public async Task<List<StoreDto>> GetAllStoresAsync()
    {
        return await _context.Stores
            .Select(s => new StoreDto
            {
                Id = s.Id,
                Name = s.Name,
                Email = s.Email,
                Category = s.Category
            }).ToListAsync();
    }

    public async Task<UserDto?> GetUserByUsernameAsync(string username)
    {
        var user = await _context.Users.FirstOrDefaultAsync(u => u.Username == username);
        if (user == null) return null;

        return new UserDto
        {
            Id = user.Id,
            Username = user.Username,
            Email = user.Email
        };
    }

    public async Task<StoreDto?> GetStoreByNameAsync(string storeName)
    {
        var store = await _context.Stores.FirstOrDefaultAsync(s => s.Name == storeName);
        if (store == null) return null;

        return new StoreDto
        {
            Id = store.Id,
            Name = store.Name,
            Email = store.Email,
            Category = store.Category
        };
    }
    public async Task<List<OrderReportDto>> GetOrderReportsAsync(DateTime? startDate, DateTime? endDate)
    {
        var ordersQuery = _context.Orders.AsQueryable();

        if (startDate.HasValue)
            ordersQuery = ordersQuery.Where(o => o.CreatedAt >= startDate.Value);

        if (endDate.HasValue)
            ordersQuery = ordersQuery.Where(o => o.CreatedAt <= endDate.Value);

        var storeReports = await ordersQuery
            .GroupBy(o => new { o.StoreId, o.Store.Name })
            .Select(g => new OrderReportDto
            {
                StoreId = g.Key.StoreId,
                StoreName = g.Key.Name,
                TotalOrders = g.Count(),
                TotalRevenue = g.Sum(o => o.TotalPrice),

                AverageSpeedRating = _context.StoreReviews
                    .Where(r => r.StoreId == g.Key.StoreId)
                    .Average(r => (double?)r.SpeedRating) ?? 0,

                AverageTasteRating = _context.StoreReviews
                    .Where(r => r.StoreId == g.Key.StoreId)
                    .Average(r => (double?)r.TasteRating) ?? 0,

                AverageServiceRating = _context.StoreReviews
                    .Where(r => r.StoreId == g.Key.StoreId)
                    .Average(r => (double?)r.ServiceRating) ?? 0
            })
            .ToListAsync();

        return storeReports;
    }




}
