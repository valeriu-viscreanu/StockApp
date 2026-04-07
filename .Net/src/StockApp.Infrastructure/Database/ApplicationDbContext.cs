using Microsoft.EntityFrameworkCore;
using StockApp.Domain.Entities;
using StockApp.Domain.Enums;

namespace StockApp.Infrastructure.Database;

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
    public DbSet<UserBalance> UserBalances { get; set; } = null!;
    public DbSet<UserOperation> UserOperations { get; set; } = null!;
    public DbSet<UserHolding> UserHoldings { get; set; } = null!;

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
            entity.HasKey(e => e.UserID);
            entity.Property(e => e.Email).IsRequired().HasMaxLength(256);

            entity.HasMany(u => u.BuyOrders)
                .WithOne()
                .HasForeignKey(bo => bo.UserID);

            entity.HasMany(u => u.SellOrders)
                .WithOne()
                .HasForeignKey(so => so.UserID);

            entity.HasData(
                new ApplicationUser
                {
                    UserID = Guid.Parse("00000000-0000-0000-0000-000000000001"),
                    Email = "admin@test.com"
                },
                new ApplicationUser
                {
                    UserID = Guid.Parse("00000000-0000-0000-0000-000000000002"),
                    Email = "admin1@test.com"
                }
            );
        });

        // Configuration for UserBalance
        modelBuilder.Entity<UserBalance>(entity =>
        {
            entity.HasKey(e => e.UserID);
            entity.Property(e => e.Balance).HasColumnType("decimal(18,4)");

            entity.HasOne<ApplicationUser>()
                .WithOne()
                .HasForeignKey<UserBalance>(e => e.UserID);

            entity.HasData(
                new UserBalance
                {
                    UserID = Guid.Parse("00000000-0000-0000-0000-000000000001"),
                    Balance = 1000.00
                },
                new UserBalance
                {
                    UserID = Guid.Parse("00000000-0000-0000-0000-000000000002"),
                    Balance = 1000.00
                }
            );
        });

        // Configuration for RefreshToken
        modelBuilder.Entity<RefreshToken>(entity =>
        {
            entity.HasKey(e => e.Token);
            entity.Property(e => e.Email).IsRequired().HasMaxLength(256);
        });

        // Configuration for UserOperation
        modelBuilder.Entity<UserOperation>(entity =>
        {
            entity.HasKey(e => e.UserOperationID);
            entity.Property(e => e.OperationType)
                .IsRequired()
                .HasMaxLength(50)
                .HasConversion<string>();
            entity.Property(e => e.StockSymbol).HasMaxLength(10);
            entity.Property(e => e.Amount).HasColumnType("decimal(18,4)");
            entity.Property(e => e.TimeStamp).IsRequired();

            entity.HasOne<ApplicationUser>()
                .WithMany(u => u.UserOperations)
                .HasForeignKey(uo => uo.UserID);
        });

        // Configuration for UserHolding
        modelBuilder.Entity<UserHolding>(entity =>
        {
            entity.HasKey(e => e.HoldingID);
            entity.Property(e => e.StockSymbol).IsRequired().HasMaxLength(10);
            entity.Property(e => e.StockName).IsRequired().HasMaxLength(100);
            
            entity.HasOne<ApplicationUser>()
                .WithMany()
                .HasForeignKey(h => h.UserID);
        });
    }
}
