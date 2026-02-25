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
        private readonly TradingOptions _tradingOptions;
        private readonly IConfiguration _configuration;

        public TradeController(
            IStockProfileService stockProfileService,
            IStockQuoteService stockQuoteService,
            IBuyOrdersService buyOrdersService,
            ISellOrdersService sellOrdersService,
            IOptions<TradingOptions> tradingOptions,
            IConfiguration configuration)
        {
            _stockProfileService = stockProfileService;
            _stockQuoteService = stockQuoteService;
            _buyOrdersService = buyOrdersService;
            _sellOrdersService = sellOrdersService;
            _tradingOptions = tradingOptions.Value;
            _configuration = configuration;
        }

        [Route("[action]/{stock?}")]
        [Route("/")]
        [HttpGet]
        public async Task<IActionResult> Index(string? stock)
        {
            string? stockSymbol = stock;
            if (string.IsNullOrWhiteSpace(stockSymbol))
            {
                stockSymbol = _tradingOptions.DefaultStockSymbol;
            }

            if (string.IsNullOrEmpty(stockSymbol))
            {
                stockSymbol = "MSFT";
            }

            // Attempt to get quote. If price is 0, it might be a name search
            FinnhubStockQuoteResponse? stockPriceQuote = await _stockQuoteService.GetStockPriceQuote(stockSymbol);
            
            if (stockPriceQuote == null || stockPriceQuote.CurrentPrice == 0)
            {
                // Try searching for the text to see if it's a company name
                var searchResults = await _stockProfileService.SearchStocks(stockSymbol);
                if (searchResults?.Result != null && searchResults.Result.Count > 0)
                {
                    // Take the first matching symbol (best guess)
                    stockSymbol = searchResults.Result[0].Symbol;
                    stockPriceQuote = await _stockQuoteService.GetStockPriceQuote(stockSymbol!);
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

            return View(stockTrade);
        }

        [Route("[action]")]
        [HttpPost]
        [TypeFilter(typeof(CreateOrderActionFilter))]
        public async Task<IActionResult> BuyOrder(BuyOrderRequest buyOrderRequest)
        {
            BuyOrderResponse buyOrderResponse = await _buyOrdersService.CreateBuyOrder(buyOrderRequest);

            return RedirectToAction("Orders");
        }

        [Route("[action]")]
        [HttpPost]
        [TypeFilter(typeof(CreateOrderActionFilter))]
        public async Task<IActionResult> SellOrder(SellOrderRequest sellOrderRequest)
        {
            SellOrderResponse sellOrderResponse = await _sellOrdersService.CreateSellOrder(sellOrderRequest);

            return RedirectToAction("Orders");
        }

        [Route("[action]")]
        [HttpGet]
        public async Task<IActionResult> Orders()
        {
            List<BuyOrderResponse> buyOrders = await _buyOrdersService.GetBuyOrders();
            List<SellOrderResponse> sellOrders = await _sellOrdersService.GetSellOrders();

            Models.Orders orders = new Models.Orders
            {
                BuyOrders = buyOrders,
                SellOrders = sellOrders
            };

            return View(orders);
        }
    }
}
