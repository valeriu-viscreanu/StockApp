using System.ComponentModel.DataAnnotations;
using StockApp.Application.CustomValidators;

namespace StockApp.Application.DTO
{
    public class BuyOrderRequest
    {
        [Required(ErrorMessage = "Stock Symbol is required.")]
        public string StockSymbol { get; set; } = string.Empty;

        [Required(ErrorMessage = "Stock Name is required.")]
        public string StockName { get; set; } = string.Empty;

        [MinimumDateValidator("2000-01-01")]
        public DateTime DateAndTimeOfOrder { get; set; }

        [Range(1, 100000, ErrorMessage = "Quantity must be between 1 and 100000.")]
        public uint Quantity { get; set; }

        [Range(1, 10000, ErrorMessage = "Price must be between 1 and 10000.")]
        public double Price { get; set; }
    }
}
