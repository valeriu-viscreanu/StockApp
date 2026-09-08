using StockApp.Application.ServiceContracts;

namespace StockApp.Application.Services
{
    public class SpreadPricingService : ISpreadPricingService
    {
        private readonly double _spreadPercentage;

        /// <param name="spreadPercentage">
        /// The full bid-ask spread as a percentage of the mid price
        /// (0.20 means 0.20%). Half of it is applied either side of the mid.
        /// </param>
        public SpreadPricingService(double spreadPercentage)
        {
            if (spreadPercentage < 0)
            {
                throw new ArgumentOutOfRangeException(
                    nameof(spreadPercentage), "Spread percentage cannot be negative.");
            }

            _spreadPercentage = spreadPercentage;
        }

        // Half the spread, expressed as a fraction of the mid price.
        private double HalfSpreadFraction => _spreadPercentage / 200.0;

        public double GetAskPrice(double midPrice) => midPrice * (1 + HalfSpreadFraction);

        public double GetBidPrice(double midPrice) => midPrice * (1 - HalfSpreadFraction);
    }
}
