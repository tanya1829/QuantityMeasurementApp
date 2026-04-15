# ── Stage 1: Build ────────────────────────────────────────────────────
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src

# Copy solution file
COPY QuantityMeasurementApp.sln .

# Copy all project files first (for better layer caching)
COPY QuantityMeasurementApp.API/QuantityMeasurementApp.API.csproj                         QuantityMeasurementApp.API/
COPY QuantityMeasurementApp.BusinessLayer/QuantityMeasurementApp.BusinessLayer.csproj     QuantityMeasurementApp.BusinessLayer/
COPY QuantityMeasurementApp.RepoLayer/QuantityMeasurementApp.RepoLayer.csproj             QuantityMeasurementApp.RepoLayer/
COPY QuantityMeasurementApp.ModelLayer/QuantityMeasurementApp.ModelLayer.csproj           QuantityMeasurementApp.ModelLayer/

# Restore dependencies
RUN dotnet restore QuantityMeasurementApp.API/QuantityMeasurementApp.API.csproj

# Copy all source code
COPY QuantityMeasurementApp.API/           QuantityMeasurementApp.API/
COPY QuantityMeasurementApp.BusinessLayer/ QuantityMeasurementApp.BusinessLayer/
COPY QuantityMeasurementApp.RepoLayer/     QuantityMeasurementApp.RepoLayer/
COPY QuantityMeasurementApp.ModelLayer/    QuantityMeasurementApp.ModelLayer/

# Publish the API
RUN dotnet publish QuantityMeasurementApp.API/QuantityMeasurementApp.API.csproj \
    -c Release \
    -o /app/publish \
    --no-restore

# ── Stage 2: Runtime ──────────────────────────────────────────────────
FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS runtime
WORKDIR /app

COPY --from=build /app/publish .

ENV ASPNETCORE_URLS=http://+:8080
ENV ASPNETCORE_ENVIRONMENT=Production

EXPOSE 8080

ENTRYPOINT ["dotnet", "QuantityMeasurementApp.API.dll"]