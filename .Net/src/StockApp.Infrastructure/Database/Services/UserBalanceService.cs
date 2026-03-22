using StockApp.Application.ServiceContracts;

namespace StockApp.Infrastructure.Database.Services;

public class UserBalanceService : IUserBalanceService
{
    private readonly ApplicationDbContext _dbContext;

    public UserBalanceService(ApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public double GetBalance(Guid userID)
    {
        var user = _dbContext.Users.FirstOrDefault(u => u.UserID == userID);
        return user?.CashBalance ?? 1000.00;
    }

    public bool DeductBalance(Guid userID, double amount)
    {
        var user = _dbContext.Users.FirstOrDefault(u => u.UserID == userID);
        if (user == null) return false;

        if (user.CashBalance < amount)
        {
            return false;
        }

        user.CashBalance -= amount;
        _dbContext.SaveChanges();
        return true;
    }

    public void AddBalance(Guid userID, double amount)
    {
        var user = _dbContext.Users.FirstOrDefault(u => u.UserID == userID);
        if (user != null)
        {
            user.CashBalance += amount;
            _dbContext.SaveChanges();
        }
    }
}
