using System.Collections.Concurrent;
using StockApp.Application.ServiceContracts;

namespace StockApp.Infrastructure.Services
{
    public class InMemoryUserBalanceService : IUserBalanceService
    {
        private static readonly ConcurrentDictionary<Guid, double> _balances = new();
        private const double InitialBalance = 1000.00;

        public double GetBalance(Guid userID)
        {
            return _balances.GetOrAdd(userID, InitialBalance);
        }

        public bool DeductBalance(Guid userID, double amount)
        {
            var currentBalance = GetBalance(userID);
            if (currentBalance < amount)
            {
                return false;
            }

            _balances[userID] = currentBalance - amount;
            return true;
        }

        public void AddBalance(Guid userID, double amount)
        {
            var currentBalance = GetBalance(userID);
            _balances[userID] = currentBalance + amount;
        }
    }
}
