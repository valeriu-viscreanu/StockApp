using StockApp.Domain.Entities;
using StockApp.Domain.RepositoryContracts;
using System.Linq;

namespace StockAppTests.Mocks
{
    public class InMemoryCashRepository : ICashRepository
    {
        private readonly List<Cash> _cashAllocations = new();

        public void Add(Cash cash)
        {
            _cashAllocations.Add(cash);
        }

        public void Update(Cash cash)
        {
            // Object reference is already updated
        }

        public void Delete(Guid cashID)
        {
            var item = _cashAllocations.FirstOrDefault(c => c.CashID == cashID);
            if (item != null) _cashAllocations.Remove(item);
        }

        public List<Cash> GetByAccountID(Guid accountID)
        {
            return _cashAllocations.Where(c => c.AccountID == accountID).ToList();
        }

        public Cash? GetBySymbol(Guid accountID, string symbol)
        {
            return _cashAllocations.FirstOrDefault(c => c.AccountID == accountID && c.StockSymbol == symbol);
        }
    }

    public class InMemoryAccountRepository : IAccountRepository
    {
        private readonly List<Account> _accounts = new();

        public void Add(Account account)
        {
            _accounts.Add(account);
        }

        public Account? GetByUserID(Guid userID)
        {
            return _accounts.FirstOrDefault(a => a.UserID == userID);
        }

        public void Update(Account account)
        {
            // Object reference is already updated
        }
    }
}
