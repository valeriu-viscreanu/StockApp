using Imposter.Abstractions;
using StockApp.Application.ServiceContracts;
using StockApp.Domain.RepositoryContracts;

[assembly: GenerateImposter(typeof(IStockProfileService))]
[assembly: GenerateImposter(typeof(IStockQuoteService))]
[assembly: GenerateImposter(typeof(IBuyOrdersService))]
[assembly: GenerateImposter(typeof(ISellOrdersService))]
[assembly: GenerateImposter(typeof(IAccountProfileService))]
[assembly: GenerateImposter(typeof(IBuyOrderRepository))]
[assembly: GenerateImposter(typeof(ISellOrderRepository))]
[assembly: GenerateImposter(typeof(ICashRepository))]
[assembly: GenerateImposter(typeof(IAccountRepository))]
[assembly: GenerateImposter(typeof(IOrderStatusRepository))]
[assembly: GenerateImposter(typeof(IUserOperationRepository))]
