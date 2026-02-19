using StockApp.Options;
using StockApp.ServiceContracts;
using StockApp.Services;
using StockApp.Middleware;
using StockApp.Repositories;
using StockApp.Mappers;
using StockApp.DTO;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

builder.Host.UseSerilog((context, loggerConfiguration) =>
{
    loggerConfiguration
        .ReadFrom.Configuration(context.Configuration)
        .Enrich.FromLogContext();
});

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.Configure<TradingOptions>(builder.Configuration.GetSection("TradingOptions"));
builder.Services.AddHttpClient();
builder.Services.AddSingleton<IFinnhubService, FinnhubService>();
builder.Services.AddSingleton<IStockProfileService>(sp => sp.GetRequiredService<IFinnhubService>());
builder.Services.AddSingleton<IStockQuoteService>(sp => sp.GetRequiredService<IFinnhubService>());

builder.Services.AddSingleton<IBuyOrderRepository, InMemoryBuyOrderRepository>();
builder.Services.AddSingleton<ISellOrderRepository, InMemorySellOrderRepository>();
builder.Services.AddSingleton<IRequestValidator<BuyOrderRequest>, DataAnnotationsRequestValidator<BuyOrderRequest>>();
builder.Services.AddSingleton<IRequestValidator<SellOrderRequest>, DataAnnotationsRequestValidator<SellOrderRequest>>();
builder.Services.AddSingleton<IBuyOrderMapper, BuyOrderMapper>();
builder.Services.AddSingleton<ISellOrderMapper, SellOrderMapper>();
builder.Services.AddSingleton<IBuyOrdersService, BuyOrdersService>();
builder.Services.AddSingleton<ISellOrdersService, SellOrdersService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseMiddleware<ExceptionHandlingMiddleware>();
}

app.UseStaticFiles();
app.UseRouting();
app.UseAuthorization();

app.MapControllers();

app.Run();

public partial class Program
{
}
