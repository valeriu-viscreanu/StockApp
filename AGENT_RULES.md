# 🤖 System Prompt Extension: StockApp Rules & Context

**READ THIS FIRST** to save tokens and understand the workspace topology. This is the master entry point for all AI agents working on this project.

## 🛑 1. STRICT CONSTRAINTS (Never Violate)
- **NO AUTO-PUSH**: **NEVER** push code to `main`/`master` (`git push`) without explicit user permission. Staging/committing locally is allowed if requested.
- **NO UNPROMPTED COMMANDS**: Do NOT run tests (`npm test`, `dotnet test`) or production builds unless specifically requested.
- **MINIMAL VIABLE CHANGES**: Write minimal implementations. Do not add unrequested "extra" features, complex services, or UI elements. 
- **PARITY**: Replicate the original `.NET` MVC project functionality when building the React frontend.
- **EFFICIENT EDITING**: When modifying files, prioritize `replace_file_content` or `multi_replace_file_content` tools over full file overwrites to save context tokens.
- **PLAIN COMMIT MESSAGES**: Never use `fix:`, `feat:`, or other conventional prefixes in git commit messages unless explicitly authorized. Keep them descriptive but plain.
- **CONCISE README**: Do not overload `README.md` with excessive technical details or migration logs. Keep it focused on the high-level description and basic setup. Move detailed notes to specific context files if needed.

## 🗺️ 2. PROJECT TOPOLOGY (Don't search blindly)
This is a hybrid web application. Do not waste tokens scanning directories; instead, read the architectural maps based on your current task:

*   **Backend Details**: Read `./.Net/AGENT_CONTEXT.md` for C# Clean Architecture mapping and services.
*   **Frontend Details**: Read `./Frontend/AGENT_CONTEXT.md` for React components, styling, and `api.js` mappings.

**⚠️ STRICT RULE 5 (MAINTENANCE)**: Agents must proactively edit the relevant `AGENT_CONTEXT.md` files if they make structural changes or introduce major new architectural patterns.

## 🚀 2.5 STARTING THE APPLICATION
To save tokens, use these commands immediately when asked to "start the app":
- **Backend**: `cd .Net/src/StockApp.Web && dotnet watch run`
- **Frontend**: `cd Frontend && npm run dev`

## 🎯 3. CURRENT TASK STATE
- **Check `todo.md`**: Found at `<project_root>\todo.md`. ALWAYS read this file first to understand the current checklist and objective before taking action.

## 📝 4. PROJECT TERMINOLOGY
- **Stock Data**: Always use the term `Data` (e.g., `StockData`, `GetStockData`, `fetchStockData`) instead of `Candles` or `CandleData` across APIs, Services, and State.
