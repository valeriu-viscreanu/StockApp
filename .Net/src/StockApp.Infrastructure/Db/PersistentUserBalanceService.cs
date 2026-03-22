using StockApp.Application.ServiceContracts;

namespace StockApp.Infrastructure.Db;

public class PersistentUserBalanceService : IUserBalanceService
{
    private readonly ApplicationDbContext _dbContext;

    public PersistentUserBalanceService(ApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public double GetBalance(Guid userID)
    {
        var user = _dbContext.Users.FirstOrDefault(u => u.Id == userID);
        return (double)(user?.CashBalance ?? 1000m); // Default to 1000 for missing users to mirror in-memory behavior
    }

    public bool DeductBalance(Guid userID, double amount)
    {
        var user = _dbContext.Users.FirstOrDefault(u => u.Id == userID);
        if (user == null) return false;

        var decimalAmount = (decimal)amount;
        if (user.CashBalance < decimalAmount)
        {
            return false;
        }

        user.CashBalance -= decimalAmount;
        _dbContext.SaveChanges();
        return true;
    }

    public void AddBalance(Guid userID, double amount)
    {
        var user = _dbContext.Users.FirstOrDefault(u => u.Id == userID);
        if (user != null)
        {
            user.CashBalance += (decimal)amount;
            _dbContext.SaveChanges();
        }
    }
}
