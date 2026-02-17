using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using StockApp.DTO;
using StockApp.Filters;
using StockApp.Models;
using StockApp.Options;
using StockApp.ServiceContracts;

namespace StockApp.Controllers
{
    [Route("[controller]")]
    public class TradeController : Controller
    {
        private readonly IFinnhubService _finnhubService;
        private readonly IStocksService _stocksService;
        private readonly TradingOptions _tradingOptions;
        private readonly IConfiguration _configuration;

        public TradeController(
            IFinnhubService finnhubService,
            IStocksService stocksService,
            IOptions<TradingOptions> tradingOptions,
            IConfiguration configuration)
        {
            _finnhubService = finnhubService;
            _stocksService = stocksService;
            _tradingOptions = tradingOptions.Value;
            _configuration = configuration;
        }

        [Route("[action]")]
        [Route("/")]
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            string? stockSymbol = _tradingOptions.DefaultStockSymbol;
            if (string.IsNullOrEmpty(stockSymbol))
            {
                stockSymbol = "MSFT";
            }

            Dictionary<string, object>? companyProfile = await _finnhubService.GetCompanyProfile(stockSymbol);
            Dictionary<string, object>? stockPriceQuote = await _finnhubService.GetStockPriceQuote(stockSymbol);

            StockTrade stockTrade = new StockTrade
            {
                StockSymbol = stockSymbol,
                Quantity = _tradingOptions.DefaultOrderQuantity
            };

            if (companyProfile != null)
            {
                if (companyProfile.ContainsKey("name"))
                {
                    stockTrade.StockName = companyProfile["name"].ToString();
                }
            }

            if (stockPriceQuote != null)
            {
                if (stockPriceQuote.ContainsKey("c"))
                {
                    stockTrade.Price = Convert.ToDouble(stockPriceQuote["c"].ToString());
                }
            }

            ViewBag.FinnhubToken = _configuration["FinnhubToken"];

            return View(stockTrade);
        }

        [Route("[action]")]
        [HttpPost]
        [TypeFilter(typeof(CreateOrderActionFilter))]
        public async Task<IActionResult> BuyOrder(BuyOrderRequest buyOrderRequest)
        {
            BuyOrderResponse buyOrderResponse = await _stocksService.CreateBuyOrder(buyOrderRequest);

            return RedirectToAction("Orders");
        }

        [Route("[action]")]
        [HttpPost]
        [TypeFilter(typeof(CreateOrderActionFilter))]
        public async Task<IActionResult> SellOrder(SellOrderRequest sellOrderRequest)
        {
            SellOrderResponse sellOrderResponse = await _stocksService.CreateSellOrder(sellOrderRequest);

            return RedirectToAction("Orders");
        }

        [Route("[action]")]
        [HttpGet]
        public async Task<IActionResult> Orders()
        {
            List<BuyOrderResponse> buyOrders = await _stocksService.GetBuyOrders();
            List<SellOrderResponse> sellOrders = await _stocksService.GetSellOrders();

            Models.Orders orders = new Models.Orders
            {
                BuyOrders = buyOrders,
                SellOrders = sellOrders
            };

            return View(orders);
        }
    }
}
