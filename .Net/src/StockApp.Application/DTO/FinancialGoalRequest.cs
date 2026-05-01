using System.ComponentModel.DataAnnotations;

namespace StockApp.Application.DTO
{
    public class FinancialGoalRequest
    {
        [Required(ErrorMessage = "Title is required")]
        [MaxLength(100, ErrorMessage = "Title cannot exceed 100 characters")]
        public string Title { get; set; } = string.Empty;

        [Required(ErrorMessage = "Goal Type is required")]
        public Guid GoalTypeID { get; set; }

        [Range(0, double.MaxValue, ErrorMessage = "Target amount must be non-negative")]
        public double TargetAmount { get; set; }

        [Range(0, double.MaxValue, ErrorMessage = "Initial amount must be non-negative")]
        public double InitialAmount { get; set; }

        [Range(0, double.MaxValue, ErrorMessage = "Monthly contribution must be non-negative")]
        public double MonthlyContribution { get; set; }

        public DateTime? TargetDate { get; set; }
    }
}
