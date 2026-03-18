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
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

builder.Host.UseSerilog((context, loggerConfiguration) =>
{
    loggerConfiguration
        .ReadFrom.Configuration(context.Configuration)
        .Enrich.FromLogContext();
});


builder.Services.AddControllersWithViews();

var jwtKey = builder.Configuration["Jwt:Key"]
    ?? throw new InvalidOperationException(
        "JWT signing key is not configured. Please set 'Jwt:Key' in appsettings.json or user secrets.");

builder.Services.AddAuthentication(options =>
{
    // Cookies are standard for browsers/views.
    // JWT handles the API side.
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
})
.AddJwtBearer(JwtBearerDefaults.AuthenticationScheme, options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = builder.Configuration["Jwt:Issuer"],
        ValidAudience = builder.Configuration["Jwt:Audience"],
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey))
    };
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
builder.Services.AddSingleton<IRefreshTokenService, InMemoryRefreshTokenService>();
builder.Services.AddSingleton<IUserBalanceService, InMemoryUserBalanceService>();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
else
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
