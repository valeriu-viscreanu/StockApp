using StockApp.Domain.Entities;

namespace StockApp.Domain.RepositoryContracts
{
    public interface ISellOrderRepository
    {
        Task AddAsync(SellOrder order);
        Task UpdateAsync(SellOrder order);
        Task<List<SellOrder>> GetAllAsync();
        Task<List<SellOrder>> GetByUserIDAsync(Guid userID);
    }
}
