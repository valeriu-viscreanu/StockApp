using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using StockApp.Application.DTO;
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
            foreach (var actionArgument in context.ActionArguments)
            {
                string parameterName = actionArgument.Key;
                object? argumentValue = actionArgument.Value;

                if (argumentValue is BuyOrderRequest buyOrderRequest)
                {
                    buyOrderRequest.DateAndTimeOfOrder = DateTime.Now;
                    // Remove both prefixed and non-prefixed keys to be safe
                    context.ModelState.Remove(nameof(BuyOrderRequest.DateAndTimeOfOrder));
                    context.ModelState.Remove($"{parameterName}.{nameof(BuyOrderRequest.DateAndTimeOfOrder)}");
                }
                else if (argumentValue is SellOrderRequest sellOrderRequest)
                {
                    sellOrderRequest.DateAndTimeOfOrder = DateTime.Now;
                    // Remove both prefixed and non-prefixed keys to be safe
                    context.ModelState.Remove(nameof(SellOrderRequest.DateAndTimeOfOrder));
                    context.ModelState.Remove($"{parameterName}.{nameof(SellOrderRequest.DateAndTimeOfOrder)}");
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
