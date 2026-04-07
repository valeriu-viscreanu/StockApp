using StockApp.Domain.Entities;
using StockApp.Domain.RepositoryContracts;
using StockApp.Infrastructure.Database;
using Microsoft.EntityFrameworkCore;

namespace StockApp.Infrastructure.Repositories
{
    public class UserHoldingRepository : IUserHoldingRepository
    {
        private readonly ApplicationDbContext _db;

        public UserHoldingRepository(ApplicationDbContext db)
        {
            _db = db;
        }

        public void Add(UserHolding holding)
        {
            _db.UserHoldings.Add(holding);
            _db.SaveChanges();
        }

        public void Delete(Guid holdingID)
        {
            var holding = _db.UserHoldings.Find(holdingID);
            if (holding != null)
            {
                _db.UserHoldings.Remove(holding);
                _db.SaveChanges();
            }
        }

        public UserHolding? GetBySymbol(Guid userID, string stockSymbol)
        {
            return _db.UserHoldings.FirstOrDefault(h => h.UserID == userID && h.StockSymbol == stockSymbol);
        }

        public List<UserHolding> GetByUserID(Guid userID)
        {
            return _db.UserHoldings.Where(h => h.UserID == userID).ToList();
        }

        public void Update(UserHolding holding)
        {
            _db.UserHoldings.Update(holding);
            _db.SaveChanges();
        }
    }
}
