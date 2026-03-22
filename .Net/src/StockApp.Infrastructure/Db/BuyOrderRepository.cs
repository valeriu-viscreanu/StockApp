using Microsoft.EntityFrameworkCore;
using StockApp.Domain.Entities;
using StockApp.Domain.RepositoryContracts;

namespace StockApp.Infrastructure.Db;

public class BuyOrderRepository : IBuyOrderRepository
{
    private readonly ApplicationDbContext _dbContext;

    public BuyOrderRepository(ApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<BuyOrder> CreateBuyOrder(BuyOrder buyOrder)
    {
        buyOrder.BuyOrderID = Guid.NewGuid();
        _dbContext.BuyOrders.Add(buyOrder);
        await _dbContext.SaveChangesAsync();
        return buyOrder;
    }

    public async Task<List<BuyOrder>> GetBuyOrders()
    {
        return await _dbContext.BuyOrders.ToListAsync();
    }
}
