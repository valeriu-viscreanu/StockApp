using StockApp.Application.DTO;
using StockApp.Application.ServiceContracts;
using StockApp.Domain.Entities;

namespace StockApp.Infrastructure.Db;

public class PersistentAccountService : IAccountService
{
    private readonly ApplicationDbContext _dbContext;

    public PersistentAccountService(ApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public LoginResponse Login(LoginRequest loginRequest)
    {
        var user = _dbContext.Users.FirstOrDefault(u => u.Email == loginRequest.Email && u.PasswordHash == loginRequest.Password);
        
        if (user != null)
        {
            return new LoginResponse
            {
                IsSuccess = true,
                UserID = user.Id,
                Email = user.Email
            };
        }

        return new LoginResponse { IsSuccess = false };
    }
}
