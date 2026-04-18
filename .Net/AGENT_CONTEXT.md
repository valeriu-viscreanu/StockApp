# 🗺️ Backend Context (.NET 8)

This file maps the backend architecture so agents do not need to scan directories blindly.

## Tech Stack
- C# / .NET 8 / ASP.NET Core MVC & API

## Architecture (Clean Architecture Pattern)
The backend is located in `./src/` and separated into four main projects:

1.  **`StockApp.Application`**: 
    - Purpose: Interfaces, DTOs, and Business Logic.
    - Key Files: `FinnhubStockDataResponse.cs` (DTOs), `IFinnhubService.cs` (Contracts).

2.  **`StockApp.Domain`**: 
    - Purpose: Core Entities.
    - Key Files: `ApplicationUser.cs` (Identity).

3.  **`StockApp.Infrastructure`**: 
    - Purpose: External service implementations and Database Context.
    - Key Files: `FinnhubService.cs` (Third-party integrations).

4.  **`StockApp.Web`**: 
    - Purpose: Controllers, Views, and Application Entry Point.
    - Key Files: `TradeApiController.cs`, `Program.cs`.

## Development Commands
- Start Server: `dotnet watch run` (Run from `./src/StockApp.Web`).
