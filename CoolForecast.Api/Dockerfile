FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 8080

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY ["CoolForecast.Api/CoolForecast.Api.csproj", "CoolForecast.Api/"]
RUN dotnet restore "CoolForecast.Api/CoolForecast.Api.csproj"
COPY . .
WORKDIR "/src/CoolForecast.Api"
RUN dotnet build "CoolForecast.Api.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "CoolForecast.Api.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "CoolForecast.Api.dll"]
