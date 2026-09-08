using StockApp.Application.ServiceContracts;
using StockApp.Application.Services;

namespace StockAppTests
{
    /// <summary>
    /// The configured spread is a percentage of the mid price, split evenly
    /// either side of it: ask = mid * (1 + s/200), bid = mid * (1 - s/200),
    /// so that (ask - bid) / mid == s%.
    /// </summary>
    public class SpreadPricingServiceTests
    {
        // 1. Half the spread is added above the mid for buyers.
        [Fact]
        public void GetAskPrice_AddsHalfTheSpreadAboveMid()
        {
            ISpreadPricingService pricing = new SpreadPricingService(spreadPercentage: 0.20);

            Assert.Equal(100.10, pricing.GetAskPrice(100.0), precision: 10);
        }

        // 2. Half the spread is taken off the mid for sellers.
        [Fact]
        public void GetBidPrice_SubtractsHalfTheSpreadBelowMid()
        {
            ISpreadPricingService pricing = new SpreadPricingService(spreadPercentage: 0.20);

            Assert.Equal(99.90, pricing.GetBidPrice(100.0), precision: 10);
        }

        // 3. The gap between bid and ask is exactly the configured percentage of mid.
        [Theory]
        [InlineData(0.20, 100.0)]
        [InlineData(1.5, 250.0)]
        [InlineData(0.05, 499.70)]
        public void BidAskGap_IsTheConfiguredPercentageOfMid(double spreadPercentage, double mid)
        {
            ISpreadPricingService pricing = new SpreadPricingService(spreadPercentage);

            double spreadAsFractionOfMid = (pricing.GetAskPrice(mid) - pricing.GetBidPrice(mid)) / mid;

            Assert.Equal(spreadPercentage / 100.0, spreadAsFractionOfMid, precision: 10);
        }

        // 4. A zero spread leaves the mid untouched on both sides.
        [Fact]
        public void ZeroSpread_LeavesMidUnchanged()
        {
            ISpreadPricingService pricing = new SpreadPricingService(spreadPercentage: 0);

            Assert.Equal(100.0, pricing.GetAskPrice(100.0), precision: 10);
            Assert.Equal(100.0, pricing.GetBidPrice(100.0), precision: 10);
        }

        // 5. Buying then immediately selling at an unchanged mid must lose the spread,
        //    never gain - that round-trip cost is the whole point of the feature.
        [Fact]
        public void BuyThenSellAtSameMid_CostsTheSpread()
        {
            ISpreadPricingService pricing = new SpreadPricingService(spreadPercentage: 0.20);

            double roundTrip = pricing.GetBidPrice(100.0) - pricing.GetAskPrice(100.0);

            Assert.Equal(-0.20, roundTrip, precision: 10);
        }

        // 6. A negative spread is not a valid configuration.
        [Fact]
        public void NegativeSpread_IsRejected()
        {
            Assert.Throws<ArgumentOutOfRangeException>(() => new SpreadPricingService(spreadPercentage: -0.1));
        }
    }
}
