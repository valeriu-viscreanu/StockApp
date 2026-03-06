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
                TotalPortfolioValue = 0,
                DayChange = 0,
                DayChangePercentage = 0,
                TotalStocksHeld = 0
            };

            return View(viewModel);
        }
    }
}
