using StockApp.Application.DTO;
using StockApp.Application.ServiceContracts;

namespace StockApp.Infrastructure.Database.Services;

public class AccountService : IAccountService
{
    private readonly ApplicationDbContext _dbContext;

    public AccountService(ApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public LoginResponse Login(LoginRequest loginRequest)
    {
        var user = _dbContext.Users.FirstOrDefault(u => u.Email == loginRequest.Email);
        
        // Mock password "123" to match InMemory logic since Domain Entity doesn't have a password column yet
        if (user != null && loginRequest.Password == "123")
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
}
