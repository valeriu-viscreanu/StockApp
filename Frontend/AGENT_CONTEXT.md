# 🗺️ Frontend Context (React)

This file maps the frontend architecture so agents do not need to scan directories blindly.

## Tech Stack
- React / Vite / CSS Modules (Vanilla JS)

## Architecture
The frontend is built as a Single Page Application (SPA) located at `c:\Code\StockApp\Frontend\src\`.

### Directory Structure
- **`components/`**: All visual React UI components.
  - Examples: `Dashboard.jsx`, `Trade.jsx`, `PriceChart.jsx` (Charting), `Navbar.jsx`.
- **`services/`**: Centralized API abstraction layer.
  - Key File: `api.js` (Handles all `fetch` logic and endpoint wrapping).
- **Core Files**:
  - `App.jsx`: Main UI routing, overarching state holder, and orchestrator.
  - `index.css`: Global application styling.

## Development Commands
- Start Server: `npm run dev` (Runs the Vite dev server, typically on `http://localhost:5173`).
- **Do not run `npm run build` or `npm test` unless specifically asked.**
