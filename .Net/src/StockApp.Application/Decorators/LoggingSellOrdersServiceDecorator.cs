using Microsoft.Extensions.Logging;
using StockApp.Application.DTO;
using StockApp.Application.ServiceContracts;
using System.Diagnostics;

namespace StockApp.Application.Decorators
{
    public class LoggingSellOrdersServiceDecorator : ISellOrdersService
    {
        private readonly ISellOrdersService _inner;
        private readonly ILogger<LoggingSellOrdersServiceDecorator> _logger;

        public LoggingSellOrdersServiceDecorator(
            ISellOrdersService inner,
            ILogger<LoggingSellOrdersServiceDecorator> logger)
        {
            _inner = inner;
            _logger = logger;
        }

        public async Task<SellOrderResponse> CreateSellOrder(SellOrderRequest? sellOrderRequest)
        {
            _logger.LogInformation(
                "Creating sell order: {Symbol}, Qty={Quantity}, Price={Price}, User={UserID}",
                sellOrderRequest?.StockSymbol, sellOrderRequest?.Quantity,
                sellOrderRequest?.Price, sellOrderRequest?.UserID);

            var sw = Stopwatch.StartNew();
            try
            {
                var result = await _inner.CreateSellOrder(sellOrderRequest);
                sw.Stop();
                _logger.LogInformation(
                    "Sell order created successfully: OrderID={OrderID} in {ElapsedMs}ms",
                    result.SellOrderID, sw.ElapsedMilliseconds);
                return result;
            }
            catch (Exception ex)
            {
                sw.Stop();
                _logger.LogError(ex,
                    "Sell order failed for {Symbol} after {ElapsedMs}ms",
                    sellOrderRequest?.StockSymbol, sw.ElapsedMilliseconds);
                throw;
            }
        }

        public async Task<List<SellOrderResponse>> GetSellOrders(Guid userID)
        {
            _logger.LogInformation("Fetching sell orders for User={UserID}", userID);

            var sw = Stopwatch.StartNew();
            var results = await _inner.GetSellOrders(userID);
            sw.Stop();

            _logger.LogInformation(
                "Retrieved {Count} sell orders for User={UserID} in {ElapsedMs}ms",
                results.Count, userID, sw.ElapsedMilliseconds);

            return results;
        }
    }
}
