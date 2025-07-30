using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using YemekSiparis.Application.Interfaces;
using YemekSiparis.Domain.Entities;
using YemekSiparis.Infrastructure.Context;

public class FavoriteService : IFavoriteService
{
    private readonly YemekSiparisDbContext _context;

    public FavoriteService(YemekSiparisDbContext context)
    {
        _context = context;
    }

    public async Task AddFavoriteAsync(int userId, int storeId)
    {
        var alreadyExists = await _context.FavoriteStores
            .AnyAsync(f => f.UserId == userId && f.StoreId == storeId);

        if (alreadyExists)
            throw new Exception("Bu mağaza zaten favorilere eklenmiş.");

        var fav = new FavoriteStore
        {
            UserId = userId,
            StoreId = storeId
        };

        _context.FavoriteStores.Add(fav);
        await _context.SaveChangesAsync();
    }

    public async Task RemoveFavoriteAsync(int userId, int storeId)
    {
        var favorite = await _context.FavoriteStores
            .FirstOrDefaultAsync(f => f.UserId == userId && f.StoreId == storeId);

        if (favorite == null)
            throw new Exception("Favoride böyle bir mağaza yok.");

        _context.FavoriteStores.Remove(favorite);
        await _context.SaveChangesAsync();
    }

    public async Task<List<Store>> GetFavoritesAsync(int userId)
    {
        return await _context.FavoriteStores
            .Include(f => f.Store)
            .Where(f => f.UserId == userId)
            .Select(f => f.Store)
            .ToListAsync();
    }
}
