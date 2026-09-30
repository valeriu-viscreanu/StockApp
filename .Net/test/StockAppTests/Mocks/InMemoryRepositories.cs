using StockApp.Domain.Entities;
using StockApp.Domain.RepositoryContracts;
using System.Linq;

namespace StockAppTests.Mocks
{
    public class InMemoryCashRepository : ICashRepository
    {
        private readonly List<Cash> _cashAllocations = new();

        public Task AddAsync(Cash cash)
        {
            _cashAllocations.Add(cash);
            return Task.CompletedTask;
        }

        public Task UpdateAsync(Cash cash)
        {
            // Object reference is already updated
            return Task.CompletedTask;
        }

        public Task DeleteAsync(Guid cashID)
        {
            var item = _cashAllocations.FirstOrDefault(c => c.CashID == cashID);
            if (item != null) _cashAllocations.Remove(item);
            return Task.CompletedTask;
        }

        public Task<List<Cash>> GetByAccountIDAsync(Guid accountID)
        {
            return Task.FromResult(_cashAllocations.Where(c => c.AccountID == accountID).ToList());
        }

        public Task<Cash?> GetBySymbolAsync(Guid accountID, string symbol)
        {
            return Task.FromResult(_cashAllocations.FirstOrDefault(c => c.AccountID == accountID && c.StockSymbol == symbol));
        }
    }

    public class InMemoryAccountRepository : IAccountRepository
    {
        private readonly List<Account> _accounts = new();

        public void Add(Account account)
        {
            _accounts.Add(account);
        }

        public Task<Account?> GetByUserIDAsync(Guid userID)
        {
            return Task.FromResult(_accounts.FirstOrDefault(a => a.UserID == userID));
        }

        public Task UpdateAsync(Account account)
        {
            // Object reference is already updated
            return Task.CompletedTask;
        }
    }
}
