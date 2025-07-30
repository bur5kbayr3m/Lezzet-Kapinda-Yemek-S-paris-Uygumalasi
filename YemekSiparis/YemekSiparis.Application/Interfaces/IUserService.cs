using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YemekSiparis.Application.Dtos;

namespace YemekSiparis.Application.Interfaces
{
    public interface IUserService
    {
        Task CreateUserAsync(UserDto userDto);
        Task<string?> LoginAsync(LoginDto loginDto);
        Task UpdateOrderByUserAsync(int orderId, UpdateOrderDto dto);
        Task ForgotPasswordAsync(ForgotPasswordDto dto);
        Task ResetPasswordAsync(ResetPasswordDto dto);
        Task RequestUsernameChangeAsync(int userId, RequestUsernameChangeDto dto);
        Task ChangePasswordAsync(int userId, ChangePasswordDto dto);
        Task AddStoreReviewAsync(int userId, AddStoreReviewDto dto);
        Task<List<StoreReview>> GetStoreReviewsAsync(int storeId);




    }
}
