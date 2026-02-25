using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using StockApp.Application.DTO;
using StockApp.Application.ServiceContracts;

namespace StockApp.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    [Authorize]
    public class TradeApiController : ControllerBase
    {
        private readonly IStockProfileService _stockProfileService;
        private readonly IStockQuoteService _stockQuoteService;
        private readonly IBuyOrdersService _buyOrdersService;
        private readonly ISellOrdersService _sellOrdersService;

        public TradeApiController(
            IStockProfileService stockProfileService,
            IStockQuoteService stockQuoteService,
            IBuyOrdersService buyOrdersService,
            ISellOrdersService sellOrdersService)
        {
            _stockProfileService = stockProfileService;
            _stockQuoteService = stockQuoteService;
            _buyOrdersService = buyOrdersService;
            _sellOrdersService = sellOrdersService;
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
            var buyOrders = await _buyOrdersService.GetBuyOrders();
            var sellOrders = await _sellOrdersService.GetSellOrders();

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
                var response = await _buyOrdersService.CreateBuyOrder(buyOrderRequest);
                return CreatedAtAction(nameof(GetOrders), response);
            }
            catch (ArgumentException)
            {
                return BadRequest();
            }
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
