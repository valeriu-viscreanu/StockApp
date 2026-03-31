using StockApp.Domain.Entities;

namespace StockApp.Domain.RepositoryContracts
{
    public interface IUserOperationRepository
    {
        void Add(UserOperation userOperation);
        IEnumerable<UserOperation> GetByUserId(Guid userId);
    }
}
