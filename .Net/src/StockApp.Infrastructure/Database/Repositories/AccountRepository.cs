using StockApp.Domain.Entities;
using StockApp.Domain.RepositoryContracts;
using StockApp.Infrastructure.Database;
using Microsoft.EntityFrameworkCore;
using System;

namespace StockApp.Infrastructure.Database.Repositories
{
    public class AccountRepository : IAccountRepository
    {
        private readonly ApplicationDbContext _db;

        public AccountRepository(ApplicationDbContext db)
        {
            _db = db;
        }

        public async Task<Account?> GetByUserIDAsync(Guid userID)
        {
            return await _db.Accounts.AsNoTracking().FirstOrDefaultAsync(a => a.UserID == userID);
        }

        public async Task UpdateAsync(Account account)
        {
            _db.Accounts.Update(account);
            await _db.SaveChangesAsync();
        }
    }
}
