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

        public void Add(UserOperation userOperation)
        {
            _dbContext.UserOperations.Add(userOperation);
            _dbContext.SaveChanges();
        }

        public IEnumerable<UserOperation> GetByUserId(Guid userId)
        {
            return _dbContext.UserOperations
                .Where(uo => uo.UserID == userId)
                .OrderByDescending(uo => uo.TimeStamp)
                .ToList();
        }
    }
}
