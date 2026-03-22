using Microsoft.EntityFrameworkCore;
using StockApp.Domain.Entities;
using StockApp.Domain.RepositoryContracts;

namespace StockApp.Infrastructure.Db;

public class SellOrderRepository : ISellOrderRepository
{
    private readonly ApplicationDbContext _dbContext;

    public SellOrderRepository(ApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<SellOrder> CreateSellOrder(SellOrder sellOrder)
    {
        sellOrder.SellOrderID = Guid.NewGuid();
        _dbContext.SellOrders.Add(sellOrder);
        await _dbContext.SaveChangesAsync();
        return sellOrder;
    }

    public async Task<List<SellOrder>> GetSellOrders()
    {
        return await _dbContext.SellOrders.ToListAsync();
    }
}
