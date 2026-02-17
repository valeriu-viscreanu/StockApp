using System.ComponentModel.DataAnnotations;

namespace StockApp.Entities
{
    public class BuyOrder
    {
        public Guid BuyOrderID { get; set; }

        [Required]
        public string StockSymbol { get; set; } = string.Empty;

        [Required]
        public string StockName { get; set; } = string.Empty;

        public DateTime DateAndTimeOfOrder { get; set; }

        [Range(1, 100000)]
        public uint Quantity { get; set; }

        [Range(1, 10000)]
        public double Price { get; set; }
    }
}
