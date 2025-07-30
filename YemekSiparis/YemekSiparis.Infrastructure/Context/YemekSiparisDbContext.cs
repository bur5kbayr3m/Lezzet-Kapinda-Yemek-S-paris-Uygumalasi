using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using YemekSiparis.Domain.Entities;

namespace YemekSiparis.Infrastructure.Context;

public class YemekSiparisDbContext : DbContext
{
    public YemekSiparisDbContext(DbContextOptions<YemekSiparisDbContext> options)
    : base(options) { }

    public DbSet<User> Users => Set<User>();
    public DbSet<Store> Stores { get; set; }
    public DbSet<Product> Products { get; set; }
    public DbSet<Cart> Carts { get; set; }
    public DbSet<Order> Orders { get; set; }
    public DbSet<OrderItem> OrderItems { get; set; }
    public DbSet<Admin> Admins { get; set; }
    public DbSet<FavoriteStore> FavoriteStores { get; set; }
    public DbSet<CartItem> CartItems { get; set; }
    public DbSet<UsernameChangeRequest> UsernameChangeRequests { get; set; }
    public DbSet<StoreReview> StoreReviews { get; set; }

    public DbSet<Courier> Couriers { get; set; }


    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Cart>()
            .HasMany(c => c.Items)
            .WithOne(ci => ci.Cart)
            .HasForeignKey(ci => ci.CartId);

        base.OnModelCreating(modelBuilder);
    }


}
