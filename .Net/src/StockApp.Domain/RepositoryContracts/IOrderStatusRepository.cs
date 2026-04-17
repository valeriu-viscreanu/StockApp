using StockApp.Domain.Entities;

namespace StockApp.Domain.RepositoryContracts
{
    public interface IOrderStatusRepository
    {
        Task<OrderStatus?> GetByName(string statusName);
        Task<List<OrderStatus>> GetAll();
    }
}
