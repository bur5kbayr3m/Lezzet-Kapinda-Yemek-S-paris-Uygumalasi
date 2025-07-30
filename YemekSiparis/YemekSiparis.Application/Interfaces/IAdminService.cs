using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YemekSiparis.Application.Dtos;

public interface IAdminService
{
    Task RegisterAsync(AdminRegisterDto dto);
    Task<string> LoginAsync(AdminLoginDto dto);
    Task DeleteStoreAsync(int storeId);
    Task UpdateStoreAsync(int storeId, UpdateStoreDto dto);
    Task DeleteUserAsync(int userId);
    Task UpdateUserAsync(int userId, UpdateUserDto dto);
    Task UpdateOrderStatusAsync(int orderId, UpdateOrderDto dto);
    Task<List<OrderReportDto>> GetOrderReportsAsync(DateTime? startDate, DateTime? endDate);
    Task<List<UserDto>> GetAllUsersAsync();
    Task<List<StoreDto>> GetAllStoresAsync();
    Task<UserDto?> GetUserByUsernameAsync(string username);
    Task<StoreDto?> GetStoreByNameAsync(string storeName);


}
