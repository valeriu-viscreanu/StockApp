using StockApp.Application.DTO;
using StockApp.Application.ServiceContracts;
using StockApp.Infrastructure.Database;
using System;
using System.Linq;

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
            var user = _db.Users.FirstOrDefault(u => u.Email == loginRequest.Email);
            if (user != null && loginRequest.Password == "123") // Hardcoded for tutorial
            {
                return new LoginResponse
                {
                    IsSuccess = true,
                    UserID = user.UserID,
                    Email = user.Email
                };
            }
            return new LoginResponse { IsSuccess = false };
        }

        public RegisterResponse Register(RegisterRequest registerRequest)
        {
            return new RegisterResponse { IsSuccess = true };
        }
    }
}
