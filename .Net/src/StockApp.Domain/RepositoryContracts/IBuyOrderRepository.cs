using StockApp.Domain.Entities;

namespace StockApp.Domain.RepositoryContracts
{
    public interface IBuyOrderRepository
    {
        Task AddAsync(BuyOrder order);
        Task UpdateAsync(BuyOrder order);
        Task<List<BuyOrder>> GetAllAsync();
        Task<List<BuyOrder>> GetByUserIDAsync(Guid userID);
    }
}
