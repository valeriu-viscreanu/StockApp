using System.ComponentModel.DataAnnotations;

namespace StockApp.Domain.Entities
{
    public class OrderStatus
    {
        [Key]
        public Guid OrderStatusID { get; set; }

        [Required]
        [StringLength(50)]
        public string StatusName { get; set; } = string.Empty;
    }
}
