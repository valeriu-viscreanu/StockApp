using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using StockApp.Models;
using StockApp.Application.ServiceContracts;
using System.Security.Claims;

namespace StockApp.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        private readonly IAccountProfileService _accountProfileService;

        public HomeController(IAccountProfileService accountProfileService)
        {
            _accountProfileService = accountProfileService;
        }

        [Route("/Home")]
        [Route("/")]
        public IActionResult Index()
        {
            double cashBalance = 0;
            var userIdString = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (Guid.TryParse(userIdString, out Guid userId))
            {
                cashBalance = _accountProfileService.GetBalance(userId);
            }

            var viewModel = new PortfolioSummary
            {
                TotalPortfolioValue = 0,
                DayChange = 0,
                DayChangePercentage = 0,
                TotalStocksHeld = 0,
                CashBalance = cashBalance
            };

            return View(viewModel);
        }
    }
}
