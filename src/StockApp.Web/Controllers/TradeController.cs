using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using StockApp.Application.DTO;
using StockApp.Filters;
using StockApp.Models;
using StockApp.Options;
using StockApp.Application.ServiceContracts;

namespace StockApp.Controllers
{
    [Route("[controller]")]
    [Authorize]
    public class TradeController : Controller
    {
        private readonly IStockProfileService _stockProfileService;
        private readonly IStockQuoteService _stockQuoteService;
        private readonly IBuyOrdersService _buyOrdersService;
        private readonly ISellOrdersService _sellOrdersService;
        private readonly IUserBalanceService _userBalanceService;
        private readonly TradingOptions _tradingOptions;
        private readonly IConfiguration _configuration;

        public TradeController(
            IStockProfileService stockProfileService,
            IStockQuoteService stockQuoteService,
            IBuyOrdersService buyOrdersService,
            ISellOrdersService sellOrdersService,
            IUserBalanceService userBalanceService,
            IOptions<TradingOptions> tradingOptions,
            IConfiguration configuration)
        {
            _stockProfileService = stockProfileService;
            _stockQuoteService = stockQuoteService;
            _buyOrdersService = buyOrdersService;
            _sellOrdersService = sellOrdersService;
            _userBalanceService = userBalanceService;
            _tradingOptions = tradingOptions.Value;
            _configuration = configuration;
        }

        [Route("[action]/{stock?}")]
        [HttpGet]
        public async Task<IActionResult> Index(string? stock)
        {
            string stockSymbol = !string.IsNullOrWhiteSpace(stock)
                ? stock
                : (!string.IsNullOrWhiteSpace(_tradingOptions.DefaultStockSymbol) ? _tradingOptions.DefaultStockSymbol : "MSFT");

            // Attempt to get quote. If price is 0, it might be a name search
            FinnhubStockQuoteResponse? stockPriceQuote = await _stockQuoteService.GetStockPriceQuote(stockSymbol);
            
            if (stockPriceQuote == null || stockPriceQuote.CurrentPrice == 0)
            {
                // Try searching for the text to see if it's a company name
                var searchResults = await _stockProfileService.SearchStocks(stockSymbol);
                if (searchResults?.Result != null && searchResults.Result.Count > 0)
                {
                    // Take the first matching symbol (best guess)
                    stockSymbol = searchResults.Result[0].Symbol!;
                    stockPriceQuote = await _stockQuoteService.GetStockPriceQuote(stockSymbol);
                }
            }

            FinnhubCompanyProfileResponse? companyProfile = await _stockProfileService.GetCompanyProfile(stockSymbol!);

            StockTrade stockTrade = new StockTrade
            {
                StockSymbol = stockSymbol,
                Quantity = _tradingOptions.DefaultOrderQuantity
            };

            if (companyProfile != null)
            {
                stockTrade.StockName = companyProfile.Name;
            }

            if (stockPriceQuote != null)
            {
                stockTrade.Price = stockPriceQuote.CurrentPrice ?? 0;
            }

            ViewBag.FinnhubToken = _configuration["FinnhubToken"];

            var userIdString = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
            if (Guid.TryParse(userIdString, out Guid userId))
            {
                stockTrade.CashBalance = _userBalanceService.GetBalance(userId);
            }

            return View(stockTrade);
        }

        [Route("[action]")]
        [HttpPost]
        [TypeFilter(typeof(CreateOrderActionFilter))]
        public async Task<IActionResult> BuyOrder(BuyOrderRequest buyOrderRequest)
        {
            var userIdString = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
            if (Guid.TryParse(userIdString, out Guid userId))
            {
                buyOrderRequest.UserID = userId;
                double totalCost = buyOrderRequest.Price * buyOrderRequest.Quantity;
                if (!_userBalanceService.DeductBalance(userId, totalCost))
                {
                    TempData["Error"] = "Insufficient funds for this purchase.";
                    return RedirectToAction("Index", new { stock = buyOrderRequest.StockSymbol });
                }
            }

            BuyOrderResponse buyOrderResponse = await _buyOrdersService.CreateBuyOrder(buyOrderRequest);

            return RedirectToAction("Orders");
        }

        [Route("[action]")]
        [HttpPost]
        [TypeFilter(typeof(CreateOrderActionFilter))]
        public async Task<IActionResult> SellOrder(SellOrderRequest sellOrderRequest)
        {
            var userIdString = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
            if (Guid.TryParse(userIdString, out Guid userId))
            {
                sellOrderRequest.UserID = userId;
                double totalProceeds = sellOrderRequest.Price * sellOrderRequest.Quantity;
                _userBalanceService.AddBalance(userId, totalProceeds);
            }

            SellOrderResponse sellOrderResponse = await _sellOrdersService.CreateSellOrder(sellOrderRequest);

            return RedirectToAction("Orders");
        }

        [Route("[action]")]
        [HttpGet]
        public async Task<IActionResult> Orders()
        {
            var userIdString = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
            Guid userId = Guid.Empty;
            if (Guid.TryParse(userIdString, out Guid parsedId))
            {
                userId = parsedId;
            }

            List<BuyOrderResponse> buyOrders = await _buyOrdersService.GetBuyOrders(userId);
            List<SellOrderResponse> sellOrders = await _sellOrdersService.GetSellOrders(userId);

            Models.Orders orders = new Models.Orders
            {
                BuyOrders = buyOrders,
                SellOrders = sellOrders
            };

            return View(orders);
        }

        [Route("[action]")]
        [HttpGet]
        public IActionResult Cash()
        {
            var userIdString = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
            if (Guid.TryParse(userIdString, out Guid userId))
            {
                ViewBag.CashBalance = _userBalanceService.GetBalance(userId);
            }
            else
            {
                ViewBag.CashBalance = 0.0;
            }

            return View();
        }

        [Route("[action]")]
        [HttpPost]
        public IActionResult AddCash(double amount)
        {
            var userIdString = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
            if (Guid.TryParse(userIdString, out Guid userId) && amount > 0)
            {
                _userBalanceService.AddBalance(userId, amount);
            }

            return RedirectToAction("Cash");
        }

        [Route("[action]")]
        [HttpPost]
        public IActionResult WithdrawCash(double amount)
        {
            var userIdString = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
            if (Guid.TryParse(userIdString, out Guid userId) && amount > 0)
            {
                if (!_userBalanceService.DeductBalance(userId, amount))
                {
                    TempData["Error"] = "Insufficient funds for withdrawal.";
                }
            }

            return RedirectToAction("Cash");
        }
    }
}
