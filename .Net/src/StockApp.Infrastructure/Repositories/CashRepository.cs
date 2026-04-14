using StockApp.Domain.Entities;
using StockApp.Domain.RepositoryContracts;
using StockApp.Infrastructure.Database;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;

namespace StockApp.Infrastructure.Repositories
{
    public class CashRepository : ICashRepository
    {
        private readonly ApplicationDbContext _db;

        public CashRepository(ApplicationDbContext db)
        {
            _db = db;
        }

        public void Add(Cash cash)
        {
            _db.CashAllocations.Add(cash);
            _db.SaveChanges();
        }

        public void Delete(Guid cashID)
        {
            var cash = _db.CashAllocations.Find(cashID);
            if (cash != null)
            {
                _db.CashAllocations.Remove(cash);
                _db.SaveChanges();
            }
        }

        public Cash? GetBySymbol(Guid accountID, string stockSymbol)
        {
            return _db.CashAllocations.FirstOrDefault(h => h.AccountID == accountID && h.StockSymbol == stockSymbol);
        }

        public List<Cash> GetByAccountID(Guid accountID)
        {
            return _db.CashAllocations.Where(h => h.AccountID == accountID).ToList();
        }

        public void Update(Cash cash)
        {
            _db.CashAllocations.Update(cash);
            _db.SaveChanges();
        }
    }
}
