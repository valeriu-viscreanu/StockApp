# Stock Trading App

An ASP.NET Core MVC application that displays live stock prices using the **Finnhub.io** API and WebSocket, allowing users to place buy and sell orders.

## Features
- **Live Updates**: Real-time stock price updates via Finnhub WebSockets.
- **Order Management**: Create buy and sell orders with full server-side validation.
- **Orders View**: View a clear list of all executed buy and sell orders with calculated trade amounts.
- **N-Layer Architecture**: Clean separation of concerns with Controllers, Services, DTOs, and Entities.
- **Unit Testing**: Comprehensive test suite with 20 xUnit test cases covering all service logic.

## Technology Stack
- **Backend**: ASP.NET Core 8.0 (MVC)
- **Service layer**: Dependency Injection, Options Pattern, HttpClient
- **Validation**: Data Annotations & Custom Validation Attributes
- **Frontend**: Razor Views, Vanilla CSS, JavaScript (WebSockets)
- **Testing**: xUnit

## Setup
1. Clone the repository.
2. Add your Finnhub.io API token to User Secrets:
   ```bash
   dotnet user-secrets set "FinnhubToken" "your_token_here"
   ```
3. Run the application:
   ```bash
   dotnet run --project StockApp
   ```

## Testing
Run the unit tests:
```bash
dotnet test
```
