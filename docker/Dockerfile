# =============================================================================
# Dockerfile for Senado .NET 8 Application
# Multi-stage build for optimized image size
# =============================================================================

# Stage 1: Build
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copy csproj and restore dependencies
COPY ["src/Senado.Api/*.csproj", "Senado.Api/"]
COPY ["src/Senado.Core/*.csproj", "Senado.Core/"]
COPY ["src/Senado.Infrastructure/*.csproj", "Senado.Infrastructure/"]
RUN dotnet restore "Senado.Api/Senado.Api.csproj"

# Copy everything and build
COPY src/ .
WORKDIR "/src/Senado.Api"
RUN dotnet build "Senado.Api.csproj" -c Release -o /app/build

# Stage 2: Publish
FROM build AS publish
RUN dotnet publish "Senado.Api.csproj" -c Release -o /app/publish /p:UseAppHost=false

# Stage 3: Runtime
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final
WORKDIR /app

# Create non-root user for security
RUN adduser --disabled-password --gecos '' appuser && chown -R appuser /app
USER appuser

# Copy published app
COPY --from=publish /app/publish .

# Environment variables
ENV ASPNETCORE_ENVIRONMENT=Production
ENV ASPNETCORE_URLS=http://+:5000
ENV DOTNET_RUNNING_IN_CONTAINER=true

# Health check
HEALTHCHECK --interval=30s --timeout=3s --start-period=5s --retries=3 \
    CMD curl -f http://localhost:5000/health || exit 1

# Expose port
EXPOSE 5000

# Start the application
ENTRYPOINT ["dotnet", "Senado.Api.dll"]
