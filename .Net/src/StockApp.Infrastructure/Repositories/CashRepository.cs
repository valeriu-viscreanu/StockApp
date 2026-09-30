using StockApp.Domain.Entities;
using StockApp.Domain.RepositoryContracts;
using StockApp.Infrastructure.Database;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;

namespace StockApp.Infrastructure.Repositories
{
    public class CashRepository : ICashRepository
    {
        private readonly ApplicationDbContext _db;

        public CashRepository(ApplicationDbContext db)
        {
            _db = db;
        }

        public async Task AddAsync(Cash cash)
        {
            _db.CashAllocations.Add(cash);
            await _db.SaveChangesAsync();
        }

        public async Task DeleteAsync(Guid cashID)
        {
            var cash = await _db.CashAllocations.FindAsync(cashID);
            if (cash != null)
            {
                _db.CashAllocations.Remove(cash);
                await _db.SaveChangesAsync();
            }
        }

        public async Task<Cash?> GetBySymbolAsync(Guid accountID, string stockSymbol)
        {
            return await _db.CashAllocations
                .AsNoTracking()
                .FirstOrDefaultAsync(h => h.AccountID == accountID && h.StockSymbol == stockSymbol);
        }

        public async Task<List<Cash>> GetByAccountIDAsync(Guid accountID)
        {
            return await _db.CashAllocations.AsNoTracking().Where(h => h.AccountID == accountID).ToListAsync();
        }

        public async Task UpdateAsync(Cash cash)
        {
            _db.CashAllocations.Update(cash);
            await _db.SaveChangesAsync();
        }
    }
}
