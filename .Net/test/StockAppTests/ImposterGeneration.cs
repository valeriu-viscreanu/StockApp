using Imposter.Abstractions;
using StockApp.Application.ServiceContracts;

[assembly: GenerateImposter(typeof(IStockProfileService))]
[assembly: GenerateImposter(typeof(IStockQuoteService))]
[assembly: GenerateImposter(typeof(IBuyOrdersService))]
[assembly: GenerateImposter(typeof(ISellOrdersService))]
