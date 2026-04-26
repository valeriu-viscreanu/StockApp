using StockApp.Application.DTO;
using StockApp.Application.ServiceContracts;
using StockApp.Infrastructure.Database.Services;
using StockApp.Infrastructure.Database;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading.Tasks;
using Xunit;

namespace StockAppTests
{
    public class AccountServiceTests
    {
        private readonly IAccountService _accountService;
        private readonly ApplicationDbContext _db;

        public AccountServiceTests()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;
            _db = new ApplicationDbContext(options);
            _accountService = new AccountAuthService(_db);
        }

        [Fact]
        public void Register_ValidRequest_ReturnsSuccess()
        {
            // Arrange
            var request = new RegisterRequest
            {
                Name = "Test User",
                Email = "test@example.com",
                Password = "123",
                DateOfBirth = DateTime.UtcNow.AddYears(-25)
            };

            // Act
            var response = _accountService.Register(request);

            // Assert
            Assert.True(response.IsSuccess);
            Assert.True(_db.Users.Any(u => u.Email == "test@example.com"));
            
            var user = _db.Users.First(u => u.Email == "test@example.com");
            var account = _db.Accounts.First(a => a.UserID == user.UserID);
            Assert.Equal(0.00, account.Balance);
        }

        [Fact]
        public void Login_ValidCredentials_ReturnsSuccess()
        {
            // Arrange
            var email = "login@example.com";
            _accountService.Register(new RegisterRequest
            {
                Name = "Login User",
                Email = email,
                Password = "123",
                DateOfBirth = DateTime.UtcNow.AddYears(-25)
            });

            var loginRequest = new LoginRequest
            {
                Email = email,
                Password = "123"
            };

            // Act
            var response = _accountService.Login(loginRequest);

            // Assert
            Assert.True(response.IsSuccess);
            Assert.Equal(email, response.Email);
        }

        [Fact]
        public void Login_InvalidPassword_ReturnsFailure()
        {
            // Arrange
            var email = "wrongpass@example.com";
            _accountService.Register(new RegisterRequest
            {
                Name = "Wrong Pass User",
                Email = email,
                Password = "123",
                DateOfBirth = DateTime.UtcNow.AddYears(-25)
            });

            var loginRequest = new LoginRequest
            {
                Email = email,
                Password = "wrong"
            };

            // Act
            var response = _accountService.Login(loginRequest);

            // Assert
            Assert.False(response.IsSuccess);
        }

        [Fact]
        public void Register_DuplicateEmail_ReturnsFailure()
        {
            // Arrange
            var email = "duplicate@example.com";
            var request = new RegisterRequest
            {
                Name = "First User",
                Email = email,
                Password = "123",
                DateOfBirth = DateTime.UtcNow.AddYears(-25)
            };
            _accountService.Register(request);

            var duplicateRequest = new RegisterRequest
            {
                Name = "Second User",
                Email = email,
                Password = "123",
                DateOfBirth = DateTime.UtcNow.AddYears(-25)
            };

            // Act
            var response = _accountService.Register(duplicateRequest);

            // Assert
            Assert.False(response.IsSuccess);
            Assert.Equal("Email already exists", response.ErrorMessage);
        }
    }
}
