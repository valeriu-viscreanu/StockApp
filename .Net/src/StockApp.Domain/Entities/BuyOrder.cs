using System.ComponentModel.DataAnnotations;

namespace StockApp.Domain.Entities
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

        public Guid UserID { get; set; }

        public Guid OrderStatusID { get; set; }
        public virtual OrderStatus OrderStatus { get; set; } = null!;
    }
}
