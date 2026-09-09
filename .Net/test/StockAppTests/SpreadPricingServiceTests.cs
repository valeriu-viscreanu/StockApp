using StockApp.Application.ServiceContracts;
using StockApp.Application.Services;
using StockAppTests.Mocks;

namespace StockAppTests
{
    /// <summary>
    /// The spread is read from the database so it can be changed without a
    /// redeploy. It is a percentage of the mid price, split evenly either side:
    /// ask = mid * (1 + s/200), bid = mid * (1 - s/200), so (ask - bid) / mid == s%.
    /// </summary>
    public class SpreadPricingServiceTests
    {
        private static ISpreadPricingService PricingWithSpread(double spreadPercentage) =>
            new SpreadPricingService(new InMemorySpreadSettingRepository(spreadPercentage));

        // 1. Half the spread is added above the mid for buyers.
        [Fact]
        public void GetAskPrice_AddsHalfTheSpreadAboveMid()
        {
            Assert.Equal(100.10, PricingWithSpread(0.20).GetAskPrice(100.0), precision: 10);
        }

        // 2. Half the spread is taken off the mid for sellers.
        [Fact]
        public void GetBidPrice_SubtractsHalfTheSpreadBelowMid()
        {
            Assert.Equal(99.90, PricingWithSpread(0.20).GetBidPrice(100.0), precision: 10);
        }

        // 3. The gap between bid and ask is exactly the configured percentage of mid.
        [Theory]
        [InlineData(0.20, 100.0)]
        [InlineData(1.5, 250.0)]
        [InlineData(0.05, 499.70)]
        public void BidAskGap_IsTheConfiguredPercentageOfMid(double spreadPercentage, double mid)
        {
            ISpreadPricingService pricing = PricingWithSpread(spreadPercentage);

            double spreadAsFractionOfMid = (pricing.GetAskPrice(mid) - pricing.GetBidPrice(mid)) / mid;

            Assert.Equal(spreadPercentage / 100.0, spreadAsFractionOfMid, precision: 10);
        }

        // 4. A zero spread leaves the mid untouched on both sides.
        [Fact]
        public void ZeroSpread_LeavesMidUnchanged()
        {
            ISpreadPricingService pricing = PricingWithSpread(0);

            Assert.Equal(100.0, pricing.GetAskPrice(100.0), precision: 10);
            Assert.Equal(100.0, pricing.GetBidPrice(100.0), precision: 10);
        }

        // 5. Buying then immediately selling at an unchanged mid must lose the spread.
        [Fact]
        public void BuyThenSellAtSameMid_CostsTheSpread()
        {
            ISpreadPricingService pricing = PricingWithSpread(0.20);

            double roundTrip = pricing.GetBidPrice(100.0) - pricing.GetAskPrice(100.0);

            Assert.Equal(-0.20, roundTrip, precision: 10);
        }
        [Fact]
        public void NegativeSpreadInDatabase_IsRejected()
        {
            ISpreadPricingService pricing = PricingWithSpread(-0.1);

            Assert.Throws<InvalidOperationException>(() => pricing.GetAskPrice(100.0));
        }
    }
}
