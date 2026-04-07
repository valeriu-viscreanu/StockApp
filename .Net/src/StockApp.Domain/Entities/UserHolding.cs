using System.ComponentModel.DataAnnotations;

namespace StockApp.Domain.Entities
{
    public class UserHolding
    {
        [Key]
        public Guid HoldingID { get; set; }

        public Guid UserID { get; set; }

        [Required]
        public string StockSymbol { get; set; } = string.Empty;

        [Required]
        public string StockName { get; set; } = string.Empty;

        public uint Quantity { get; set; }
    }
}
