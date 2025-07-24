FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 8080

FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["src/GenericWebApp/GenericWebApp.csproj", "src/GenericWebApp/"]
COPY ["src/Application/Application.csproj", "src/Application/"]
COPY ["src/Domain/Domain.csproj", "src/Domain/"]
COPY ["Framework/Framework.Common/Framework.Common.csproj", "Framework/Framework.Common/"]
COPY ["Framework/Framework.CQRS/Framework.CQRS.csproj", "Framework/Framework.CQRS/"]
COPY ["Framework/Framework.Database/Framework.Database.csproj", "Framework/Framework.Database/"]
COPY ["Framework/Framework.Database.MongoDB/Framework.Database.MongoDB.csproj", "Framework/Framework.Database.MongoDB/"]
COPY ["Framework/Framework.DI.Autofac/Framework.DI.Autofac.csproj", "Framework/Framework.DI.Autofac/"]
COPY ["Framework/Framework.Domain/Framework.Domain.csproj", "Framework/Framework.Domain/"]
COPY ["Framework/Framework.Domain.Events/Framework.Domain.Events.csproj", "Framework/Framework.Domain.Events/"]
COPY ["Framework/Framework.Domain.Repository/Framework.Domain.Repository.csproj", "Framework/Framework.Domain.Repository/"]
COPY ["Framework/Framework.EventManager/Framework.EventManager.csproj", "Framework/Framework.EventManager/"]
COPY ["Framework/Framework.EventManager.MongoDb/Framework.EventManager.MongoDb.csproj", "Framework/Framework.EventManager.MongoDb/"]
COPY ["Framework/Framework.Security/Framework.Security.csproj", "Framework/Framework.Security/"]
COPY ["Framework/Framework.Specification/Framework.Specification.csproj", "Framework/Framework.Specification/"]
RUN dotnet restore "src/GenericWebApp/GenericWebApp.csproj"
COPY . .
RUN dotnet build "src/GenericWebApp/GenericWebApp.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "src/GenericWebApp/GenericWebApp.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "GenericWebApp.dll"]
