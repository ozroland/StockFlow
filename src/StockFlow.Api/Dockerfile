# Step 1: Build
FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /app

# Copy Directory.Build files from root
COPY ["Directory.Build.props", "."]
COPY ["Directory.Packages.props", "."]

# Copy csproj files and restore dependencies
COPY ["src/StockFlow.Api/StockFlow.Api.csproj", "src/StockFlow.Api/"]
COPY ["src/StockFlow.Application/StockFlow.Application.csproj", "src/StockFlow.Application/"]
COPY ["src/StockFlow.Domain/StockFlow.Domain.csproj", "src/StockFlow.Domain/"]
COPY ["src/StockFlow.Infrastructure/StockFlow.Infrastructure.csproj", "src/StockFlow.Infrastructure/"]

# Run restore for the API project
RUN dotnet restore "src/StockFlow.Api/StockFlow.Api.csproj"

# Copy the entire source code
COPY . .

# Build and Publish
WORKDIR "/app/src/StockFlow.Api"
RUN dotnet publish "StockFlow.Api.csproj" -c Release -o /app/publish /p:UseAppHost=false

# Step 2: Runtime
FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS final
WORKDIR /app
EXPOSE 8080
COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "StockFlow.Api.dll"]