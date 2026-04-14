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
    public DbSet<Account> Accounts { get; set; } = null!;
    public DbSet<UserOperation> UserOperations { get; set; } = null!;
    public DbSet<Cash> CashAllocations { get; set; } = null!;
    public DbSet<UserDetails> UserDetails { get; set; } = null!;
    public DbSet<UserRole> UserRoles { get; set; } = null!;

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

            entity.HasOne(u => u.Role)
                .WithMany()
                .HasForeignKey(u => u.RoleID)
                .IsRequired(false)
                .OnDelete(DeleteBehavior.SetNull);

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

        // Configuration for UserRole
        modelBuilder.Entity<UserRole>(entity =>
        {
            entity.HasKey(e => e.RoleID);
            entity.Property(e => e.RoleName).IsRequired().HasMaxLength(50);

            entity.HasData(
                new UserRole { RoleID = Guid.Parse("10000000-0000-0000-0000-000000000001"), RoleName = "User" },
                new UserRole { RoleID = Guid.Parse("10000000-0000-0000-0000-000000000002"), RoleName = "Admin" },
                new UserRole { RoleID = Guid.Parse("10000000-0000-0000-0000-000000000003"), RoleName = "Analyst" },
                new UserRole { RoleID = Guid.Parse("10000000-0000-0000-0000-000000000004"), RoleName = "Moderator" },
                new UserRole { RoleID = Guid.Parse("10000000-0000-0000-0000-000000000005"), RoleName = "Viewer" }
            );
        });

        // Configuration for Account
        modelBuilder.Entity<Account>(entity =>
        {
            entity.HasKey(e => e.AccountID);
            entity.Property(e => e.Balance).HasColumnType("decimal(18,4)");
            entity.Property(e => e.DateOfBirth).IsRequired();

            entity.HasOne(a => a.User)
                .WithOne(u => u.Account)
                .HasForeignKey<Account>(a => a.UserID);

            entity.HasData(
                new Account
                {
                    AccountID = Guid.Parse("A0000000-0000-0000-0000-000000000001"),
                    UserID = Guid.Parse("00000000-0000-0000-0000-000000000001"),
                    Balance = 1000.00,
                    DateOfBirth = new DateTime(1990, 1, 1)
                },
                new Account
                {
                    AccountID = Guid.Parse("A0000000-0000-0000-0000-000000000002"),
                    UserID = Guid.Parse("00000000-0000-0000-0000-000000000002"),
                    Balance = 1000.00,
                    DateOfBirth = new DateTime(1995, 5, 20)
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

        // Configuration for Cash (formerly UserHolding)
        modelBuilder.Entity<Cash>(entity =>
        {
            entity.HasKey(e => e.CashID);
            entity.Property(e => e.StockSymbol).IsRequired().HasMaxLength(10);
            entity.Property(e => e.StockName).IsRequired().HasMaxLength(100);
            
            entity.HasOne(c => c.Account)
                .WithMany(a => a.CashAllocations)
                .HasForeignKey(c => c.AccountID);
        });

        // Configuration for UserDetails
        modelBuilder.Entity<UserDetails>(entity =>
        {
            entity.HasKey(e => e.DetailsID);
            entity.Property(e => e.Street).HasMaxLength(100);
            entity.Property(e => e.StreetNumber).HasMaxLength(20);
            entity.Property(e => e.Building).HasMaxLength(50);
            entity.Property(e => e.Unit).HasMaxLength(20);
            entity.Property(e => e.City).HasMaxLength(100);
            entity.Property(e => e.ZipCode).HasMaxLength(20);
            entity.Property(e => e.Country).HasMaxLength(100);
            entity.Property(e => e.AdditionalInfo).HasMaxLength(500);
            entity.Property(e => e.PhoneNumber).HasMaxLength(20);

            entity.HasOne<ApplicationUser>()
                .WithOne()
                .HasForeignKey<UserDetails>(e => e.UserID);
        });
    }
}
