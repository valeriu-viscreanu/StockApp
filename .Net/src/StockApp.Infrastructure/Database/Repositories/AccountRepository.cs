using StockApp.Domain.Entities;
using StockApp.Domain.RepositoryContracts;
using StockApp.Infrastructure.Database;
using System;
using System.Linq;

namespace StockApp.Infrastructure.Database.Repositories
{
    public class AccountRepository : IAccountRepository
    {
        private readonly ApplicationDbContext _db;

        public AccountRepository(ApplicationDbContext db)
        {
            _db = db;
        }

        public Account? GetByUserID(Guid userID)
        {
            return _db.Accounts.FirstOrDefault(a => a.UserID == userID);
        }

        public void Update(Account account)
        {
            _db.Accounts.Update(account);
            _db.SaveChanges();
        }
    }
}
