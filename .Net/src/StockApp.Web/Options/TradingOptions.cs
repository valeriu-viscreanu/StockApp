namespace StockApp.Options
{
    public class TradingOptions
    {
        public string? DefaultStockSymbol { get; set; }
        public uint DefaultOrderQuantity { get; set; }

        /// <summary>
        /// Full bid-ask spread as a percentage of the mid price (0.20 means 0.20%).
        /// Half is applied either side of mid, so users buy at the ask and sell at
        /// the bid.
        /// </summary>
        public double SpreadPercentage { get; set; }
    }
}
