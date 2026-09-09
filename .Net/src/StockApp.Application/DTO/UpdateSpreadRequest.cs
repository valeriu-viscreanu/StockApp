namespace StockApp.Application.DTO
{
    public class UpdateSpreadRequest
    {
        /// <summary>
        /// Full bid-ask spread as a percentage of the mid price (0.20 means 0.20%).
        /// </summary>
        public double SpreadPercentage { get; set; }
    }
}
