using StockApp.Domain.Entities;
using StockApp.Domain.RepositoryContracts;
using StockApp.Infrastructure.Database;
using System.Linq;

namespace StockApp.Infrastructure.Repositories
{
    public class UserDetailsRepository : IUserDetailsRepository
    {
        private readonly ApplicationDbContext _db;

        public UserDetailsRepository(ApplicationDbContext db)
        {
            _db = db;
        }

        public void Add(UserDetails userDetails)
        {
            _db.UserDetails.Add(userDetails);
            _db.SaveChanges();
        }

        public UserDetails? GetByUserID(Guid userID)
        {
            return _db.UserDetails.FirstOrDefault(ud => ud.UserID == userID);
        }

        public void Update(UserDetails userDetails)
        {
            _db.UserDetails.Update(userDetails);
            _db.SaveChanges();
        }
    }
}
