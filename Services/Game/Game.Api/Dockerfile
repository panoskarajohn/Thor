#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["Services/Game/Game.Api/Game.Api.csproj", "Services/Game/Game.Api/"]
COPY ["Shared/Shared.Community.Web/Shared.Community.Web.csproj", "Shared/Shared.Community.Web/"]
COPY ["Shared/Shared.CQRS/Shared.CQRS.csproj", "Shared/Shared.CQRS/"]
COPY ["Shared/Shared.Attributes/Shared.Attributes.csproj", "Shared/Shared.Attributes/"]
COPY ["Shared/Shared.Logging/Shared.Logging.csproj", "Shared/Shared.Logging/"]
COPY ["Shared.Extensions/Shared.Extensions.csproj", "Shared.Extensions/"]
COPY ["Shared/Shared.Web/Shared.Web.csproj", "Shared/Shared.Web/"]
COPY ["Services/Game/Game.Application/Game.Application.csproj", "Services/Game/Game.Application/"]
COPY ["Services/Game/Game.Infrastructure/Game.Infrastructure.csproj", "Services/Game/Game.Infrastructure/"]
COPY ["Shared/Shared.Redis/Shared.Redis.csproj", "Shared/Shared.Redis/"]
RUN dotnet restore "Services/Game/Game.Api/Game.Api.csproj"
COPY . .
WORKDIR "/src/Services/Game/Game.Api"
RUN dotnet build "Game.Api.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Game.Api.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Game.Api.dll"]