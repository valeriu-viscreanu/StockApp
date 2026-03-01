using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using StockApp.Models;

namespace StockApp.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        [Route("/Home")]
        [Route("/")]
        public IActionResult Index()
        {
            // Mock dummy data for the dashboard
            var viewModel = new PortfolioSummary
            {
                TotalPortfolioValue = 125430.85,
                DayChange = 2340.12,
                DayChangePercentage = 1.87,
                TotalStocksHeld = 12
            };

            return View(viewModel);
        }
    }
}
