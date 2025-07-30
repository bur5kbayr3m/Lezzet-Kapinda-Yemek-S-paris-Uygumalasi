using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using YemekSiparis.Application.Dtos;
using YemekSiparis.Application.Interfaces;
using YemekSiparis.Application.Settings;
using YemekSiparis.Domain.Entities;
using YemekSiparis.Infrastructure.Context;

namespace YemekSiparis.Infrastructure.Services
{
    public class StoreService : IStoreService
    {
        private readonly YemekSiparisDbContext _context;
        private readonly JwtSettings _jwtSettings;
        private readonly IMapper _mapper;

        public StoreService(
            YemekSiparisDbContext context,
            IOptions<JwtSettings> jwtSettings,
            IMapper mapper)
        {
            _context = context;
            _jwtSettings = jwtSettings.Value;
            _mapper = mapper;
        }

        public async Task CreateStoreAsync(StoreDto storeDto)
        {
            var store = new Store
            {
                Name = storeDto.Name,
                Email = storeDto.Email,
                PasswordHash = storeDto.Password,
                Category = storeDto.Category,
                ImageUrl = storeDto.ImageUrl,
                Phone = storeDto.Phone,
                Address = storeDto.Address,
                Description = storeDto.Description,
                WorkingHours = storeDto.WorkingHours,
                CreatedAt = DateTime.UtcNow
            };

            _context.Stores.Add(store);
            await _context.SaveChangesAsync();
        }

        public async Task ForgotPasswordAsync(ForgotPasswordDto dto)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == dto.Email);
            if (user == null)
                throw new Exception("Kullanıcı bulunamadı.");
        }

        public async Task ResetPasswordAsync(ResetPasswordDto dto)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == dto.Email);
            if (user == null)
                throw new Exception("Kullanıcı bulunamadı.");
            user.PasswordHash = dto.NewPassword;
            await _context.SaveChangesAsync();
        }

        public async Task RequestUsernameChangeAsync(int storeId, RequestUsernameChangeDto dto)
        {
            var existing = await _context.UsernameChangeRequests
                .FirstOrDefaultAsync(r => r.UserId == storeId && r.UserType == "store" && r.Status == "Pending");

            if (existing != null)
                throw new Exception("Zaten bekleyen bir talebiniz var.");

            var request = new UsernameChangeRequest
            {
                UserId = storeId,
                UserType = "store",
                NewUsername = dto.NewUsername
            };

            await _context.UsernameChangeRequests.AddAsync(request);
            await _context.SaveChangesAsync();
        }

        public async Task ChangePasswordAsync(int userId, ChangePasswordDto dto)
        {
            var user = await _context.Stores.FindAsync(userId);
            if (user == null)
                throw new Exception("Kullanıcı bulunamadı.");

            if (user.PasswordHash != dto.CurrentPassword)
                throw new Exception("Mevcut şifre yanlış.");

            user.PasswordHash = dto.NewPassword;
            await _context.SaveChangesAsync();
        }

        public async Task<string> LoginAsync(StoreLoginDto loginDto)
        {
            var store = await _context.Stores
                .FirstOrDefaultAsync(s => s.Email == loginDto.Email && s.PasswordHash == loginDto.Password);

            if (store == null)
                throw new Exception("Geçersiz e-posta veya şifre.");

            var claims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier, store.Id.ToString()),
                new Claim(ClaimTypes.Name, store.Name),
                new Claim(ClaimTypes.Email, store.Email),
                new Claim("http://schemas.microsoft.com/ws/2008/06/identity/claims/role", "Store")
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSettings.Key));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: _jwtSettings.Issuer,
                audience: _jwtSettings.Audience,
                claims: claims,
                expires: DateTime.Now.AddMinutes(_jwtSettings.ExpireMinutes),
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        public async Task<List<Store>> SearchStoresAsync(string? productName, string? category, string? sortBy)
        {
            var query = _context.Stores.Include(s => s.Products).AsQueryable();

            if (!string.IsNullOrEmpty(productName))
                query = query.Where(s => s.Products.Any(p => p.Name.ToLower().Contains(productName.ToLower())));

            if (!string.IsNullOrEmpty(category))
                query = query.Where(s => s.Category.ToLower() == category.ToLower());

            if (!string.IsNullOrEmpty(sortBy))
            {
                switch (sortBy.ToLower())
                {
                    case "popular":
                        query = query.OrderByDescending(s => s.TotalOrders);
                        break;
                    case "newest":
                        query = query.OrderByDescending(s => s.CreatedAt);
                        break;
                }
            }
            return await query.ToListAsync();
        }

        public StoreDto GetById(int id)
        {
            var store = _context.Stores
                .Include(s => s.Products)
                .FirstOrDefault(s => s.Id == id);

            if (store == null)
                return null;

            return new StoreDto
            {
                Id = store.Id,
                Name = store.Name,
                Email = store.Email,
                Category = store.Category,
                Products = store.Products.Select(p => new ProductDto
                {
                    Id = p.Id,
                    Name = p.Name,
                    Price = p.Price,
                    Description = p.Description,
                    ImageUrl = p.ImageUrl
                }).ToList()
            };
        }

        public List<StoreDto> GetPopularStores()
        {
            return _context.Stores
                .OrderByDescending(x => x.OrderCount)
                .Select(s => new StoreDto
                {
                    Id = s.Id,
                    Name = s.Name,
                    ImageUrl = s.ImageUrl
                })
                .Take(5)
                .ToList();
        }

        public async Task<StoreDto> GetStorePanelInfoAsync(int storeId)
        {
            var store = await _context.Stores
                .Where(x => x.Id == storeId)
                .Select(x => new StoreDto
                {
                    Id = x.Id,
                    Name = x.Name,
                    Address = x.Address,
                    Description = x.Description,
                    Email = x.Email,
                    Phone = x.Phone,
                    ImageUrl = x.ImageUrl,
                    WorkingHours = x.WorkingHours
                })
                .FirstOrDefaultAsync();

            if (store == null)
                throw new Exception("Mağaza bulunamadı.");

            return store;
        }

        public async Task<StoreDto> GetByIdWithProductsAsync(int id)
        {
            var store = await _context.Stores
                .Include(s => s.Products)
                .FirstOrDefaultAsync(s => s.Id == id);

            if (store == null)
                return null;

            return new StoreDto
            {
                Id = store.Id,
                Name = store.Name,
                Description = store.Description,
                Products = store.Products.Select(p => new ProductDto
                {
                    Id = p.Id,
                    Name = p.Name,
                    Description = p.Description,
                    Price = p.Price,
                    ImageUrl = p.ImageUrl
                }).ToList()
            };
        }

        // En çok hata alınan yer burasıdır!
        // Interface ile birebir aynı olmalı.
        public List<StoreDto> GetAllStores()
        {
            return _context.Stores
                .Select(s => _mapper.Map<StoreDto>(s))
                .ToList();
        }
    }
}
