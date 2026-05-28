using StockApp.Application.DTO;
using StockApp.Application.ServiceContracts;
using StockApp.Domain.Entities;
using StockApp.Infrastructure.Database;
using System;
using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace StockApp.Infrastructure.Database.Services
{
    public class AccountAuthService : IAccountService
    {
        private readonly ApplicationDbContext _db;

        public AccountAuthService(ApplicationDbContext db)
        {
            _db = db;
        }

        public LoginResponse Login(LoginRequest loginRequest)
        {
            var user = _db.Users
                .Include(u => u.Role)
                .FirstOrDefault(u => u.Email == loginRequest.Email);
                
            // Note: Password hashing is omitted as per current tutorial state ("123" hardcode)
            if (user != null && loginRequest.Password == "123") 
            {
                return new LoginResponse
                {
                    IsSuccess = true,
                    UserID = user.UserID,
                    Email = user.Email,
                    RoleName = user.Role?.RoleName ?? "User" // Default to User if no role exists
                };
            }

            return new LoginResponse { IsSuccess = false };
        }

        public RegisterResponse Register(RegisterRequest registerRequest)
        {
            if (_db.Users.Any(u => u.Email == registerRequest.Email))
            {
                return new RegisterResponse { IsSuccess = false, ErrorMessage = "Email already exists" };
            }

            var userId = Guid.NewGuid();
            var newUser = new ApplicationUser
            {
                UserID = userId,
                Email = registerRequest.Email,
                RoleID = registerRequest.RoleID ?? Guid.Parse("10000000-0000-0000-0000-000000000001") // Default to Customer
            };

            var newAccount = new Account
            {
                AccountID = Guid.NewGuid(),
                UserID = userId,
                Balance = 0.00, // Initial balance
                DateOfBirth = registerRequest.DateOfBirth ?? DateTime.UtcNow.AddYears(-20)
            };

            var userDetails = new UserDetails
            {
                DetailsID = Guid.NewGuid(),
                UserID = userId,
                Street = registerRequest.Street,
                StreetNumber = registerRequest.StreetNumber,
                Building = registerRequest.Building,
                Unit = registerRequest.Unit,
                City = registerRequest.City,
                ZipCode = registerRequest.ZipCode,
                Country = registerRequest.Country,
                AdditionalInfo = registerRequest.AdditionalInfo,
                PhoneNumber = registerRequest.PhoneNumber
            };

            _db.Users.Add(newUser);
            _db.Accounts.Add(newAccount);
            _db.UserDetails.Add(userDetails);
            _db.SaveChanges();

            return new RegisterResponse { IsSuccess = true };
        }
    }
}
