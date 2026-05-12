using Microsoft.Extensions.Logging;
using StockApp.Application.DTO;
using StockApp.Application.ServiceContracts;
using System.Diagnostics;

namespace StockApp.Application.Decorators
{
    public class LoggingBuyOrdersServiceDecorator : IBuyOrdersService
    {
        private readonly IBuyOrdersService _inner;
        private readonly ILogger<LoggingBuyOrdersServiceDecorator> _logger;

        public LoggingBuyOrdersServiceDecorator(
            IBuyOrdersService inner,
            ILogger<LoggingBuyOrdersServiceDecorator> logger)
        {
            _inner = inner;
            _logger = logger;
        }

        public async Task<BuyOrderResponse> CreateBuyOrder(BuyOrderRequest? buyOrderRequest)
        {
            _logger.LogInformation(
                "Creating buy order: {Symbol}, Qty={Quantity}, Price={Price}, User={UserID}",
                buyOrderRequest?.StockSymbol, buyOrderRequest?.Quantity,
                buyOrderRequest?.Price, buyOrderRequest?.UserID);

            var sw = Stopwatch.StartNew();
            try
            {
                var result = await _inner.CreateBuyOrder(buyOrderRequest);
                sw.Stop();
                _logger.LogInformation(
                    "Buy order created successfully: OrderID={OrderID} in {ElapsedMs}ms",
                    result.BuyOrderID, sw.ElapsedMilliseconds);
                return result;
            }
            catch (Exception ex)
            {
                sw.Stop();
                _logger.LogError(ex,
                    "Buy order failed for {Symbol} after {ElapsedMs}ms",
                    buyOrderRequest?.StockSymbol, sw.ElapsedMilliseconds);
                throw;
            }
        }

        public async Task<List<BuyOrderResponse>> GetBuyOrders(Guid userID)
        {
            _logger.LogInformation("Fetching buy orders for User={UserID}", userID);

            var sw = Stopwatch.StartNew();
            var results = await _inner.GetBuyOrders(userID);
            sw.Stop();

            _logger.LogInformation(
                "Retrieved {Count} buy orders for User={UserID} in {ElapsedMs}ms",
                results.Count, userID, sw.ElapsedMilliseconds);

            return results;
        }
    }
}
