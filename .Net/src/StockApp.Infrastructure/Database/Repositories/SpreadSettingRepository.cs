using Microsoft.EntityFrameworkCore;
using StockApp.Domain.Entities;
using StockApp.Domain.RepositoryContracts;

namespace StockApp.Infrastructure.Database.Repositories
{
    public class SpreadSettingRepository : ISpreadSettingRepository
    {
        private readonly ApplicationDbContext _dbContext;

        public SpreadSettingRepository(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<SpreadSetting?> GetCurrentAsync()
        {
            return await _dbContext
                .SpreadSettings
                .AsNoTracking()
                .OrderByDescending(s => s.UpdatedAt)
                .FirstOrDefaultAsync();
        }

        public async Task<SpreadSetting> UpdateSpreadPercentageAsync(double spreadPercentage)
        {
            // Own tracked query rather than GetCurrentAsync(): the fetched entity is
            // mutated in place below, so it must stay attached for SaveChangesAsync
            // to pick up the change.
            var setting = await _dbContext
                .SpreadSettings
                .OrderByDescending(s => s.UpdatedAt)
                .FirstOrDefaultAsync();

            if (setting == null)
            {
                setting = new SpreadSetting { SpreadSettingID = Guid.NewGuid() };
                _dbContext.SpreadSettings.Add(setting);
            }

            setting.SpreadPercentage = spreadPercentage;
            setting.UpdatedAt = DateTime.UtcNow;
            await _dbContext.SaveChangesAsync();

            return setting;
        }
    }
}
