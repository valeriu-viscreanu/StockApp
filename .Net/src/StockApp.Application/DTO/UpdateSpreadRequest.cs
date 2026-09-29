using System.ComponentModel.DataAnnotations;

namespace StockApp.Application.DTO
{
    public class UpdateSpreadRequest
    {
        /// <summary>
        /// Full bid-ask spread as a percentage of the mid price (0.20 means 0.20%).
        /// </summary>
        // Double literals matter here: Range(int, int) rounds the value to an int
        // before comparing, so -0.1 and 100.1 would both pass as 0 and 100.
        [Range(0.0, 100.0, ErrorMessage = "Spread percentage must be between 0 and 100.")]
        public double SpreadPercentage { get; set; }
    }
}
