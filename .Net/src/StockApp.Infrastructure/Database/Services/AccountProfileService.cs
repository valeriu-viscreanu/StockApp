using StockApp.Application.ServiceContracts;
using StockApp.Domain.Entities;
using StockApp.Infrastructure.Database;
using System;
using System.Linq;

namespace StockApp.Infrastructure.Database.Services
{
    public class AccountProfileService : IAccountProfileService
    {
        private readonly ApplicationDbContext _dbContext;

        public AccountProfileService(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public double GetBalance(Guid userID)
        {
            var account = _dbContext.Accounts.FirstOrDefault(a => a.UserID == userID);
            return account?.Balance ?? 0.0;
        }

        public bool DeductBalance(Guid userID, double amount)
        {
            var account = _dbContext.Accounts.FirstOrDefault(a => a.UserID == userID);
            if (account == null) return false;

            if (account.Balance < amount)
            {
                return false;
            }

            account.Balance -= amount;

            _dbContext.UserOperations.Add(new UserOperation
            {
                UserOperationID = Guid.NewGuid(),
                UserID = userID,
                OperationType = StockApp.Domain.Enums.OperationType.Withdrawal,
                TimeStamp = DateTime.UtcNow,
                Amount = amount,
                Description = $"Withdrawal of {amount:C} from account"
            });

            _dbContext.SaveChanges();

            return true;
        }

        public void AddBalance(Guid userID, double amount)
        {
            var account = _dbContext.Accounts.FirstOrDefault(a => a.UserID == userID);
            if (account == null)
            {
                // In a real app we might throw or create account
                return;
            }

            account.Balance += amount;

            _dbContext.UserOperations.Add(new UserOperation
            {
                UserOperationID = Guid.NewGuid(),
                UserID = userID,
                OperationType = StockApp.Domain.Enums.OperationType.Deposit,
                TimeStamp = DateTime.UtcNow,
                Amount = amount,
                Description = $"Deposit of {amount:C} to account"
            });

            _dbContext.SaveChanges();
        }

        public DateTime? GetDateOfBirth(Guid userID)
        {
            var account = _dbContext.Accounts.FirstOrDefault(a => a.UserID == userID);
            return account?.DateOfBirth;
        }
    }
}
