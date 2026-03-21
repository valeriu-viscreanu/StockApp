# Agent Rules & Constraints

## ⚠️ STRICT: GIT OPERATIONS
- **NEVER** push code to `main` or `master` branches without explicit user approval.
- You may stage and commit changes (if asked), but the final `git push` must always be manually requested or confirmed by the user.

## Implementation Standards
- **Minimal implementations always**: Follow the "minimal implementation" rule. Do not add complex, unrequested backend services or UI elements.
- **Parity**: Replicate functionality from the .NET project as closely as possible when building the React frontend.
- **Permission**: Ask permission before performing any "extra" steps.
- **Testing/Building**: Do NOT run tests (`npm test`, `dotnet test`) or production builds unless specifically requested, to save time/tokens.
