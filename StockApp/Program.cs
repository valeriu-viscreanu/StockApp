using StockApp.Options;
using StockApp.Application.ServiceContracts;
using StockApp.Application.Services;
using StockApp.Application.Mappers;
using StockApp.Application.DTO;
using StockApp.Domain.RepositoryContracts;
using StockApp.Infrastructure.Repositories;
using StockApp.Infrastructure.Services;
using StockApp.Middleware;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

builder.Host.UseSerilog((context, loggerConfiguration) =>
{
    loggerConfiguration
        .ReadFrom.Configuration(context.Configuration)
        .Enrich.FromLogContext();
});


builder.Services.AddControllersWithViews();

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = "Cookies";
    options.DefaultSignInScheme = "Cookies";
    options.DefaultChallengeScheme = "Cookies";
})
.AddCookie("Cookies", options =>
{
    options.LoginPath = "/Account/Login";
    options.LogoutPath = "/Account/Logout";
    options.AccessDeniedPath = "/Account/AccessDenied";
    options.ExpireTimeSpan = TimeSpan.FromMinutes(60);
});

builder.Services.AddAuthorization();

builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(policy =>
    {
        policy.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader();
    });
});
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
builder.Services.AddSingleton<IAccountService, InMemoryAccountService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseMiddleware<ExceptionHandlingMiddleware>();
}

app.UseStaticFiles();
app.UseRouting();
app.UseCors();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();

public partial class Program
{
}
