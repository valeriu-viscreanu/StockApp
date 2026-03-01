using Microsoft.AspNetCore.Mvc;

namespace StockApp.Models
{
    public class PortfolioSummary
    {
        public double TotalPortfolioValue { get; set; }
        public double DayChange { get; set; }
        public double DayChangePercentage { get; set; }
        public int TotalStocksHeld { get; set; }
    }
}
