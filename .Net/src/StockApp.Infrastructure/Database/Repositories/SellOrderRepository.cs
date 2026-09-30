using StockApp.Domain.Entities;
using StockApp.Domain.RepositoryContracts;
using Microsoft.EntityFrameworkCore;

namespace StockApp.Infrastructure.Database.Repositories;

public class SellOrderRepository : ISellOrderRepository
{
    private readonly ApplicationDbContext _dbContext;

    public SellOrderRepository(ApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task AddAsync(SellOrder order)
    {
        order.SellOrderID = Guid.NewGuid();
        _dbContext.SellOrders.Add(order);
        await _dbContext.SaveChangesAsync();
    }

    public async Task UpdateAsync(SellOrder order)
    {
        _dbContext.SellOrders.Update(order);
        await _dbContext.SaveChangesAsync();
    }

    public async Task<List<SellOrder>> GetAllAsync()
    {
        return await _dbContext.SellOrders.AsNoTracking().Include(o => o.OrderStatus).ToListAsync();
    }

    public async Task<List<SellOrder>> GetByUserIDAsync(Guid userID)
    {
        return await _dbContext.SellOrders
            .AsNoTracking()
            .Include(o => o.OrderStatus)
            .Where(so => so.UserID == userID)
            .ToListAsync();
    }
}
