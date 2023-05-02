FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["Services/Game/Game.Api/Game.Api.csproj", "Services/Game/Game.Api/"]
RUN dotnet restore "Services/Game/Game.Api/Game.Api.csproj"
COPY . .
WORKDIR "/src/Services/Game/Game.Api"
RUN dotnet build "Game.Api.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Game.Api.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Game.Api.dll"]
