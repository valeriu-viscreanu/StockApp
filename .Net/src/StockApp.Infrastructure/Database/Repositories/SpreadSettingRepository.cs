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

        public SpreadSetting UpdateSpreadPercentage(double spreadPercentage)
        {
            var setting = GetCurrent();

            if (setting == null)
            {
                setting = new SpreadSetting { SpreadSettingID = Guid.NewGuid() };
                _dbContext.SpreadSettings.Add(setting);
            }

            setting.SpreadPercentage = spreadPercentage;
            setting.UpdatedAt = DateTime.UtcNow;
            _dbContext.SaveChanges();

            return setting;
        }
    }
}
