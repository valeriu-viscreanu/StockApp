# Use the official .NET SDK image to build the application
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build-env
WORKDIR /app

# Copy the solution and project files
COPY StockApp.sln ./
COPY src/StockApp.Web/StockApp.Web.csproj ./src/StockApp.Web/
COPY src/StockApp.Application/StockApp.Application.csproj ./src/StockApp.Application/
COPY src/StockApp.Domain/StockApp.Domain.csproj ./src/StockApp.Domain/
COPY src/StockApp.Infrastructure/StockApp.Infrastructure.csproj ./src/StockApp.Infrastructure/
COPY test/StockAppTests/StockAppTests.csproj ./test/StockAppTests/

# Restore dependencies
RUN dotnet restore

# Copy the remaining files and build
COPY . ./
RUN dotnet publish src/StockApp.Web/StockApp.Web.csproj -c Release -o out

# Use the official .NET runtime image to run the application
FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /app
COPY --from=build-env /app/out .

# Expose the port the app runs on
EXPOSE 8080

# Start the application
ENTRYPOINT ["dotnet", "StockApp.Web.dll"]
