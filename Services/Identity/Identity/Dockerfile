FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["Services/Identity/Identity/Identity.csproj", "Services/Identity/Identity/"]
RUN dotnet restore "Services/Identity/Identity/Identity.csproj"
COPY . .
WORKDIR "/src/Services/Identity/Identity"
RUN dotnet build "Identity.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Identity.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Identity.dll"]
