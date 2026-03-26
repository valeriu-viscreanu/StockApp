using StockApp.Application.ServiceContracts;
using StockApp.Domain.Entities;

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
        var balance = _dbContext.UserBalances.FirstOrDefault(b => b.UserID == userID);
        return balance?.Balance ?? 1000.00;
    }

    public bool DeductBalance(Guid userID, double amount)
    {
        var balance = _dbContext.UserBalances.FirstOrDefault(b => b.UserID == userID);
        if (balance == null) return false;

        if (balance.Balance < amount)
        {
            return false;
        }

        balance.Balance -= amount;
        _dbContext.SaveChanges();
        return true;
    }

    public void AddBalance(Guid userID, double amount)
    {
        var balance = _dbContext.UserBalances.FirstOrDefault(b => b.UserID == userID);
        if (balance != null)
        {
            balance.Balance += amount;
            _dbContext.SaveChanges();
        }
    }
}
