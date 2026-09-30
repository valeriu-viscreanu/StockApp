namespace StockApp.Application.ServiceContracts
{
    /// <summary>
    /// Derives the prices a user actually trades at from the mid-market price.
    /// The configured spread is split evenly either side of the mid, so a user
    /// buys at the ask (above mid) and sells at the bid (below mid).
    /// </summary>
    public interface ISpreadPricingService
    {
        /// <summary>Price a buyer pays: mid plus half the spread.</summary>
        Task<double> GetAskPriceAsync(double midPrice);

        /// <summary>Price a seller receives: mid minus half the spread.</summary>
        Task<double> GetBidPriceAsync(double midPrice);
    }
}
