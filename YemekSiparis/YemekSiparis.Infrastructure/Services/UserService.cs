using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using YemekSiparis.Application.Dtos;
using YemekSiparis.Application.Interfaces;
using YemekSiparis.Application.Settings;
using YemekSiparis.Domain.Entities;
using YemekSiparis.Domain.Enums;
using YemekSiparis.Infrastructure.Context;

namespace YemekSiparis.Infrastructure.Services
{
    public class UserService : IUserService
    {
        private readonly YemekSiparisDbContext _context;
        private readonly JwtSettings _jwtSettings;

        public UserService(YemekSiparisDbContext context, IOptions<JwtSettings> jwtSettings)
        {
            _context = context;
            _jwtSettings = jwtSettings.Value;
        }

        public async Task CreateUserAsync(UserDto userDto)
        {
            var user = new User
            {
                Username = userDto.Username,
                Email = userDto.Email,
                PasswordHash = userDto.Password
            };

            _context.Users.Add(user);
            await _context.SaveChangesAsync();
        }

        public async Task<string> LoginAsync(LoginDto loginDto)
        {
            var user = await _context.Users
                .FirstOrDefaultAsync(u => u.Email == loginDto.Email && u.PasswordHash == loginDto.Password);

            if (user == null)
                throw new Exception("Geçersiz e-posta veya şifre.");

            var claims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Name, user.Username),
                new Claim(ClaimTypes.Email, user.Email),
                new Claim(ClaimTypes.Role, user.Role ?? "User")
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

        public async Task ForgotPasswordAsync(ForgotPasswordDto dto)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == dto.Email);
            if (user == null)
                throw new Exception("Kullanıcı bulunamadı.");

            // Gerçek bir sıfırlama işlemi yapılacaksa burada yapılmalı
        }

        public async Task ResetPasswordAsync(ResetPasswordDto dto)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == dto.Email);
            if (user == null)
                throw new Exception("Kullanıcı bulunamadı.");

            user.PasswordHash = dto.NewPassword;
            await _context.SaveChangesAsync();
        }

        public async Task ChangePasswordAsync(int userId, ChangePasswordDto dto)
        {
            var user = await _context.Users.FindAsync(userId);
            if (user == null)
                throw new Exception("Kullanıcı bulunamadı.");

            if (user.PasswordHash != dto.CurrentPassword)
                throw new Exception("Mevcut şifre yanlış.");

            user.PasswordHash = dto.NewPassword;
            await _context.SaveChangesAsync();
        }

        public async Task RequestUsernameChangeAsync(int userId, RequestUsernameChangeDto dto)
        {
            var existing = await _context.UsernameChangeRequests
                .FirstOrDefaultAsync(r => r.UserId == userId && r.UserType == "user" && r.Status == "Pending");

            if (existing != null)
                throw new Exception("Zaten bekleyen bir talebiniz var.");

            var request = new UsernameChangeRequest
            {
                UserId = userId,
                UserType = "user",
                NewUsername = dto.NewUsername
            };

            await _context.UsernameChangeRequests.AddAsync(request);
            await _context.SaveChangesAsync();
        }

        public async Task<List<StoreReview>> GetStoreReviewsAsync(int storeId)
        {
            return await _context.StoreReviews
                .Where(r => r.StoreId == storeId)
                .OrderByDescending(r => r.CreatedAt)
                .ToListAsync();
        }

        public async Task AddStoreReviewAsync(int userId, AddStoreReviewDto dto)
        {
            var order = await _context.Orders
                .FirstOrDefaultAsync(o => o.Id == dto.OrderId && o.UserId == userId && o.StoreId == dto.StoreId);

            if (order == null)
                throw new Exception("Bu mağazadan sipariş bulunamadı.");

            var existing = await _context.StoreReviews
                .AnyAsync(r => r.OrderId == dto.OrderId);

            if (existing)
                throw new Exception("Bu sipariş için zaten yorum yapılmış.");

            var review = new StoreReview
            {
                UserId = userId,
                StoreId = dto.StoreId,
                OrderId = dto.OrderId,
                SpeedRating = dto.SpeedRating,
                TasteRating = dto.TasteRating,
                ServiceRating = dto.ServiceRating,
                Comment = dto.Comment
            };

            await _context.StoreReviews.AddAsync(review);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateOrderByUserAsync(int orderId, UpdateOrderDto dto)
        {
            var order = await _context.Orders
                .Include(o => o.OrderItems)
                .FirstOrDefaultAsync(o => o.Id == orderId);

            if (order == null)
                throw new Exception("Sipariş bulunamadı.");

            if (dto.IsPaid || order.Status == OrderStatus.Yolda || order.Status == OrderStatus.TeslimEdildi)
            {
                order.StoreNote = dto.StoreNote;
                await _context.SaveChangesAsync();
                return;
            }

            order.OrderItems = dto.OrderItems.Select(i => new OrderItem
            {
                ProductId = i.ProductId,
                Quantity = i.Quantity
            }).ToList();

            order.Status = OrderStatus.Hazirlaniyor;
            await _context.SaveChangesAsync();
        }
    }
}
