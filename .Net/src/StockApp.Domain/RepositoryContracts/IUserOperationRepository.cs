using StockApp.Domain.Entities;

namespace StockApp.Domain.RepositoryContracts
{
    public interface IUserOperationRepository
    {
        Task AddAsync(UserOperation userOperation);
        Task<IEnumerable<UserOperation>> GetByUserIdAsync(Guid userId);
    }
}
