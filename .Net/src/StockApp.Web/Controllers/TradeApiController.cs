using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using StockApp.Application.DTO;
using StockApp.Application.ServiceContracts;

namespace StockApp.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    [Authorize(AuthenticationSchemes = Microsoft.AspNetCore.Authentication.JwtBearer.JwtBearerDefaults.AuthenticationScheme)]
    public class TradeApiController : ControllerBase
    {
        private readonly IStockProfileService _stockProfileService;
        private readonly IStockQuoteService _stockQuoteService;
        private readonly IBuyOrdersService _buyOrdersService;
        private readonly ISellOrdersService _sellOrdersService;
        private readonly IUserBalanceService _userBalanceService;
        private readonly IFinnhubService _finnhubService;

        [HttpGet("popular-stocks")]
        public ActionResult<List<PopularStockResponse>> GetPopularStocks()
        {
            var popularStocks = new List<PopularStockResponse>
            {
                new() { Symbol = "MSFT", Name = "Microsoft" },
                new() { Symbol = "AAPL", Name = "Apple" },
                new() { Symbol = "GOOGL", Name = "Alphabet" },
                new() { Symbol = "AMZN", Name = "Amazon" },
                new() { Symbol = "NVDA", Name = "Nvidia" },
                new() { Symbol = "META", Name = "Meta Platforms" },
                new() { Symbol = "TSLA", Name = "Tesla" },
                new() { Symbol = "AMD", Name = "Advanced Micro Devices" },
                new() { Symbol = "JPM", Name = "JPMorgan Chase" },
                new() { Symbol = "V", Name = "Visa" }
            };
            return Ok(popularStocks);
        }

        public TradeApiController(
            IStockProfileService stockProfileService,
            IStockQuoteService stockQuoteService,
            IBuyOrdersService buyOrdersService,
            ISellOrdersService sellOrdersService,
            IUserBalanceService userBalanceService,
            IFinnhubService finnhubService)
        {
            _stockProfileService = stockProfileService;
            _stockQuoteService = stockQuoteService;
            _buyOrdersService = buyOrdersService;
            _sellOrdersService = sellOrdersService;
            _userBalanceService = userBalanceService;
            _finnhubService = finnhubService;
        }

        [HttpGet("profile/{stockSymbol}")]
        public async Task<ActionResult<FinnhubCompanyProfileResponse>> GetCompanyProfile(string stockSymbol)
        {
            var profile = await _stockProfileService.GetCompanyProfile(stockSymbol);
            if (profile == null)
            {
                return NotFound();
            }
            return Ok(profile);
        }

        [HttpGet("data/{stockSymbol}")]
        public async Task<ActionResult<FinnhubStockDataResponse>> GetStockData(string stockSymbol, [FromQuery] string timeframe = "month")
        {
            var now = DateTimeOffset.UtcNow;
            string resolution;
            DateTimeOffset from;

            switch (timeframe)
            {
                case "day":
                    resolution = "60";
                    from = now.AddHours(-20);
                    break;
                case "year":
                    resolution = "D";
                    from = now.AddDays(-365);
                    break;
                default: // month
                    resolution = "D";
                    from = now.AddDays(-20);
                    break;
            }

            var stockData = await _finnhubService.GetStockData(stockSymbol, resolution, from.ToUnixTimeSeconds(), now.ToUnixTimeSeconds());
            if (stockData == null)
            {
                return NotFound();
            }
            return Ok(stockData);
        }

        [HttpGet("quote/{stockSymbol}")]
        public async Task<ActionResult<FinnhubStockQuoteResponse>> GetStockQuote(string stockSymbol)
        {
            var quote = await _stockQuoteService.GetStockPriceQuote(stockSymbol);
            if (quote == null)
            {
                return NotFound();
            }
            return Ok(quote);
        }

        [HttpGet("orders")]
        public async Task<ActionResult<Models.Orders>> GetOrders()
        {
            var userIdString = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
            Guid userId = Guid.Empty;
            if (Guid.TryParse(userIdString, out Guid parsedId))
            {
                userId = parsedId;
            }

            var buyOrders = await _buyOrdersService.GetBuyOrders(userId);
            var sellOrders = await _sellOrdersService.GetSellOrders(userId);

            var orders = new Models.Orders
            {
                BuyOrders = buyOrders,
                SellOrders = sellOrders
            };

            return Ok(orders);
        }

        [HttpPost("buy-order")]
        public async Task<ActionResult<BuyOrderResponse>> CreateBuyOrder(BuyOrderRequest buyOrderRequest)
        {
            if (buyOrderRequest == null)
            {
                return BadRequest();
            }

            try
            {
                var userIdString = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
                if (Guid.TryParse(userIdString, out Guid userId))
                {
                    buyOrderRequest.UserID = userId;
                    double totalCost = buyOrderRequest.Price * buyOrderRequest.Quantity;
                    if (!_userBalanceService.DeductBalance(userId, totalCost))
                    {
                        return BadRequest(new { message = "Insufficient funds for this purchase." });
                    }
                }

                var response = await _buyOrdersService.CreateBuyOrder(buyOrderRequest);
                return CreatedAtAction(nameof(GetOrders), response);
            }
            catch (ArgumentException)
            {
                return BadRequest();
            }
        }

        [HttpGet("quotes")]
        public async Task<ActionResult<Dictionary<string, FinnhubStockQuoteResponse>>> GetBatchQuotes([FromQuery] List<string> symbols)
        {
            if (symbols == null || symbols.Count == 0)
            {
                return BadRequest("symbols query parameter is required");
            }

            var result = new Dictionary<string, FinnhubStockQuoteResponse>();

            foreach (var symbol in symbols)
            {
                var quote = await _stockQuoteService.GetStockPriceQuote(symbol);
                if (quote != null)
                {
                    result[symbol] = quote;
                }
                
                // Add a small delay between requests if there are multiple symbols, 
                // to avoid slamming the free tier API limit
                if (symbols.Count > 3)
                {
                    await Task.Delay(150); // 150ms delay
                }
            }

            return Ok(result);
        }

        [HttpPost("sell-order")]
        public async Task<ActionResult<SellOrderResponse>> CreateSellOrder(SellOrderRequest sellOrderRequest)
        {
            if (sellOrderRequest == null)
            {
                return BadRequest();
            }

            try
            {
                var userIdString = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
                if (Guid.TryParse(userIdString, out Guid userId))
                {
                    sellOrderRequest.UserID = userId;
                    double totalProceeds = sellOrderRequest.Price * sellOrderRequest.Quantity;
                    _userBalanceService.AddBalance(userId, totalProceeds);
                }

                var response = await _sellOrdersService.CreateSellOrder(sellOrderRequest);
                return CreatedAtAction(nameof(GetOrders), response);
            }
            catch (ArgumentException)
            {
                return BadRequest();
            }
        }
    }
}
