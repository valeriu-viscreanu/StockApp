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
        public async Task GetAskPrice_AddsHalfTheSpreadAboveMid()
        {
            Assert.Equal(100.10, await PricingWithSpread(0.20).GetAskPriceAsync(100.0), precision: 10);
        }

        // 2. Half the spread is taken off the mid for sellers.
        [Fact]
        public async Task GetBidPrice_SubtractsHalfTheSpreadBelowMid()
        {
            Assert.Equal(99.90, await PricingWithSpread(0.20).GetBidPriceAsync(100.0), precision: 10);
        }

        // 3. The gap between bid and ask is exactly the configured percentage of mid.
        [Theory]
        [InlineData(0.20, 100.0)]
        [InlineData(1.5, 250.0)]
        [InlineData(0.05, 499.70)]
        public async Task BidAskGap_IsTheConfiguredPercentageOfMid(double spreadPercentage, double mid)
        {
            ISpreadPricingService pricing = PricingWithSpread(spreadPercentage);

            double spreadAsFractionOfMid = (await pricing.GetAskPriceAsync(mid) - await pricing.GetBidPriceAsync(mid)) / mid;

            Assert.Equal(spreadPercentage / 100.0, spreadAsFractionOfMid, precision: 10);
        }

        // 4. A zero spread leaves the mid untouched on both sides.
        [Fact]
        public async Task ZeroSpread_LeavesMidUnchanged()
        {
            ISpreadPricingService pricing = PricingWithSpread(0);

            Assert.Equal(100.0, await pricing.GetAskPriceAsync(100.0), precision: 10);
            Assert.Equal(100.0, await pricing.GetBidPriceAsync(100.0), precision: 10);
        }

        // 5. Buying then immediately selling at an unchanged mid must lose the spread.
        [Fact]
        public async Task BuyThenSellAtSameMid_CostsTheSpread()
        {
            ISpreadPricingService pricing = PricingWithSpread(0.20);

            double roundTrip = await pricing.GetBidPriceAsync(100.0) - await pricing.GetAskPriceAsync(100.0);

            Assert.Equal(-0.20, roundTrip, precision: 10);
        }

        [Fact]
        public async Task NegativeSpreadInDatabase_IsRejected()
        {
            ISpreadPricingService pricing = PricingWithSpread(-0.1);

            await Assert.ThrowsAsync<InvalidOperationException>(() => pricing.GetAskPriceAsync(100.0));
        }
    }
}
