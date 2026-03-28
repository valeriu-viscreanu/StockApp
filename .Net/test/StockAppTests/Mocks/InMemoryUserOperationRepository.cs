using StockApp.Domain.Entities;
using StockApp.Domain.RepositoryContracts;

namespace StockAppTests.Mocks
{
    public class InMemoryUserOperationRepository : IUserOperationRepository
    {
        private readonly List<UserOperation> _userOperations = new();

        public void Add(UserOperation userOperation)
        {
            _userOperations.Add(userOperation);
        }

        public List<UserOperation> GetAll() => _userOperations;
    }
}
