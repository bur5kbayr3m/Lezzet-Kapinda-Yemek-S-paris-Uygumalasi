using System.Threading.Tasks;
using YemekSiparis.Application.Dtos;
using YemekSiparis.Domain.Entities;


namespace YemekSiparis.Application.Interfaces
{
    public interface IStoreService
    {
        Task CreateStoreAsync(StoreDto storeDto);
        Task<string> LoginAsync(StoreLoginDto loginDto);
        Task<List<Store>> SearchStoresAsync(string? productName, string? category, string? sortBy);
        Task ForgotPasswordAsync(ForgotPasswordDto dto);
        Task ResetPasswordAsync(ResetPasswordDto dto);
        Task RequestUsernameChangeAsync(int userId, RequestUsernameChangeDto dto);
        Task ChangePasswordAsync(int userId, ChangePasswordDto dto);
        StoreDto GetById(int id);
        List<StoreDto> GetPopularStores();
        Task<StoreDto> GetStorePanelInfoAsync(int storeId);
        Task<StoreDto> GetByIdWithProductsAsync(int storeId);
        List<StoreDto> GetAllStores();

    }
}
