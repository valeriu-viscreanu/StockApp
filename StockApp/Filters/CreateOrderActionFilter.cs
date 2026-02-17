using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using StockApp.DTO;
using StockApp.Models;

namespace StockApp.Filters
{
    public class CreateOrderActionFilter : IActionFilter
    {
        private readonly IConfiguration _configuration;

        public CreateOrderActionFilter(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public void OnActionExecuting(ActionExecutingContext context)
        {
            foreach (object? actionArgument in context.ActionArguments.Values)
            {
                switch (actionArgument)
                {
                    case BuyOrderRequest buyOrderRequest:
                        buyOrderRequest.DateAndTimeOfOrder = DateTime.Now;
                        context.ModelState.Remove(nameof(BuyOrderRequest.DateAndTimeOfOrder));
                        break;
                    case SellOrderRequest sellOrderRequest:
                        sellOrderRequest.DateAndTimeOfOrder = DateTime.Now;
                        context.ModelState.Remove(nameof(SellOrderRequest.DateAndTimeOfOrder));
                        break;
                }
            }

            if (!context.ModelState.IsValid)
            {
                if (context.Controller is Controller controller)
                {
                    controller.ViewBag.FinnhubToken = _configuration["FinnhubToken"];
                    controller.ViewBag.Errors = context.ModelState.Values
                        .SelectMany(v => v.Errors)
                        .Select(e => e.ErrorMessage)
                        .ToList();

                    StockTrade stockTrade = CreateStockTradeFromRequest(context.ActionArguments.Values);
                    context.Result = controller.View("Index", stockTrade);
                }
                else
                {
                    context.Result = new RedirectToActionResult("Index", "Trade", null);
                }
            }
        }

        public void OnActionExecuted(ActionExecutedContext context)
        {
        }

        private static StockTrade CreateStockTradeFromRequest(IEnumerable<object?> actionArguments)
        {
            foreach (object? actionArgument in actionArguments)
            {
                if (actionArgument is BuyOrderRequest buyOrderRequest)
                {
                    return new StockTrade
                    {
                        StockSymbol = buyOrderRequest.StockSymbol,
                        StockName = buyOrderRequest.StockName,
                        Price = buyOrderRequest.Price,
                        Quantity = buyOrderRequest.Quantity
                    };
                }

                if (actionArgument is SellOrderRequest sellOrderRequest)
                {
                    return new StockTrade
                    {
                        StockSymbol = sellOrderRequest.StockSymbol,
                        StockName = sellOrderRequest.StockName,
                        Price = sellOrderRequest.Price,
                        Quantity = sellOrderRequest.Quantity
                    };
                }
            }

            return new StockTrade();
        }
    }
}
