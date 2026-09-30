using StockApp.Application.ServiceContracts;
using StockApp.Domain.RepositoryContracts;

namespace StockApp.Application.Services
{
    public class SpreadPricingService : ISpreadPricingService
    {
        private readonly ISpreadSettingRepository _spreadSettingRepository;

        public SpreadPricingService(ISpreadSettingRepository spreadSettingRepository)
        {
            _spreadSettingRepository = spreadSettingRepository;
        }

        public async Task<double> GetAskPriceAsync(double midPrice) => midPrice * (1 + await HalfSpreadFraction());

        public async Task<double> GetBidPriceAsync(double midPrice) => midPrice * (1 - await HalfSpreadFraction());

        /// <summary>
        /// Half the configured spread, as a fraction of the mid price. Read on every
        /// call rather than cached, so editing the row takes effect immediately.
        /// </summary>
        private async Task<double> HalfSpreadFraction()
        {
            // No row configured yet: trade at mid rather than invent a charge.
            var current = await _spreadSettingRepository.GetCurrentAsync();
            double spreadPercentage = current?.SpreadPercentage ?? 0;

            if (spreadPercentage < 0)
            {
                throw new InvalidOperationException(
                    $"Configured spread percentage ({spreadPercentage}) cannot be negative.");
            }

            return spreadPercentage / 200.0;
        }
    }
}
