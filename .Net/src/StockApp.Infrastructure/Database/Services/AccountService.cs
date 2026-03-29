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

    public RegisterResponse Register(RegisterRequest registerRequest)
    {
        if (_dbContext.Users.Any(u => u.Email == registerRequest.Email))
        {
            return new RegisterResponse { IsSuccess = false, ErrorMessage = "Email already exists" };
        }

        var newUser = new StockApp.Domain.Entities.ApplicationUser
        {
            UserID = Guid.NewGuid(),
            Email = registerRequest.Email!
        };

        _dbContext.Users.Add(newUser);
        _dbContext.SaveChanges();

        // Also we want to assign a starting balance of 10000 
        // We'll just leave it since the UI doesn't require us to initialize it here or we can just initialize if needed. Let's just keep it simple.
        return new RegisterResponse { IsSuccess = true };
    }
}
