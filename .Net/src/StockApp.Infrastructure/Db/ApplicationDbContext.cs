using Microsoft.EntityFrameworkCore;
using StockApp.Domain.Entities;

namespace StockApp.Infrastructure.Db;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }

    public DbSet<BuyOrder> BuyOrders { get; set; } = null!;
    public DbSet<SellOrder> SellOrders { get; set; } = null!;
    public DbSet<ApplicationUser> Users { get; set; } = null!;
    public DbSet<RefreshToken> RefreshTokens { get; set; } = null!;

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        
        // Configuration for BuyOrder
        modelBuilder.Entity<BuyOrder>(entity =>
        {
            entity.HasKey(e => e.BuyOrderID);
            entity.Property(e => e.StockSymbol).IsRequired().HasMaxLength(10);
            entity.Property(e => e.StockName).IsRequired().HasMaxLength(100);
            entity.Property(e => e.Price).HasColumnType("decimal(18,4)");
        });

        // Configuration for SellOrder
        modelBuilder.Entity<SellOrder>(entity =>
        {
            entity.HasKey(e => e.SellOrderID);
            entity.Property(e => e.StockSymbol).IsRequired().HasMaxLength(10);
            entity.Property(e => e.StockName).IsRequired().HasMaxLength(100);
            entity.Property(e => e.Price).HasColumnType("decimal(18,4)");
        });

        // Configuration for ApplicationUser
        modelBuilder.Entity<ApplicationUser>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Email).IsRequired().HasMaxLength(256);
            entity.Property(e => e.PasswordHash).IsRequired();
            entity.Property(e => e.CashBalance).HasColumnType("decimal(18,4)");
        });

        // Configuration for RefreshToken
        modelBuilder.Entity<RefreshToken>(entity =>
        {
            entity.HasKey(e => e.Token);
            entity.Property(e => e.Email).IsRequired().HasMaxLength(256);
        });
    }
}
