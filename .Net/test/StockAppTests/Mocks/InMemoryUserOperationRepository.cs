using StockApp.Domain.Entities;
using StockApp.Domain.RepositoryContracts;
using System.Linq;

namespace StockAppTests.Mocks
{
    public class InMemoryUserOperationRepository : IUserOperationRepository
    {
        private readonly List<UserOperation> _userOperations = new();

        public void Add(UserOperation userOperation)
        {
            _userOperations.Add(userOperation);
        }

        public IEnumerable<UserOperation> GetByUserId(Guid userId)
        {
            return _userOperations.Where(uo => uo.UserID == userId);
        }

        public List<UserOperation> GetAll() => _userOperations;
    }
}
