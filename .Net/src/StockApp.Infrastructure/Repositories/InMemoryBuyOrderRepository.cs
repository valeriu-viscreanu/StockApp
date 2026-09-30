using StockApp.Domain.Entities;
using StockApp.Domain.RepositoryContracts;
using System.Linq;

namespace StockApp.Infrastructure.Repositories
{
    public class InMemoryBuyOrderRepository : IBuyOrderRepository
    {
        private readonly List<BuyOrder> _orders = new();

        public Task AddAsync(BuyOrder order)
        {
            _orders.Add(order);
            return Task.CompletedTask;
        }

        public Task UpdateAsync(BuyOrder order)
        {
            // In-memory: object reference is already updated in the list
            return Task.CompletedTask;
        }

        public Task<List<BuyOrder>> GetAllAsync()
        {
            return Task.FromResult(_orders.ToList());
        }

        public Task<List<BuyOrder>> GetByUserIDAsync(Guid userID)
        {
            return Task.FromResult(_orders.Where(o => o.UserID == userID).ToList());
        }
    }
}
