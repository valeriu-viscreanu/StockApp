using System.ComponentModel.DataAnnotations;
using StockApp.Domain.Enums;

namespace StockApp.Domain.Entities
{
    public class UserOperation
    {
        public Guid UserOperationID { get; set; }

        public Guid UserID { get; set; }

        [Required]
        public OperationType OperationType { get; set; }

        public DateTime TimeStamp { get; set; }

        public double Amount { get; set; }

        public string? StockSymbol { get; set; }

        public string? Description { get; set; }
    }
}
