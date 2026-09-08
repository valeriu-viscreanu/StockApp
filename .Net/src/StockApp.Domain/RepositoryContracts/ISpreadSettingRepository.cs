using StockApp.Domain.Entities;

namespace StockApp.Domain.RepositoryContracts
{
    public interface ISpreadSettingRepository
    {
        /// <summary>
        /// The spread setting currently in force, or null if none is configured.
        /// </summary>
        SpreadSetting? GetCurrent();
    }
}
