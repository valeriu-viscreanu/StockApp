using StockApp.Domain.Entities;
using StockApp.Domain.RepositoryContracts;
using System.Linq;

namespace StockAppTests.Mocks
{
    public class InMemoryUserOperationRepository : IUserOperationRepository
    {
        private readonly List<UserOperation> _userOperations = new();

        public Task AddAsync(UserOperation userOperation)
        {
            _userOperations.Add(userOperation);
            return Task.CompletedTask;
        }

        public Task<IEnumerable<UserOperation>> GetByUserIdAsync(Guid userId)
        {
            return Task.FromResult(_userOperations.Where(uo => uo.UserID == userId));
        }

        public List<UserOperation> GetAll() => _userOperations;
    }
}
