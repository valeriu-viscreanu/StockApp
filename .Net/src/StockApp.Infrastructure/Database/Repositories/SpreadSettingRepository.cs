using StockApp.Domain.Entities;
using StockApp.Domain.RepositoryContracts;
using System.Linq;

namespace StockApp.Infrastructure.Database.Repositories
{
    public class SpreadSettingRepository : ISpreadSettingRepository
    {
        private readonly ApplicationDbContext _dbContext;

        public SpreadSettingRepository(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public SpreadSetting? GetCurrent()
        {
            return _dbContext.SpreadSettings
                .OrderByDescending(s => s.UpdatedAt)
                .FirstOrDefault();
        }
    }
}
