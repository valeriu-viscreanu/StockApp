using StockApp.Application.ServiceContracts;
using StockApp.Domain.Entities;
using StockApp.Domain.RepositoryContracts;

namespace StockApp.Infrastructure.Database.Services;

public class UserBalanceService : IUserBalanceService
{
    private readonly ApplicationDbContext _dbContext;
    private readonly IUserOperationRepository _userOperationRepository;

    public UserBalanceService(ApplicationDbContext dbContext, IUserOperationRepository userOperationRepository)
    {
        _dbContext = dbContext;
        _userOperationRepository = userOperationRepository;
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

        _userOperationRepository.Add(new UserOperation
        {
            UserOperationID = Guid.NewGuid(),
            UserID = userID,
            OperationType = StockApp.Domain.Enums.OperationType.Withdrawal,
            TimeStamp = DateTime.UtcNow,
            Amount = amount,
            Description = $"Withdrawal of {amount:C}"
        });

        return true;
    }

    public void AddBalance(Guid userID, double amount)
    {
        var balance = _dbContext.UserBalances.FirstOrDefault(b => b.UserID == userID);
        if (balance == null)
        {
            balance = new UserBalance
            {
                UserID = userID,
                Balance = 0
            };
            _dbContext.UserBalances.Add(balance);
        }

        balance.Balance += amount;
        _dbContext.SaveChanges();

        _userOperationRepository.Add(new UserOperation
        {
            UserOperationID = Guid.NewGuid(),
            UserID = userID,
            OperationType = StockApp.Domain.Enums.OperationType.Deposit,
            TimeStamp = DateTime.UtcNow,
            Amount = amount,
            Description = $"Deposit of {amount:C}"
        });
    }
}
