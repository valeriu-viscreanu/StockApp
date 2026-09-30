using Microsoft.EntityFrameworkCore;
using StockApp.Domain.Entities;
using StockApp.Domain.RepositoryContracts;
using StockApp.Infrastructure.Database;

namespace StockApp.Infrastructure.Repositories
{
    public class UserDetailsRepository : IUserDetailsRepository
    {
        private readonly ApplicationDbContext _db;

        public UserDetailsRepository(ApplicationDbContext db)
        {
            _db = db;
        }

        public async Task AddAsync(UserDetails userDetails)
        {
            _db.UserDetails.Add(userDetails);
            await _db.SaveChangesAsync();
        }

        public async Task<UserDetails?> GetByUserIDAsync(Guid userID)
        {
            return await _db.UserDetails.AsNoTracking().FirstOrDefaultAsync(ud => ud.UserID == userID);
        }

        public async Task UpdateAsync(UserDetails userDetails)
        {
            _db.UserDetails.Update(userDetails);
            await _db.SaveChangesAsync();
        }
    }
}
