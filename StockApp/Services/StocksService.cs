using System.ComponentModel.DataAnnotations;
using StockApp.DTO;
using StockApp.Entities;
using StockApp.ServiceContracts;

namespace StockApp.Services
{
    public class StocksService : IStocksService
    {
        private readonly List<BuyOrder> _buyOrders;
        private readonly List<SellOrder> _sellOrders;

        public StocksService()
        {
            _buyOrders = new List<BuyOrder>();
            _sellOrders = new List<SellOrder>();
        }

        public Task<BuyOrderResponse> CreateBuyOrder(BuyOrderRequest? buyOrderRequest)
        {
            if (buyOrderRequest == null)
            {
                throw new ArgumentNullException(nameof(buyOrderRequest));
            }

            // Validate using data annotations
            ValidationHelper.ModelValidation(buyOrderRequest);

            BuyOrder buyOrder = new BuyOrder
            {
                BuyOrderID = Guid.NewGuid(),
                StockSymbol = buyOrderRequest.StockSymbol,
                StockName = buyOrderRequest.StockName,
                DateAndTimeOfOrder = buyOrderRequest.DateAndTimeOfOrder,
                Quantity = buyOrderRequest.Quantity,
                Price = buyOrderRequest.Price
            };

            _buyOrders.Add(buyOrder);

            BuyOrderResponse response = new BuyOrderResponse
            {
                BuyOrderID = buyOrder.BuyOrderID,
                StockSymbol = buyOrder.StockSymbol,
                StockName = buyOrder.StockName,
                DateAndTimeOfOrder = buyOrder.DateAndTimeOfOrder,
                Quantity = buyOrder.Quantity,
                Price = buyOrder.Price,
                TradeAmount = buyOrder.Quantity * buyOrder.Price
            };

            return Task.FromResult(response);
        }

        public Task<SellOrderResponse> CreateSellOrder(SellOrderRequest? sellOrderRequest)
        {
            if (sellOrderRequest == null)
            {
                throw new ArgumentNullException(nameof(sellOrderRequest));
            }

            // Validate using data annotations
            ValidationHelper.ModelValidation(sellOrderRequest);

            SellOrder sellOrder = new SellOrder
            {
                SellOrderID = Guid.NewGuid(),
                StockSymbol = sellOrderRequest.StockSymbol,
                StockName = sellOrderRequest.StockName,
                DateAndTimeOfOrder = sellOrderRequest.DateAndTimeOfOrder,
                Quantity = sellOrderRequest.Quantity,
                Price = sellOrderRequest.Price
            };

            _sellOrders.Add(sellOrder);

            SellOrderResponse response = new SellOrderResponse
            {
                SellOrderID = sellOrder.SellOrderID,
                StockSymbol = sellOrder.StockSymbol,
                StockName = sellOrder.StockName,
                DateAndTimeOfOrder = sellOrder.DateAndTimeOfOrder,
                Quantity = sellOrder.Quantity,
                Price = sellOrder.Price,
                TradeAmount = sellOrder.Quantity * sellOrder.Price
            };

            return Task.FromResult(response);
        }

        public Task<List<BuyOrderResponse>> GetBuyOrders()
        {
            List<BuyOrderResponse> buyOrderResponses = _buyOrders
                .Select(buyOrder => new BuyOrderResponse
                {
                    BuyOrderID = buyOrder.BuyOrderID,
                    StockSymbol = buyOrder.StockSymbol,
                    StockName = buyOrder.StockName,
                    DateAndTimeOfOrder = buyOrder.DateAndTimeOfOrder,
                    Quantity = buyOrder.Quantity,
                    Price = buyOrder.Price,
                    TradeAmount = buyOrder.Quantity * buyOrder.Price
                })
                .ToList();

            return Task.FromResult(buyOrderResponses);
        }

        public Task<List<SellOrderResponse>> GetSellOrders()
        {
            List<SellOrderResponse> sellOrderResponses = _sellOrders
                .Select(sellOrder => new SellOrderResponse
                {
                    SellOrderID = sellOrder.SellOrderID,
                    StockSymbol = sellOrder.StockSymbol,
                    StockName = sellOrder.StockName,
                    DateAndTimeOfOrder = sellOrder.DateAndTimeOfOrder,
                    Quantity = sellOrder.Quantity,
                    Price = sellOrder.Price,
                    TradeAmount = sellOrder.Quantity * sellOrder.Price
                })
                .ToList();

            return Task.FromResult(sellOrderResponses);
        }
    }
}
