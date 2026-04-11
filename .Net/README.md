# Stock Trading App

An ASP.NET Core MVC application that displays live stock prices using the **Yahoo Finance** API, allowing users to place buy and sell orders.

## Features
- **Live Updates**: Real-time stock price quotes via Yahoo Finance.
- **Order Management**: Create buy and sell orders with full server-side validation.
- **Login**: Basic in-memory login functionality for demonstration.
- **Orders View**: View a clear list of all executed buy and sell orders with calculated trade amounts.
- **User Roles**: Role-based user system with seeded roles (User, Admin, Analyst, Moderator, Viewer).
- **N-Layer Architecture**: Clean separation of concerns with Controllers, Services, DTOs, and Entities.
- **Unit Testing**: Comprehensive test suite with 20 xUnit test cases covering all service logic.

## Technology Stack
- **Backend**: ASP.NET Core 8.0 (MVC)
- **Market Data**: Yahoo Finance (no API key required)
- **Service layer**: Dependency Injection, Options Pattern, HttpClient
- **Validation**: Data Annotations & Custom Validation Attributes
- **Frontend**: React, Vanilla CSS
- **Testing**: xUnit

## Setup
1. Clone the repository.
2. Run the application:
   ```bash
   dotnet run --project src/StockApp.Web
   ```
   No API keys are required — Yahoo Finance data is fetched without authentication.

## Login Demonstration
The application includes a basic login feature for demonstration purposes:
- **Email**: `admin@test.com`
- **Password**: `123`
- *Note: Session persistence is currently not implemented, so the authenticated state is not maintained across requests.*

## Testing
Run the unit tests:
```bash
dotnet test
```

## Database Configuration
Uses `SqlServer` by default (LocalDB). To use In-Memory database, update `DatabaseProvider` in `appsettings.json`.


## TODO
- [ ] update stock price in the sidebar panel from controller instead of the websocket