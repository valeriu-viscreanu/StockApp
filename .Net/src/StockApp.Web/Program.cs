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
using Microsoft.EntityFrameworkCore;
using StockApp.Infrastructure.Database;
using StockApp.Infrastructure.Database.Repositories;
using StockApp.Infrastructure.Database.Services;

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

var provider = builder.Configuration["DatabaseProvider"] ?? "InMemory";
var sqlServerConnectionString = builder.Configuration.GetConnectionString("SqlServerConnection");

builder.Services.AddDbContext<ApplicationDbContext>(options =>
{
    if (provider.Equals("SqlServer", StringComparison.OrdinalIgnoreCase))
    {
        options.UseSqlServer(sqlServerConnectionString!);
    }
    else
    {
        options.UseInMemoryDatabase("StockAppInMemoryDb");
    }
});

builder.Services.AddScoped<IBuyOrderRepository, BuyOrderRepository>();
builder.Services.AddScoped<ISellOrderRepository, SellOrderRepository>();
builder.Services.AddScoped<IUserOperationRepository, UserOperationRepository>();
builder.Services.AddScoped<IUserHoldingRepository, UserHoldingRepository>();
builder.Services.AddScoped<IUserDetailsRepository, UserDetailsRepository>();

builder.Services.AddScoped<IRequestValidator<BuyOrderRequest>, DataAnnotationsRequestValidator<BuyOrderRequest>>();
builder.Services.AddScoped<IRequestValidator<SellOrderRequest>, DataAnnotationsRequestValidator<SellOrderRequest>>();
builder.Services.AddScoped<IBuyOrderMapper, BuyOrderMapper>();
builder.Services.AddScoped<ISellOrderMapper, SellOrderMapper>();
builder.Services.AddScoped<IBuyOrdersService, BuyOrdersService>();
builder.Services.AddScoped<ISellOrdersService, SellOrdersService>();
if (provider.Equals("SqlServer", StringComparison.OrdinalIgnoreCase))
{
    builder.Services.AddScoped<IAccountService, AccountService>();
    builder.Services.AddScoped<IRefreshTokenService, RefreshTokenService>();
    builder.Services.AddScoped<IUserBalanceService, UserBalanceService>();
}
else
{
    builder.Services.AddScoped<IAccountService, StockApp.Infrastructure.Services.InMemoryAccountService>();
    builder.Services.AddScoped<IRefreshTokenService, StockApp.Infrastructure.Services.InMemoryRefreshTokenService>();
    builder.Services.AddSingleton<IUserBalanceService, StockApp.Infrastructure.Services.InMemoryUserBalanceService>();
}

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
    if (provider.Equals("SqlServer", StringComparison.OrdinalIgnoreCase))
    {
        dbContext.Database.Migrate();
    }
    else
    {
        dbContext.Database.EnsureCreated();
    }
}

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
