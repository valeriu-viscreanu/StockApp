using StockApp.Domain.Entities;
using StockApp.Domain.RepositoryContracts;
using System.Linq;

namespace StockApp.Infrastructure.Repositories
{
    public class InMemoryBuyOrderRepository : IBuyOrderRepository
    {
        private readonly List<BuyOrder> _orders = new();

        public void Add(BuyOrder order)
        {
            _orders.Add(order);
        }

        public void Update(BuyOrder order)
        {
            // In-memory: object reference is already updated in the list
        }

        public List<BuyOrder> GetAll()
        {
            return _orders.ToList();
        }

        public List<BuyOrder> GetByUserID(Guid userID)
        {
            return _orders.Where(o => o.UserID == userID).ToList();
        }
    }
}
