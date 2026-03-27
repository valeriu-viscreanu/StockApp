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
    }
}
