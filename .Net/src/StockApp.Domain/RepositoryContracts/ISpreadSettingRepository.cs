using StockApp.Domain.Entities;

namespace StockApp.Domain.RepositoryContracts
{
    public interface ISpreadSettingRepository
    {
        /// <summary>
        /// The spread setting currently in force, or null if none is configured.
        /// </summary>
        Task<SpreadSetting?> GetCurrentAsync();

        /// <summary>
        /// Stores the spread in force, updating the existing row if there is one
        /// and creating it otherwise. Returns the saved setting.
        /// </summary>
        Task<SpreadSetting> UpdateSpreadPercentageAsync(double spreadPercentage);
    }
}
