# StockApp - TODO List

## Upcoming: Graph Implementation Plan
- [ ] **Backend: Historical Data Endpoint**
    - Add endpoint in `TradeApiController` to fetch candle/historical data from Finnhub.
    - Support different time resolutions (D, W, M).
- [ ] **Frontend: Charting Library Integration**
    - Install and set up a charting library (e.g., `chart.js` with `react-chartjs-2`).
- [ ] **Frontend: PriceChart Component**
    - Create a reusable `PriceChart` component.
    - Implement data fetching for historical prices based on selected timeframe.
- [ ] **Frontend: Integration**
    - Replace the placeholder in `Trade.jsx` with the real `PriceChart`.
    - Wire up the "Day", "Month", "Year" buttons to update the chart.


[] TIMEFRAME -- TRADE.JSX -- >  

## Backend: Database Persistence Configuration
- [ ] Update `Program.cs` to conditionally register `ApplicationDbContext` and database repositories based on `appsettings.json` configuration for `DatabaseProvider` (e.g. `"InMemory"`, `"Sqlite"`, `"SqlServer"`).