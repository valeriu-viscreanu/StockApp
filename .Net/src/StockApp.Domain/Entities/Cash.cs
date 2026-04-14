using System;
using System.ComponentModel.DataAnnotations;

namespace StockApp.Domain.Entities
{
    public class Cash
    {
        [Key]
        public Guid CashID { get; set; }

        public Guid AccountID { get; set; }
        public virtual Account Account { get; set; } = null!;

        [Required]
        public string StockSymbol { get; set; } = string.Empty;

        [Required]
        public string StockName { get; set; } = string.Empty;

        public uint Quantity { get; set; }
    }
}
