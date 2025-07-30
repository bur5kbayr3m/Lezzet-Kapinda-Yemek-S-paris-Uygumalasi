using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Collections.Generic;
using System.Threading.Tasks;
using YemekSiparis.Domain.Entities;

public interface IFavoriteService
{
    Task AddFavoriteAsync(int userId, int storeId);
    Task RemoveFavoriteAsync(int userId, int storeId);
    Task<List<Store>> GetFavoritesAsync(int userId);
}
