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

        public double GetAskPrice(double midPrice) => midPrice * (1 + HalfSpreadFraction());

        public double GetBidPrice(double midPrice) => midPrice * (1 - HalfSpreadFraction());

        /// <summary>
        /// Half the configured spread, as a fraction of the mid price. Read on every
        /// call rather than cached, so editing the row takes effect immediately.
        /// </summary>
        private double HalfSpreadFraction()
        {
            // No row configured yet: trade at mid rather than invent a charge.
            double spreadPercentage = _spreadSettingRepository.GetCurrent()?.SpreadPercentage ?? 0;

            if (spreadPercentage < 0)
            {
                throw new InvalidOperationException(
                    $"Configured spread percentage ({spreadPercentage}) cannot be negative.");
            }

            return spreadPercentage / 200.0;
        }
    }
}
