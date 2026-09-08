using StockApp.Domain.Entities;
using StockApp.Domain.RepositoryContracts;

namespace StockAppTests.Mocks
{
    public class InMemorySpreadSettingRepository : ISpreadSettingRepository
    {
        private SpreadSetting? _current;

        /// <summary>Number of reads, so tests can assert the value is not cached.</summary>
        public int ReadCount { get; private set; }

        public InMemorySpreadSettingRepository(double? spreadPercentage = null)
        {
            if (spreadPercentage.HasValue)
            {
                Set(spreadPercentage.Value);
            }
        }

        public void Set(double spreadPercentage)
        {
            _current = new SpreadSetting
            {
                SpreadSettingID = Guid.NewGuid(),
                SpreadPercentage = spreadPercentage,
                UpdatedAt = DateTime.UtcNow
            };
        }

        /// <summary>Leaves the table empty, as it would be before seeding.</summary>
        public void Clear() => _current = null;

        public SpreadSetting? GetCurrent()
        {
            ReadCount++;
            return _current;
        }
    }
}
