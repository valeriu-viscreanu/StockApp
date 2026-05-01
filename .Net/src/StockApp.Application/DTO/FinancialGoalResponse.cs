namespace StockApp.Application.DTO
{
    public class FinancialGoalResponse
    {
        public Guid FinancialGoalID { get; set; }
        public Guid UserID { get; set; }
        public Guid GoalTypeID { get; set; }
        public string GoalTypeName { get; set; } = string.Empty;
        public string Title { get; set; } = string.Empty;
        public double TargetAmount { get; set; }
        public double InitialAmount { get; set; }
        public double MonthlyContribution { get; set; }
        public double CurrentAmount { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime? TargetDate { get; set; }
        public bool IsCompleted { get; set; }
        public double ProgressPercentage => TargetAmount > 0 ? (CurrentAmount / TargetAmount) * 100 : 0;
    }
}
