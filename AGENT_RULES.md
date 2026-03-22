# 🤖 System Prompt Extension: StockApp Rules & Context

**READ THIS FIRST** to save tokens and understand the workspace topology. This is the master entry point for all AI agents working on this project.

## 🛑 1. STRICT CONSTRAINTS (Never Violate)
- **NO AUTO-PUSH**: **NEVER** push code to `main`/`master` (`git push`) without explicit user permission. Staging/committing locally is allowed if requested.
- **NO UNPROMPTED COMMANDS**: Do NOT run tests (`npm test`, `dotnet test`) or production builds unless specifically requested.
- **MINIMAL VIABLE CHANGES**: Write minimal implementations. Do not add unrequested "extra" features, complex services, or UI elements. 
- **PARITY**: Replicate the original `.NET` MVC project functionality when building the React frontend.
- **EFFICIENT EDITING**: When modifying files, prioritize `replace_file_content` or `multi_replace_file_content` tools over full file overwrites to save context tokens.

## 🗺️ 2. PROJECT TOPOLOGY (Don't search blindly)
This is a hybrid web application. Do not waste tokens scanning directories; use these exact paths:

*   **Backend (C# / .NET 8 / ASP.NET Core)**: `c:\Code\StockApp\.Net\src\`
    *   `StockApp.Application`: Interfaces, DTOs (`FinnhubStockDataResponse`), Business Logic.
    *   `StockApp.Domain`: Entities (e.g., `ApplicationUser`).
    *   `StockApp.Infrastructure`: Finnhub API service implementations, DB Context.
    *   `StockApp.Web`: API Controllers (`TradeApiController.cs`), Views, `Program.cs`.

*   **Frontend (React / Vite)**: `c:\Code\StockApp\Frontend\src\`
    *   `components/`: React UI components (`Dashboard.jsx`, `Trade.jsx`, `PriceChart.jsx`).
    *   `services/`: Centralized API calls (`api.js`).
    *   `App.jsx`: Main UI routing and state holder.
    *   `index.css`: Global styling.

## 🎯 3. CURRENT TASK STATE
- **Check `todo.md`**: Found at `<project_root>\todo.md`. ALWAYS read this file first to understand the current checklist and objective before taking action.

## 📝 4. PROJECT TERMINOLOGY
- **Stock Data**: Always use the term `Data` (e.g., `StockData`, `GetStockData`, `fetchStockData`) instead of `Candles` or `CandleData` across APIs, Services, and State.
