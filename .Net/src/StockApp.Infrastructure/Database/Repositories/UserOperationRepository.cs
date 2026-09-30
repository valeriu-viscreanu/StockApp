using Microsoft.EntityFrameworkCore;
using StockApp.Domain.Entities;
using StockApp.Domain.RepositoryContracts;

namespace StockApp.Infrastructure.Database.Repositories
{
    public class UserOperationRepository : IUserOperationRepository
    {
        private readonly ApplicationDbContext _dbContext;

        public UserOperationRepository(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task AddAsync(UserOperation userOperation)
        {
            _dbContext.UserOperations.Add(userOperation);
            await _dbContext.SaveChangesAsync();
        }

        public async Task<IEnumerable<UserOperation>> GetByUserIdAsync(Guid userId)
        {
            return await _dbContext.UserOperations
                .AsNoTracking()
                .Where(uo => uo.UserID == userId)
                .OrderByDescending(uo => uo.TimeStamp)
                .ToListAsync();
        }
    }
}
