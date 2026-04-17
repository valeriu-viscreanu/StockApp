using Microsoft.EntityFrameworkCore;
using StockApp.Domain.Entities;
using StockApp.Domain.RepositoryContracts;

namespace StockApp.Infrastructure.Database.Repositories
{
    public class OrderStatusRepository : IOrderStatusRepository
    {
        private readonly ApplicationDbContext _dbContext;

        public OrderStatusRepository(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<OrderStatus?> GetByName(string statusName)
        {
            return await _dbContext.OrderStatuses
                .FirstOrDefaultAsync(os => os.StatusName == statusName);
        }

        public async Task<List<OrderStatus>> GetAll()
        {
            return await _dbContext.OrderStatuses.ToListAsync();
        }
    }
}
