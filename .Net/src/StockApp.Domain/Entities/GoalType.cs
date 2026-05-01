using System.ComponentModel.DataAnnotations;

namespace StockApp.Domain.Entities
{
    public class GoalType
    {
        [Key]
        public Guid GoalTypeID { get; set; }

        [Required]
        [MaxLength(50)]
        public string Name { get; set; } = string.Empty;
    }
}
