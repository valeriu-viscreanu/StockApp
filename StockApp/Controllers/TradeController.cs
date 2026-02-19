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

            FinnhubCompanyProfileResponse? companyProfile = await _stockProfileService.GetCompanyProfile(stockSymbol);
            FinnhubStockQuoteResponse? stockPriceQuote = await _stockQuoteService.GetStockPriceQuote(stockSymbol);

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
                stockTrade.Price = stockPriceQuote.CurrentPrice;
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
