using System.ComponentModel.DataAnnotations;

namespace StockApp.Domain.Entities
{
    public class FinancialGoal
    {
        [Key]
        public Guid FinancialGoalID { get; set; }

        public Guid UserID { get; set; }
        public virtual ApplicationUser User { get; set; } = null!;

        [Required]
        public Guid GoalTypeID { get; set; }
        public virtual GoalType GoalType { get; set; } = null!;

        [Required]
        [MaxLength(100)]
        public string Title { get; set; } = string.Empty;

        [Range(0, double.MaxValue)]
        public double TargetAmount { get; set; }

        [Range(0, double.MaxValue)]
        public double InitialAmount { get; set; }

        [Range(0, double.MaxValue)]
        public double MonthlyContribution { get; set; }

        [Range(0, double.MaxValue)]
        public double CurrentAmount { get; set; }

        public DateTime CreatedDate { get; set; }
        public DateTime? TargetDate { get; set; }
        public bool IsCompleted { get; set; }
    }
}
