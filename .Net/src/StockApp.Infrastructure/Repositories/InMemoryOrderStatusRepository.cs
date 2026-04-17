using StockApp.Domain.Entities;
using StockApp.Domain.RepositoryContracts;

namespace StockApp.Infrastructure.Repositories
{
    public class InMemoryOrderStatusRepository : IOrderStatusRepository
    {
        private readonly List<OrderStatus> _statuses = new()
        {
            new OrderStatus { OrderStatusID = Guid.Parse("20000000-0000-0000-0000-000000000001"), StatusName = "Pending" },
            new OrderStatus { OrderStatusID = Guid.Parse("20000000-0000-0000-0000-000000000002"), StatusName = "Authorized" },
            new OrderStatus { OrderStatusID = Guid.Parse("20000000-0000-0000-0000-000000000003"), StatusName = "Processed" },
            new OrderStatus { OrderStatusID = Guid.Parse("20000000-0000-0000-0000-000000000004"), StatusName = "Canceled" }
        };

        public Task<OrderStatus?> GetByName(string statusName)
        {
            return Task.FromResult(_statuses.FirstOrDefault(os => os.StatusName == statusName));
        }

        public Task<List<OrderStatus>> GetAll()
        {
            return Task.FromResult(_statuses.ToList());
        }
    }
}
