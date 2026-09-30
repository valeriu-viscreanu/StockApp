using StockApp.Domain.Entities;
using StockApp.Domain.RepositoryContracts;
using System.Linq;

namespace StockApp.Infrastructure.Repositories
{
    public class InMemorySellOrderRepository : ISellOrderRepository
    {
        private readonly List<SellOrder> _orders = new();

        public Task AddAsync(SellOrder order)
        {
            _orders.Add(order);
            return Task.CompletedTask;
        }

        public Task UpdateAsync(SellOrder order)
        {
            // In-memory: object reference is already updated in the list
            return Task.CompletedTask;
        }

        public Task<List<SellOrder>> GetAllAsync()
        {
            return Task.FromResult(_orders.ToList());
        }

        public Task<List<SellOrder>> GetByUserIDAsync(Guid userID)
        {
            return Task.FromResult(_orders.Where(o => o.UserID == userID).ToList());
        }
    }
}
